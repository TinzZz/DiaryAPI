using Diary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Domain.Entity
{
    public class Report : IEntityId<long>, IAuditable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }                // one report - one user, 1 - 1
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
