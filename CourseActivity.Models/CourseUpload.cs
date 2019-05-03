using System.Text;
using System.Threading.Tasks;

namespace CourseActivity.Models
{
    public class CourseUpload : ModelBase
    {
        public string CourseId { get; set; }
        public string CSVData { get;set; }
    }
}