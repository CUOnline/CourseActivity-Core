using System;

namespace CourseActivity.Web.Models
{
    public class ActivityItem
    {
        public string Name { get; set; }
        public int? Views { get; set; }
        public int? Participations { get; set; }
        public DateTime? FirstAccess { get; set; }
        public DateTime? LastAccess { get; set; }
    }
}