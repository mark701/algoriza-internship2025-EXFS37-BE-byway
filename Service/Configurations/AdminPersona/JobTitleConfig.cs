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
    internal class JobTitleConfig : IEntityTypeConfiguration<JobTitle>
    {
        public void Configure(EntityTypeBuilder<JobTitle> builder)
        {
            builder.HasKey(x => x.JobTilteID);
            builder.Property(x => x.JobTilteID).UseIdentityColumn();
            builder.Property(x => x.JobTilteName).IsRequired();
            builder.HasIndex(x => x.JobTilteName).IsUnique();


        }
    }
}
