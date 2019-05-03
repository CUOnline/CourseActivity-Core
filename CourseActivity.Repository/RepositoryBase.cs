using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivity.Repository
{
    public abstract class RepositoryBase
    {
        protected readonly CourseActivityContext Context;

        protected RepositoryBase(CourseActivityContext context)
        {
            Context = context;
        }
    }
}
