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
    public class instructorConfig : IEntityTypeConfiguration<instructors>
    {
        public void Configure(EntityTypeBuilder<instructors> builder)
        {
            builder.HasKey(x => x.instructorID);
            builder.Property(x => x.instructorID).UseIdentityColumn();
            builder.Property(x => x.instructorName).IsRequired().HasMaxLength(255);
            builder.Property(x => x.courseRate).IsRequired();

            builder.ToTable(tb => tb.HasCheckConstraint("CK_Course_Rate_0<x<=5", "[courseRate] > 0 AND [courseRate] <= 5"));

            builder.Property(x => x.instructorDescription).IsRequired();
            builder.Property(x => x.instructorImagePath).IsRequired();

            builder.HasOne(x => x.jobTitle).
                WithMany(x => x.instructors).
                HasForeignKey(x => x.jobTitleID).OnDelete(DeleteBehavior.Restrict);
            ;
        }
    }
}
