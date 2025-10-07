using Domain.Models.DataBase.AdminPersona;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Configurations.AdminPersona
{
    internal class CoursesConfig : IEntityTypeConfiguration<Courses>
    {
        public void Configure(EntityTypeBuilder<Courses> builder)
        {
            builder.HasKey(x => x.courseID);
            builder.Property(x => x.courseID).UseIdentityColumn();
            builder.Property(x => x.courseName).IsRequired().HasMaxLength(255);

            builder.Property(x => x.courseLevel).IsRequired();

            builder.Property(x => x.courseRate).IsRequired();

            builder.ToTable(tb => tb.HasCheckConstraint("CK_Course_Rate_0<x<=5", "[courseRate] > 0 AND [courseRate] <= 5"));

            builder.Property(x => x.courseHours).IsRequired();
            builder.Property(c => c.courseHours).HasColumnType("decimal(7,2)");

            builder.Property(x => x.CoursePrice).IsRequired();


            builder.Property(x => x.CourseDescription).IsRequired();
            builder.Property(x => x.CourseCertification).IsRequired();
            builder.Property(x => x.CourseImagePath).IsRequired();

            builder.HasOne(x => x.categories).
                WithMany(x => x.courses).
                HasForeignKey(x => x.categoryID).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.instructors).
                WithMany(x => x.courses).
                HasForeignKey(x => x.instructorID).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
