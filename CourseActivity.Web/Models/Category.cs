using System.Collections.Generic;

namespace CourseActivity.Web.Models
{
    public class Category : ActivityItem
    {
        public List<Item> Items { get; set; }
    }
}