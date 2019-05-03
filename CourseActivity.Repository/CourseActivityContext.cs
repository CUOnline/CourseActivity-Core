using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseActivity.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseActivity.Repository
{
    public class CourseActivityContext : DbContext
    {

        public CourseActivityContext(DbContextOptions<CourseActivityContext> options) : base(options) { }

        public DbSet<CourseUpload> CourseUploads { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
