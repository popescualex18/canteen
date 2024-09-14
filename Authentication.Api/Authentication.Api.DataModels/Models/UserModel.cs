using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.DataModels.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string SurName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }

        public virtual RoleModel Role { get; set; }
    }
}
