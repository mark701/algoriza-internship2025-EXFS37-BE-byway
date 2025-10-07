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
    public class AdminConfig : IEntityTypeConfiguration<Admins>
    {
        public void Configure(EntityTypeBuilder<Admins> builder)
        {
            builder.HasKey(x => x.adminID);
            builder.Property(x => x.adminID).UseIdentityColumn();
            builder.Property(x => x.adminName).IsRequired().HasMaxLength(255);
            builder.HasIndex(x => x.adminName).IsUnique();

            builder.Property(x => x.adminEmail).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.adminEmail).IsUnique();
            builder.ToTable(tb => tb.HasCheckConstraint("CK_Admins_Email_Gmail", "[adminEmail] LIKE '%@%.com'"));


        }
    }
}
