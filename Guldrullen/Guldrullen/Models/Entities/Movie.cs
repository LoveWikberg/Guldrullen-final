using System;
using System.Collections.Generic;

namespace Guldrullen.Models.Entities
{
    public partial class Movie
    {
        public Movie()
        {
            Review = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Length { get; set; }
        public string Genre { get; set; }
        public string About { get; set; }
        public string Trailer { get; set; }

        public virtual ICollection<Review> Review { get; set; }
    }
}
