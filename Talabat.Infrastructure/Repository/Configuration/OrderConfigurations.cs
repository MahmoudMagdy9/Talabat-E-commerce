using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastructure.Repository.Configuration
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, x => x.WithOwner());
            builder.Property(o => o.Status)
                .HasConversion(
                    orderStatus => orderStatus.ToString(),
                    orderStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatus)
                );

            builder.Property(o => o.Subtotal).HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
