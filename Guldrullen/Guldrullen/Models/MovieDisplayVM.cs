using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guldrullen.Models
{
    public class MovieDisplayVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Trailer{ get; set; }

        public string InfoText { get; set; }

        public string Genre { get; set; }

        public int Length { get; set; }

        public double Rate { get; set; }
    }
}
