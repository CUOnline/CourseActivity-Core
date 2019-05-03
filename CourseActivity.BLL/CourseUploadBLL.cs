using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CourseActivity.Interfaces.BLL;
using CourseActivity.Interfaces.Repository;
using CourseActivity.Models;

namespace CourseActivity.BLL
{
    public class CourseUploadBLL : ICourseUploadBLL
    {
        private readonly ICourseUploadRepository courseUploadRepository;

        public CourseUploadBLL(ICourseUploadRepository courseUploadRepository)
        {
            this.courseUploadRepository = courseUploadRepository;
        }

        public CourseUpload Add(CourseUpload model)
        {
            model.DateCreated = DateTime.Now;
            return courseUploadRepository.Add(model);
        }

        public IQueryable<CourseUpload> GetAll()
        {
            return courseUploadRepository.GetAll();
        }

        public CourseUpload Get(int modelId)
        {
            return courseUploadRepository.Get(modelId);
        }

        public CourseUpload Update(CourseUpload model)
        {
            model.LastUpdated = DateTime.Now;
            return courseUploadRepository.Update(model);
        }

        public void Delete(int modelId)
        {
            var course = Get(modelId);
            courseUploadRepository.Delete(course);
        }
    }
}