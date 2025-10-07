using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.UserPersona;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Configurations.UserPersona
{
    public class UserCoursesHeaderConfig : IEntityTypeConfiguration<UserCoursesHeader>
    {
        public void Configure(EntityTypeBuilder<UserCoursesHeader> builder)
        {
            builder.HasKey(x => x.UserCoursesHeaderID);
            builder.Property(x => x.UserCoursesHeaderID).UseIdentityColumn();
         


            builder.HasOne(x => x.users).
                WithMany(x => x.UserCourses).
                HasForeignKey(x => x.UserID).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
