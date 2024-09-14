using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.DataModels.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<UserModel> Users { get; set; }
    }
}
