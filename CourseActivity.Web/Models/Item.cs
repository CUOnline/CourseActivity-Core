using System.Collections.Generic;

namespace CourseActivity.Web.Models
{
    public class Item : ActivityItem
    {
        public List<ActivityItem> Students { get; set; }
    }
}