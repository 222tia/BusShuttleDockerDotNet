using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DomainModel;

namespace WebMvc.Models {
    public class LoginViewModel {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}