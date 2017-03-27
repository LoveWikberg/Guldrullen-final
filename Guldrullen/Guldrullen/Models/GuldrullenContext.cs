using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Guldrullen.Models.Entities
{
    public partial class GuldrullenContext : DbContext
    {
        public GuldrullenContext(DbContextOptions<GuldrullenContext> options) : base(options)
        {

        }

        public MovieDisplayVM[] ListMovies(string title, string genre)
        {
            var ret = Movie
                .Select(m => new MovieDisplayVM
                {
                    Id = m.Id,
                    Title = m.Title,
                    Genre = m.Genre,
                    Length = m.Length,
                })
            .Where(m => m.Title.Contains(title) && m.Genre.Contains(genre))
            .OrderBy(m => m.Title)
            .ToArray();

            foreach (var movie in ret)
            {
                var ratings = this.Review
                    .Where(o => o.MovieId == movie.Id)
                    .Select(o => o.Rate)
                    .ToArray();

                if (ratings.Length > 0)
                {
                    movie.Rate = ratings
                          .Average();

                    if (movie.Rate.ToString().Length > 2)
                        movie.Rate = Math.Round(movie.Rate, 1);
                }
            }
            return ret;
        }

        public MovieIndexTopRatedVM[] GetTopTenMovies()
        {
            var topFiveMovies = ListMovies("", "")
                .OrderByDescending(m => m.Rate).Take(10)
                .Select(m => new MovieIndexTopRatedVM
                {
                    Title = m.Title,
                    Rate = m.Rate.ToString(),
                    Id = m.Id
                })
                .ToArray();
            return topFiveMovies;
        }

        public MovieIndexRecentlyAddedVM[] GetRecentlyAddedMovies()
        {
            var newMovies = ListMovies("", "")
                .OrderByDescending(m => m.Id).Take(10)
                .Select(m => new MovieIndexRecentlyAddedVM
                {
                    Id = m.Id,
                    Title = m.Title,
                    Genre = m.Genre,
                })
                .ToArray();
            return newMovies;
        }

        public int AddMovie(MovieCreateVM viewModel)
        {
            var movieToAdd = new Movie
            {
                Title = viewModel.Title,
                Length = viewModel.Length,
                Genre = viewModel.Genre,
                About = viewModel.About,
                Trailer = viewModel.Trailer,
            };


            Movie.Add(movieToAdd);
            SaveChanges();
            return movieToAdd.Id;
        }

        public void AddReview(ReviewCreateVM viewModel, int id)
        {
            var movie = Movie.SingleOrDefault(c => c.Id == id);

            var reviewToAdd = new Review
            {
                Title = viewModel.Title,
                Text = viewModel.Text,
                Rate = viewModel.SelectedRate,
                MovieId = movie.Id,
            };


            Review.Add(reviewToAdd);
            SaveChanges();
        }

        public MovieInfoVM[] ListReviews(int id)
        {
            //var movieTitle = Movie.SingleOrDefault(m => m.Id == id).Title;
            var reviews = Review
                .Where(c => c.MovieId == id)
                .Select(m => new MovieInfoVM
                {
                    ReviewTitle = m.Title,
                    Text = m.Text,
                    Rate = m.Rate,
                    Id = m.Id,
                    // Movie = movieTitle,
                }).ToArray();


            return reviews;

        }

        internal MovieDisplayVM GetMovieToShowOnReviewPage(int id)
        {
            var movie = Movie.SingleOrDefault(c => c.Id == id);
            return new MovieDisplayVM
            {
                Title = movie.Title,
                InfoText = movie.About,
                Id = movie.Id,
                Trailer = movie.Trailer,
                Genre = movie.Genre
            };
        }

        public MovieEditVM GetMovieForEdit(int id)
        {
            var movie = Movie.SingleOrDefault(c => c.Id == id);
            return new MovieEditVM
            {
                Title = movie.Title,
                About = movie.About,
                Genre = movie.Genre,
                Length = movie.Length,
                Trailer = movie.Trailer
            };
        }

        public void EditMovie(MovieEditVM viewModel, int id)
        {
            var movie = Movie.SingleOrDefault(c => c.Id == id);

            movie.Title = viewModel.Title;
            movie.About = viewModel.About;
            movie.Genre = viewModel.Genre;
            movie.Length = viewModel.Length;
            movie.Trailer = viewModel.Trailer;

            SaveChanges();
        }

        public IEnumerable<string> GetAllGenres()
        {
            return new List<string>
            {
                "Action",
                "Comedy",
                "Drama",
                "Kids",
                "Thriller",
                "Horror",
                "Documentary",
                "Adventure"
            };
        }

        public IEnumerable<SelectListItem> GetSelectedListItem(IEnumerable<string> elements)
        {
            var selectedGenre = new List<SelectListItem>();

            foreach (var element in elements)
            {
                selectedGenre.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }
            return selectedGenre;
        }

        public MovieDisplayVM[] GetNavBarSearchResult(string search)
        {
            var moviesToReturn = Movie.Select(m => new MovieDisplayVM
            {
                Title = m.Title,
                Id = m.Id
            }).Where(m => m.Title.Contains(search)).ToArray();

            return moviesToReturn;
        }
    }
}