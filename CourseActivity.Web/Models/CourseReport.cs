using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CourseActivity.Web.Models
{
    public class CourseReport
    {            
        [JsonProperty("courseId")]
        public string CourseId { get; set; }

        [JsonProperty("csvData")]
        public string CsvData { get; set; }
    }
}
