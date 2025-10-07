using Domain.Models.DataBase.AdminPersona;
using Domain.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IInstructorRepository : IBaseRepository<instructors>
    {
        Task<instructors> AddInstructor(InstructorRequest instructorRequest);

        Task<instructors> UpdateInstructor(InstructorRequest instructorRequest);
        Task<List<object>> GetInstructorStudentsCount();

    }
}
