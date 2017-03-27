using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Guldrullen.Models.Entities;
using Guldrullen.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Guldrullen.Controllers
{
    public class MoviesController : Controller
    {

        GuldrullenContext context;
        IHostingEnvironment hostingEnvironment;

        public MoviesController(GuldrullenContext context, IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.context = context;
        }


        // GET: /<controller>/

        [HttpGet]
        public IActionResult Index()
        {
            var models = new MovieIndexAdvancedVM();

            models.TopRatedMoviesViewModels = context.GetTopTenMovies();
            models.RecentlyAddedMoviesViewModels = context.GetRecentlyAddedMovies();

            return View(models);
        }

        [HttpGet]
        public IActionResult Display()
        {
            MovieDisplayAdvancedVM viewModel = new MovieDisplayAdvancedVM();

            viewModel.Filter = new FilterGenre
            {
                Genres = new List<GenreVM>
                {
                    new GenreVM {Id=1, TypeOfGenre="All genres"},
                    new GenreVM {Id=2, TypeOfGenre="Action"},
                    new GenreVM {Id=3, TypeOfGenre="Drama"},
                    new GenreVM {Id=4, TypeOfGenre="Comedy"},
                    new GenreVM {Id=5, TypeOfGenre="Kids"},
                    new GenreVM {Id=6, TypeOfGenre="Thriller"},
                    new GenreVM {Id=7, TypeOfGenre="Horror"},
                    new GenreVM {Id=8, TypeOfGenre="Documentary"},
                    new GenreVM {Id=9, TypeOfGenre="Adventure"},
                }
            };

            viewModel.Searchinfo = (string)TempData["SearchInfo"];
            viewModel.SearchBox = (string)TempData["Search"];

            string chosenGenre;
            if ((string)TempData["ChosenGenre"] == "All genres" || (string)TempData["ChosenGenre"] == null)
                chosenGenre = "";
            else
                chosenGenre = (string)TempData["ChosenGenre"];

            //If the user left the the searchbox empty, return all movies
            //for the selected genre.
            if (viewModel.SearchBox == null)
                viewModel.ListViewModels = context.ListMovies("", chosenGenre);
            else
                viewModel.ListViewModels = context.ListMovies(viewModel.SearchBox, chosenGenre);

            return View(viewModel);


        }

        [HttpPost]
        public IActionResult Display(MovieDisplayAdvancedVM viewModel)
        {
            TempData["ChosenGenre"] = viewModel.Filter.SelectedGenre;
            TempData["Search"] = viewModel.SearchBox;
            string Searchinfo = "Result for \"" + viewModel.SearchBox + "\" in genre \"" + viewModel.Filter.SelectedGenre + "\"";

            TempData["SearchInfo"] = Searchinfo;

            return RedirectToAction(nameof(MoviesController.Display));
        }

        public IActionResult Review()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var genres = context.GetAllGenres();

            var viewModel = new MovieCreateVM();

            viewModel.Genres = context.GetSelectedListItem(genres);
            viewModel = new MovieCreateVM();


            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieCreateVM viewModel)
        {
            var genres = context.GetAllGenres();

            viewModel.Genres = context.GetSelectedListItem(genres);

            if (!ModelState.IsValid)
                return View(viewModel);

            int id = context.AddMovie(viewModel);

            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "img");
            var file = viewModel.Image;
            if (file.Length > 0)
            {
                var filePath = Path.Combine(uploads, $"{id}{Path.GetExtension(file.FileName)}");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            return RedirectToAction(nameof(MoviesController.Display));
        }

        [HttpGet]
        public IActionResult Info(int id)
        {
            var viewModel = new MovieReviewAdvancedVM();
            viewModel.ListViewModels = context.ListReviews(id);
            viewModel.FormViewModel = context.GetMovieToShowOnReviewPage(id);
            //test.CreateReview = context.GetMovieToShowOnReviewPage();
            viewModel.CreateReview = new ReviewCreateVM
            {
                Rates = new List<RateVm>
                 {
                     new RateVm {Id = 1, Rating = "1"},
                     new RateVm {Id = 2, Rating = "2"},
                     new RateVm {Id = 3, Rating = "3"},
                     new RateVm {Id = 4, Rating = "4"},
                     new RateVm {Id = 5, Rating = "5"},
                     new RateVm {Id = 6, Rating = "6"},
                     new RateVm {Id = 7, Rating = "7"},
                     new RateVm {Id = 8, Rating = "8"},
                     new RateVm {Id = 9, Rating = "9"},
                     new RateVm {Id = 10, Rating = "10"},
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Info(ReviewCreateVM viewModel, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            context.AddReview(viewModel, id);
            return RedirectToAction(nameof(MoviesController.Info));
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = context.GetMovieForEdit(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(MovieEditVM viewModel, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            context.EditMovie(viewModel, id);

            return RedirectToAction(nameof(MoviesController.Display));
        }

        public IActionResult GetMovies(string id)
        {
            var viewModel = context.GetNavBarSearchResult(id);

            return PartialView("NavBarSearch", viewModel);
        }

    }
}
