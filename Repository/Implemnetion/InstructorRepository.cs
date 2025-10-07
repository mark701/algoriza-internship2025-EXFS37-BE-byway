using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.UserPersona;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repository.Implemnetion
{
    public class InstructorRepository : BaseRepository<instructors>, IInstructorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string PathImageInstructor ="Assets/instructor";
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        public InstructorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<instructors> AddInstructor(InstructorRequest instructorRequest)
        {
            

            string imagePath = await SaveImageAsync(instructorRequest.InstructorImage, allowedExtensions, PathImageInstructor);

            instructors instructor = new instructors
            {
                jobTitleID = instructorRequest.jobTitleID,
                instructorName = instructorRequest.instructorName,
                instructorDescription = instructorRequest.instructorDescription,
                instructorImagePath = imagePath,
                courseRate = instructorRequest.courseRate
            };
            var data = await Add(instructor);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<List<object>> GetInstructorStudentsCount()
        {
            var query = await _context.instructors
                .Select(ins => new
                {
                    ins.instructorID,
                    ins.instructorName,
                    ins.instructorImagePath,
                    ins.courseRate,
                    JobTilteName = ins.jobTitle.JobTilteName,
                    StudentsCount = ins.courses
                        .SelectMany(c => c.userCoursesDetails)
                        .Select(ucd => ucd.userCoursesHeader.UserID)
                        .Distinct()
                        .Count()
                })
                .OrderByDescending(ins => ins.courseRate)
                .ThenByDescending(ins => ins.StudentsCount)
                .Take(10)
                .ToListAsync();

            return query.Cast<object>().ToList();

        }

        public async Task<instructors> UpdateInstructor(InstructorRequest instructorRequest)
        {
            var Data =  await Find(x => x.instructorID == instructorRequest.instructorID);
            
            if (Data != null)
            {


                if (instructorRequest.InstructorImage != null)
                {

                    DeleteImage(Data.instructorImagePath);
                    Data.instructorImagePath = await SaveImageAsync(instructorRequest.InstructorImage, allowedExtensions , PathImageInstructor);

                }





                Data.instructorID = Data.instructorID;
                Data.jobTitleID = instructorRequest.jobTitleID != 0 ? instructorRequest.jobTitleID : Data.jobTitleID;
                Data.instructorName = instructorRequest.instructorName ?? Data.instructorName;
                Data.instructorDescription = instructorRequest.instructorDescription ?? Data.instructorDescription;
                Data.courseRate= instructorRequest.courseRate == Data.courseRate ? Data.courseRate : instructorRequest.courseRate;







                Update(Data);
                await _context.SaveChangesAsync();
             
            }
            return Data;
        }
    }
}
