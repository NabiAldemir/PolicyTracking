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
    public class PolicyTypeConfiguration : IEntityTypeConfiguration<PolicyType>
    {
        public void Configure(EntityTypeBuilder<PolicyType> builder)
        {
            PolicyType policyType1 = new()
            {
                Id = 1,
                Name = "Trafik",
                CreateDate = DateTime.Now,
            };

            PolicyType policyType2 = new()
            {
                Id = 2,
                Name = "Kasko",
                CreateDate = DateTime.Now,
            };

            PolicyType policyType3 = new()
            {
                Id = 3,
                Name = "Dask",
                CreateDate = DateTime.Now,
            };

            PolicyType policyType4 = new()
            {
                Id = 4,
                Name = "Konut",
                CreateDate = DateTime.Now,
            };

            PolicyType policyType5 = new()
            {
                Id = 5,
                Name = "İş Yeri",
                CreateDate = DateTime.Now,
            };

            PolicyType policyType6 = new()
            {
                Id = 6,
                Name = "Sağlık",
                CreateDate = DateTime.Now,
            };

            builder.HasData(
                policyType1,
                policyType2,
                policyType3,
                policyType4,
                policyType5,
                policyType6
            );
        }
    }
}
