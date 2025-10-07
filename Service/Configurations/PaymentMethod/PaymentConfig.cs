using Domain.Models.DataBase.PaymentMethod;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Configurations.PaymentMethod
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {

            // Common configuration for all payments
            builder.HasKey(x => x.PaymentID);
            builder.Property(x => x.PaymentID).UseIdentityColumn();
            builder.Property(x => x.UserID).IsRequired();
            builder.Property(x => x.Country).IsRequired().HasMaxLength(100);
            builder.Property(x => x.State).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.CreatedAt).IsRequired();

            
            builder.HasDiscriminator<string>("PaymentType")
                .HasValue<CreditCardPayment>("CreditCard")
                .HasValue<PaypalPayment>("PayPal");

            builder.HasOne(x => x.Users)
                   .WithMany(x => x.payments)
                   .HasForeignKey(x => x.UserID)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
