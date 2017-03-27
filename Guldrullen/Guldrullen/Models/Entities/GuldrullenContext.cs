using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Guldrullen.Models.Entities
{
    public partial class GuldrullenContext : DbContext
    {
        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}