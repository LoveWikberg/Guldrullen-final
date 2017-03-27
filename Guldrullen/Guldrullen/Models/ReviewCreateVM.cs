using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Guldrullen.Models
{
    [Bind(Prefix = nameof(MovieReviewAdvancedVM.CreateReview))]
    public class ReviewCreateVM
    {
        [Display(Name = "Rate")]
        [Required(ErrorMessage = "*Required")]
        public int SelectedRate { set; get; }
        public IEnumerable<RateVm> Rates { set; get; }
        //public int Rate { get; set; }


        [Display(Name = "Title")]
        [MaxLength(50, ErrorMessage = "Only 50 character allowed")]
        [MinLength(7, ErrorMessage = "Title must contain atleast 7 characters")]
        public string Title { get; set; }

        [Display(Name = "Text")]
        [MaxLength(1500, ErrorMessage = "Only 1500 character allowed")]
        public string Text { get; set; }
    }

    public class RateVm
    {
        public int Id { set; get; }
        public string Rating { set; get; }
    }
}
