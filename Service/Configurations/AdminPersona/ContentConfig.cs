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
    internal class ContentConfig : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            builder.HasKey(x => x.contentID);
            builder.Property(x => x.contentID).UseIdentityColumn();
            builder.Property(x => x.contentName).IsRequired().HasMaxLength(255);

            builder.Property(x => x.LecturesNumber).IsRequired();
            builder.Property(x => x.contentHour).IsRequired();

            builder.HasOne(x => x.Courses).
                WithMany(x => x.content).
                HasForeignKey(x => x.courseID).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
