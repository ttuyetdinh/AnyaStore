using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Linq;

using static AnyaStore.Web.Ultilities.SD;
namespace AnyaStore.Web.Models.DTO
{
    public class RegistrationRequestDTO
    {
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public Role? Role { get; set; }
    }
}