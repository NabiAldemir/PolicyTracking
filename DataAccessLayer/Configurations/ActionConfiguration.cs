using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EntityLayer.Configurations
{
    public class ActionConfiguration : IEntityTypeConfiguration<Actions>
    {
        public void Configure(EntityTypeBuilder<Actions> builder)
        {
            Actions actions1 = new()
            {
                Id = 1,
                Name = "Add",
                Description = "Ekleme İşlemi",
                Status = true,
                CreateDate = DateTime.Now
            };

            Actions actions2 = new()
            {
                Id = 2,
                Name = "Update",
                Description = "Güncelleme İşlemi",
                Status = true,
                CreateDate = DateTime.Now
            };

            Actions actions3 = new()
            {
                Id = 3,
                Name = "Delete",
                Description = "Silme İşlemi",
                Status = true,
                CreateDate = DateTime.Now
            };
            builder.HasData(
                actions1,
                actions2,
                actions3
            );
        }
    }
}
