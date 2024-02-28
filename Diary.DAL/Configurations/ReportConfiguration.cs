using Diary.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.DAL.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(2000);

            builder.HasData(new List<Report>()
            {
                new Report()
                {
                    Id = 1,
                    Name = "TurboReport",
                    UserId = 1,
                    Description = "80' hits + modern words",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 0,
                }
            });

        }
    }
}
