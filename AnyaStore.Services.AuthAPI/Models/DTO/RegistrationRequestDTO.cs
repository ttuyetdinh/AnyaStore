using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Linq;

namespace AnyaStore.Services.AuthAPI.Models.DTO
{
    public class RegistrationRequestDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}