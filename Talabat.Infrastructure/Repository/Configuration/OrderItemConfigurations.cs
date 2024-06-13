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
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder.OwnsOne(i => i.Product,
                product => product.WithOwner());
     

            builder.Property(i => i.Price).HasColumnType("decimal(18,2)");
         }
    }
}
