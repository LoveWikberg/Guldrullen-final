using Guldrullen.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guldrullen.Models
{
    public class MovieReviewAdvancedVM
    {
        public MovieInfoVM[] ListViewModels { get; set; }
        public MovieDisplayVM FormViewModel { get; set; }
        public ReviewCreateVM CreateReview { get; set; }

        public string UserName { get; set; }
    }
}
