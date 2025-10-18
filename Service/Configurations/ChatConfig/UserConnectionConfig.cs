using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Configurations.ChatConfig
{
    internal class UserConnectionConfig : IEntityTypeConfiguration<UserConnection>
    {
        public void Configure(EntityTypeBuilder<UserConnection> builder)
        {
            builder.HasKey(e => e.ConnectionID);
            builder.Property(e => e.ConnectedAt).IsRequired();

            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
