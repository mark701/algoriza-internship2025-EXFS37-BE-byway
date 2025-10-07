using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.PaymentMethod;
using Domain.Models.DataBase.UserPersona;
using Repository.Implemnetion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {

        IAdminRepository Admins { get; }

        ICategoryRepository categories { get; }
        IBaseRepository<Content> Content { get; }

        ICourseRepoistory Courses { get; }
        IInstructorRepository instructors { get; }
        IBaseRepository<JobTitle> JobTitle { get; }
        IBaseRepository<CreditCardPayment> CreditCardPayment { get; }

        IBaseRepository<PaypalPayment> PaypalPayment { get; }

        IUserCourse UserCoursesHeader { get; }

        IBaseRepository<UserCoursesDetail> UserCoursesDetail { get; }

        IUserRepository Users { get; }


        Task<int> SaveChangesAsync();


    }
}
