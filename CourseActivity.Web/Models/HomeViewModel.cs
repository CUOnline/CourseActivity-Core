using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseActivity.Web.Models
{
    public class HomeViewModel
    {
        public string CanvasUrl { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public bool Authorized { get; set; }
        public List<CourseCsvEntry> CourseData { get; set; }
    }
}
