using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5.Models
{
    public class UserSettings
    {
        public int Id { get; set; }

        public string NetGuid { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
