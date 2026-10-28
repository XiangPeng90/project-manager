using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class UserRole
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
    }
}