using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.DataBase.UserPersona;

namespace Service.Configurations.UserPersona
{
    public class UserCoursesDetailConfig : IEntityTypeConfiguration<UserCoursesDetail>
    {
        public void Configure(EntityTypeBuilder<UserCoursesDetail> builder)
        {
            builder.HasKey(x => x.UserCoursesDetailID);
            builder.Property(x => x.UserCoursesDetailID).UseIdentityColumn();



            builder.HasOne(x => x.userCoursesHeader).
                WithMany(x => x.userCoursesDetails).
                HasForeignKey(x => x.UserCoursesHeaderID).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Courses).
                WithMany(x => x.userCoursesDetails).
                HasForeignKey(x => x.courseID).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
