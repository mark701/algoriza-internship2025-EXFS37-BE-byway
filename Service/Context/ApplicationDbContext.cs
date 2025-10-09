using Domain.Enum;
using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.UserPersona;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<Payment> payments { get; set; }

        //public DbSet<Admins>admins { get; set; }

        public DbSet<categories> categories { get; set; }
        public DbSet<Content> contents { get; set; }

        public DbSet<Courses> courses{ get; set; }

        public DbSet<instructors> instructors{ get; set; }

        public DbSet<JobTitle> jobTitles { get; set; }

        public DbSet<UserCoursesHeader> userCoursesHeaders { get; set; }

        public DbSet<UserCoursesDetail> userCoursesDetails { get; set; }

        public DbSet<Users> users{ get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);


            //modelBuilder.Entity<JobTitle>().
            //    Property(x => x.JobTilteName).
            //    HasConversion(v => v.ToString(),v => (JobTitleType)Enum.Parse(typeof(JobTitleType), v) );

            //modelBuilder.Entity<categories>().
            //    Property(x => x.categoryName).
            //    HasConversion(v => v.ToString(), v => (CategoryNameType)Enum.Parse(typeof(CategoryNameType), v));




        }

    }
}
