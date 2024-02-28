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
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Property(s => s.RefreshToken).IsRequired();
            builder.Property(s => s.RefreshTokenExpiryTime).IsRequired();

            builder.HasData(new List<UserToken>()
            {
                new UserToken
                {
                    Id = 1,
                    RefreshToken = "asfjsdksdf",
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
                    UserId = 1
                }
            });
        }
    }
}
