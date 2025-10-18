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
    internal class ChatMessageConfig : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasKey(e => e.MessageID);
            builder.Property(e => e.MessageText).IsRequired();
            builder.Property(e => e.SentDateTime).IsRequired();

            builder.HasOne(e => e.Sender)
                .WithMany()
                .HasForeignKey(e => e.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Receiver)
                .WithMany()
                .HasForeignKey(e => e.ReceiverID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    
}
