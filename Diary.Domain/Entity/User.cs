using Diary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Domain.Entity
{
    public class User : IEntityId<long>, IAuditable
    {
        public long Id { get; set; }
        public string Login {  get; set; }
        public string Password { get; set; }
        public List<Report> Reports {  get; set; }                      // one user - many reports, 1 - *
        public UserToken UserToken { get; set; }                        // one user - one token, 1 - 1
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }
}