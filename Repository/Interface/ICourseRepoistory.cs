using Domain.Models.DataBase.AdminPersona;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICourseRepoistory:  IBaseRepository<Courses>
    {
        Task<Courses> AddCourse(CoursesRequest coursesRequest);
        Task<Courses> UpdateCourse(CoursesRequest coursesRequest);
        Task<(int totalCount, List<object> data)> GetPagesFilterCourse(int pageNumber, int pageSize,
            FilterCoursesUser filterRequest,
            Expression<Func<Courses, object>> orderBy,
            bool ascending = true);

        Task<List<object>> GetCourseCount();

        Task<object> GetInclude(int courseID);

    }
}
