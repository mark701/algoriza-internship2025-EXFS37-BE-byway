using Domain.Models.DataBase.AdminPersona;
using Microsoft.AspNetCore.Http;
using Repository.Interface;
using Service.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;
using Domain.Models.Requests;
using Repository.Extentions;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Domain.Models.DataBase.UserPersona;

namespace Repository.Implemnetion
{
    public class CourseRepoistory : BaseRepository<Courses>, ICourseRepoistory
    {
        private readonly ApplicationDbContext _context;
        private readonly string PathImageCourses = "Assets/Courses";
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png", "gif" };

        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _AuthService;
        public CourseRepoistory(ApplicationDbContext context, UnitOfWork unitOfWork, IAuthService authService) : base(context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _AuthService = authService;
        }

        public async Task<Courses> AddCourse(CoursesRequest coursesRequest)
        {



            if (coursesRequest.CourseImage == null || coursesRequest.CourseImage.Length == 0)
                throw new ArgumentException("Image file is required.");

            var CheckPixels = CustomImages.CheckWidthHeightImage(coursesRequest.CourseImage, 700, 430);

            if (!CheckPixels)
            {
                throw new ArgumentException("Image dimensions must be 700x430 pixels or smaller.");
            }
            long maxSize = 5 * 1024 * 1024; // 5 MB
            if (coursesRequest.CourseImage.Length > maxSize)
            {
                throw new ArgumentException("Image size must not exceed 5 MB.");

            }
            if (coursesRequest.content == null || !coursesRequest.content.Any())
            {
                throw new ArgumentException("The course must have at least one content item.");
            }
            var path = await SaveImageAsync(coursesRequest.CourseImage, allowedExtensions, PathImageCourses);

            Courses courses = new Courses
            {
                courseID = coursesRequest.courseID,
                courseName = coursesRequest.courseName,
                categoryID = coursesRequest.categoryID,
                instructorID = coursesRequest.instructorID,
                courseLevel = coursesRequest.courseLevel,
                courseRate = coursesRequest.courseRate,
                courseHours = coursesRequest.courseHours,
                CoursePrice = coursesRequest.CoursePrice,
                CourseDescription = coursesRequest.CourseDescription,
                CourseCertification = coursesRequest.CourseCertification,
                CourseImagePath = path,
                content = coursesRequest.content,
            };

            var data = await Add(courses);
            await _context.SaveChangesAsync();

            return data;

        }

        public async Task<Courses> UpdateCourse(CoursesRequest coursesRequest)
        {
            var Data = await Find(x => x.courseID == coursesRequest.courseID, x => x.content);


            if (Data != null)
            {
                bool isBoughtByAnyUser = _context.Set<UserCoursesDetail>().Any(UserCourseDetails => UserCourseDetails.courseID == Data.courseID);
                if (isBoughtByAnyUser)
                {
                    throw new Exception("Cant Update That Course Becaause 1 or more User Bought that Course");
                }

                if (coursesRequest.CourseImage != null && coursesRequest.CourseImage.Length != 0)
                {


                    var CheckPixels = CustomImages.CheckWidthHeightImage(coursesRequest.CourseImage, 700, 430);
                    long maxSize = 5 * 1024 * 1024;

                    if (!CheckPixels)
                    {
                        throw new ArgumentException("Image dimensions must be 700x430 pixels or smaller.");
                    }

                    else if (coursesRequest.CourseImage.Length > maxSize)
                    {
                        throw new ArgumentException("Image size must not exceed 5 MB.");

                    }
                    else
                    {
                        DeleteImage(Data.CourseImagePath);
                        Data.CourseImagePath = await SaveImageAsync(coursesRequest.CourseImage, allowedExtensions, PathImageCourses);

                    }

                }
                var DeletedContent = _unitOfWork.Content.CompareDiffernce(Data.content.ToList(), coursesRequest.content.ToList(), x => x.contentID);

                if (!DeletedContent.IsNullOrEmpty())
                {
                    _unitOfWork.Content.DeleteRange(DeletedContent);

                }
                Data.courseID = Data.courseID;
                Data.courseName = coursesRequest.courseName ?? Data.courseName;
                Data.categoryID = coursesRequest.categoryID != 0 ? coursesRequest.categoryID : Data.categoryID;
                Data.instructorID = coursesRequest.instructorID != 0 ? coursesRequest.instructorID : Data.instructorID;
                Data.courseLevel = coursesRequest.courseLevel != 0 ? coursesRequest.courseLevel : Data.courseLevel;
                Data.courseRate = coursesRequest.courseRate != 0 ? coursesRequest.courseRate : Data.courseRate;
                Data.courseHours = coursesRequest.courseHours != 0 ? coursesRequest.courseHours : Data.courseHours;
                Data.CoursePrice = coursesRequest.CoursePrice != 0 ? coursesRequest.CoursePrice : Data.CoursePrice;
                Data.CourseDescription = coursesRequest.CourseDescription ?? Data.CourseDescription;
                Data.CourseCertification = coursesRequest.CourseCertification ?? Data.CourseCertification;
                Data.content = (coursesRequest.content != null && coursesRequest.content.Any()) ? coursesRequest.content : Data.content;
                Update(Data);
                await _context.SaveChangesAsync();








            }

            return Data;
        }


