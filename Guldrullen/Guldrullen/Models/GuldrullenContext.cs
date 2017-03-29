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

        // Method for displaying movies saved in the database.
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

        // Method for displaying the top ten rated movies on the index page
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

        // Method for displaying the recently added movies by using the highest id added.
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

        // Method for adding and saving new movies in the database. 
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

        // Method for adding review to a specific movie
        public void AddReview(ReviewCreateVM viewModel, int id, string userName)
        {
            var movie = Movie.SingleOrDefault(c => c.Id == id);

            var reviewToAdd = new Review
            {
                Title = viewModel.Title,
                Text = viewModel.Text,
                Rate = viewModel.SelectedRate,
                MovieId = movie.Id,
                User = userName
            };


            Review.Add(reviewToAdd);
            SaveChanges();
        }

        // Method for displaying reviews connected to a specific movie
        public MovieInfoVM[] ListReviews(int id)
        {
            var reviews = Review
                .Where(c => c.MovieId == id)
                .Select(r => new MovieInfoVM
                {
                    ReviewTitle = r.Title,
                    Text = r.Text,
                    Rate = r.Rate,
                    Id = r.Id,
                    User = r.User
                }).ToArray();


            return reviews;

        }

        // Method for displaying detailed movie info
        internal MovieDisplayVM DisplayMovieInfo(int id)
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

        // Gets info about the movie for the EditMovie method
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

        // Gets all the genres for the edit and create viewpages
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

        // Returns the user selected genre for the edited/created movie
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

        // Returns an array of the movies that matches the users search input
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