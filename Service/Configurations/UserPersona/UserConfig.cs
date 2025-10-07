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
    public class UserConfig : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(x => x.UserID);
            builder.Property(x => x.UserID).UseIdentityColumn();



            builder.Property(x => x.UserName).IsRequired().HasMaxLength(255);
            builder.HasIndex(x => x.UserName).IsUnique();

            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(255);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(255);


            builder.Property(x => x.UserEmail).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.UserEmail).IsUnique();
            builder.ToTable(tb => tb.HasCheckConstraint("CK_Users_Email_Gmail", "[UserEmail] LIKE '%@%.com'"));

            builder.Property(x => x.PasswordHash);
            builder.Property(x => x.PasswordSlat);
        }
    }
}
