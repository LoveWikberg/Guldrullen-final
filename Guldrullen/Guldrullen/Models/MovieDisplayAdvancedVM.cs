using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Guldrullen.Models
{
    public class MovieDisplayAdvancedVM
    {
        public MovieDisplayVM[] ListViewModels { get; set; }
        public MovieCreateVM FormViewModels { get; set; }
        public FilterGenre Filter { get; set; }

        public string SearchBox { get; set; }
        public string Searchinfo { get; set; }

    }

    public class FilterGenre
    {
        [Display(Name = "Filter by genre")]
        public string SelectedGenre { set; get; }
        public IEnumerable<GenreVM> Genres { set; get; }
    }

    public class GenreVM
    {
        public int Id { set; get; }
        public string TypeOfGenre { set; get; }
    }

}
