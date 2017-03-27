using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guldrullen.Models
{
    public class MovieIndexAdvancedVM
    {
        public MovieIndexTopRatedVM[] TopRatedMoviesViewModels { get; set; }

        public MovieIndexRecentlyAddedVM[] RecentlyAddedMoviesViewModels { get; set; }
    }
}
