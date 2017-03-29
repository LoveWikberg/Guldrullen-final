using System;
using System.Collections.Generic;

namespace Guldrullen.Models.Entities
{
    public partial class Review
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rate { get; set; }
        public string User { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
