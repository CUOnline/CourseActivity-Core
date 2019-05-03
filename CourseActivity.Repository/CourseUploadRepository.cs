using System.Text;
using System.Threading.Tasks;
using System.Linq;
using CourseActivity.Models;
using CourseActivity.Interfaces.Repository;

namespace CourseActivity.Repository
{
    public class CourseUploadRepository : RepositoryBase, ICourseUploadRepository
    {
        public CourseUploadRepository(CourseActivityContext context) : base(context) { }

        public CourseUpload Add(CourseUpload model)
        {
            Context.CourseUploads.Add(model);
            Context.SaveChanges();
            return model;
        }

        public IQueryable<CourseUpload> GetAll()
        {
            return Context.CourseUploads;
        }

        public CourseUpload Get(int modelId)
        {
            return Context.CourseUploads.Find(modelId);
        }

        public CourseUpload Update(CourseUpload model)
        {
            Context.CourseUploads.Update(model);
            Context.SaveChanges();
            return model;
        }

        public void Delete(CourseUpload model)
        {
            Context.CourseUploads.Remove(model);
            Context.SaveChanges();
        }
    }
}