        public async Task<(int totalCount, List<object> data)> GetPagesFilterCourse(int pageNumber, int pageSize, FilterCoursesUser filterRequest,
            Expression<Func<Courses, object>> orderBy,
            bool ascending = true)
        {
            var UserID = 0;
            var ID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
            if (ID != null)
            {
                if (int.TryParse(ID, out int userId))
                {
                    UserID = userId;
                }
            }


            var maxPrice = filterRequest.MaxPrice > 0
                ? filterRequest.MaxPrice
                : await GetMaxAsync(x => x.CoursePrice);

            var filter = Filter(filterRequest, maxPrice);

            IQueryable<Courses> query = _context.Set<Courses>().Include(c => c.categories).Include(c => c.instructors).Include(c => c.content);

            // Apply filters
            if (filter != null)
            {
                foreach (var criteria in filter)
                {
                    query = query.Where(criteria);
                }
            }

            var totalCount = await query.CountAsync();

            // Apply ordering
            if (orderBy != null)
            {
                query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            }

            // Apply pagination
            var data = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(Course => new
                    {
                        Course.courseID,
                        Course.courseName,
                        Course.courseRate,
                        Course.CourseImagePath,
                        Course.courseHours,
                        Course.CoursePrice,
                        Course.courseLevel,
                        categoryName = Course.categories.categoryName,
                        instructorName = Course.instructors.instructorName,
                        LectureSum = Course.content.Sum(x => x.LecturesNumber),
                        IsBought = _context.Set<UserCoursesDetail>().Any(UserCourseDetails =>
                        UserCourseDetails.courseID == Course.courseID && UserCourseDetails.userCoursesHeader.UserID == UserID)
                    })
                    .ToListAsync();



            return (totalCount, data.Cast<object>().ToList());

        }
        private List<Expression<Func<Courses, bool>>> Filter(FilterCoursesUser filterRequest, decimal maxPrice)
        {


            var filters = new List<Expression<Func<Courses, bool>>>();

            if (filterRequest.courseRate > 0)
                filters.Add(x => x.courseRate >= filterRequest.courseRate);

            if (filterRequest.MinPrice > 0)
                filters.Add(x => x.CoursePrice >= filterRequest.MinPrice);

            filters.Add(x => x.CoursePrice <= maxPrice);

            if (filterRequest.Category != null && filterRequest.Category.Length > 0)
                filters.Add(x => filterRequest.Category.Contains(x.categories.categoryName));

            if (filterRequest.MinLecture > 0)
                filters.Add(x => x.content.Sum(c => c.LecturesNumber) >= filterRequest.MinLecture);


            if (filterRequest.MaxLecture > 0)
                filters.Add(x => x.content.Sum(c => c.LecturesNumber) <= filterRequest.MaxLecture);
            return filters;

        }

        public async Task<List<object>> GetCourseCount()
        {
            var UserID = 0;
            var ID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
            if (ID != null)
            {
                if (int.TryParse(ID, out int userId))
                {
                    UserID = userId;
                }
            }


            var query = await _context.courses
                .Select(Course => new
                {
                    Course.courseID,
                    Course.courseName,
                    Course.courseRate,
                    Course.CourseImagePath,
                    categoryName = Course.categories.categoryName,
                    instructorName = Course.instructors.instructorName,
                    Course.courseHours,
                    Course.courseLevel,
                    Course.CoursePrice,
                    LectureSum = Course.content.Sum(c => (int?)c.LecturesNumber) ?? 0,
                    IsBought = _context.Set<UserCoursesDetail>().Any(UserCourseDetails =>
                    UserCourseDetails.courseID == Course.courseID && UserCourseDetails.userCoursesHeader.UserID == UserID)


                })
                .OrderByDescending(Course => Course.courseRate)
                .ThenByDescending(Course => Course.CoursePrice)
                .Take(10)
                .ToListAsync();

            return query.Cast<object>().ToList();

        }

        public async Task<object> GetInclude(int courseID)
        {
            var UserID = 0;
            var ID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);

            if (ID != null && int.TryParse(ID, out int userId))
            {
                UserID = userId;
            }

            var query = await _context.courses
                .Where(c => c.courseID == courseID)
                .Select(course => new
                {
                    course.courseID,
                    course.courseName,
                    course.instructorID,
                    course.categoryID,
                    course.courseRate,
                    course.CourseImagePath,
                    course.courseHours,
                    course.courseLevel,
                    course.CoursePrice,
                    course.CourseDescription,
                    course.CourseCertification,
                    course.CreateDateAndTime,
                    
                    categoryName = course.categories.categoryName,
                    categoryImagePath = course.categories.categoryImagePath,

                    instructorName = course.instructors.instructorName,
                    instructorImagePath = course.instructors.instructorImagePath,
                    instructorDescription = course.instructors.instructorDescription,
                    jobTitleName=course.instructors.jobTitle.JobTilteName,
                    CourseCount = course.instructors.courses.Count(),
                    LectureSum = course.content.Sum(content => (int?)content.LecturesNumber) ?? 0,
                    StudentsCount = _context.Set<UserCoursesDetail>().Where(ucd => ucd.Courses.instructorID == course.instructorID)
                        .Select(ucd => ucd.userCoursesHeader.UserID)
                        .Distinct()
                        .Count(),


                    IsBought = _context.Set<UserCoursesDetail>()
                        .Any(UserCourseDetails =>
                            UserCourseDetails.courseID == course.courseID &&
                            UserCourseDetails.userCoursesHeader.UserID == UserID),
                            // is for the user who is already login
                            // and check if that user bought that course or not 


                    // Get course contents
                    Contents = course.content.Select(content => new
                    {   
                        content.courseID,
                        content.contentID,
                        content.contentName,
                        content.LecturesNumber,
                        content.contentHour
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return query;
        }

    }
}
