using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyaStore.Services.AuthAPI.Models.DTO
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}