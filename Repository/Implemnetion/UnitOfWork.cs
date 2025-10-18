using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.PaymentMethod;
using Domain.Models.DataBase.UserPersona;
using Repository.Interface;
using Service.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implemnetion
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public IAdminRepository Admins { get; private set; }


        public IChatService ChatService { get; private set; }
        public ICategoryRepository categories { get; private set; }

        public IBaseRepository<Content> Content { get; private set; }

        public ICourseRepoistory Courses { get; private set; }

        public IInstructorRepository instructors { get; private set; }

        public IBaseRepository<JobTitle> JobTitle { get; private set; }

        public IBaseRepository<CreditCardPayment> CreditCardPayment { get; private set; }

        public IBaseRepository<PaypalPayment> PaypalPayment { get; private set; }

        public IUserCourse UserCoursesHeader { get; private set; }

        public IBaseRepository<UserCoursesDetail> UserCoursesDetail { get; private set; }

        public IUserRepository Users { get; private set; }

        public UnitOfWork(ApplicationDbContext context, IAuthService authService,IEmailService emailService)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;

            Admins = new AdminRepository(_context, _authService);
            categories = new CategoryRepository(_context);
            Content = new BaseRepository<Content>(_context);
            Courses = new CourseRepoistory(_context,this, _authService);
            instructors = new InstructorRepository(_context);
            JobTitle = new BaseRepository<JobTitle>(_context);
            CreditCardPayment = new BaseRepository<CreditCardPayment>(_context);
            PaypalPayment = new BaseRepository<PaypalPayment>(_context);
            UserCoursesHeader = new UserCourseRepository(_context, _authService,this,_emailService);
            UserCoursesDetail = new BaseRepository<UserCoursesDetail>(_context);
            Users = new UserRepository(_context, _authService, _emailService);

            ChatService = new ChatService(_context);


        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
