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
    internal class categoryConfig : IEntityTypeConfiguration<categories>
    {
        public void Configure(EntityTypeBuilder<categories> builder)
        {
            builder.HasKey(x => x.categoryID);
            builder.Property(x => x.categoryID).UseIdentityColumn();
            builder.Property(x => x.categoryName).IsRequired().HasMaxLength(255);
            builder.HasIndex(x => x.categoryName).IsUnique();

            builder.Property(x => x.categoryDescription).IsRequired();


            builder.Property(x => x.categoryImagePath).IsRequired();
        }
    }
}
