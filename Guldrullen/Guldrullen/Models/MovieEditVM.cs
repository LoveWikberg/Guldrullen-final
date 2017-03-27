using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Guldrullen.Models
{
    public class MovieEditVM
    {
        [Display(Name = "Title")]
        [MaxLength(100, ErrorMessage = "Only 100 character allowed")]
        [Required(ErrorMessage = "*Required")]
        public string Title { get; set; }

        [Display(Name = "About")]
        [MaxLength(300, ErrorMessage = "Only 300 character allowed")]
        [Required(ErrorMessage = "*Required")]
        public string About { get; set; }

        [Display(Name = "Length (In minutes)")]
        [Required(ErrorMessage = "*Required")]
        public int Length { get; set; }

        [Display(Name = "Genre")]
        [Required(ErrorMessage = "*Required")]
        public string Genre { get; set; }

        [Display(Name = "Trailer")]
        [Url(ErrorMessage = "The Trailer field is not a valid fully-qualified http, https, or ftp URL. Click the question mark for more information on how to do.")]
        public string Trailer { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; }

    }
}
