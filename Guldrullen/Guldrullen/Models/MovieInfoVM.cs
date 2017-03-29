using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guldrullen.Models
{
    public class MovieInfoVM
    {
        public string MovieTitle { get; set; }

        public int Id { get; set; }

        public string ReviewTitle { get; set; }

        public string Text { get; set; }

        public int Rate { get; set; }

        public int MovieId { get; set; }

        public string User { get; set; }
    }
}

