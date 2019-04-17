using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EmployeeWithEntityFramwork.BankIdService;

namespace EmployeeWithEntityFramwork.Models
{
    public class Login
    {
        [Display(Name = "PersonalNumber", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PersonalRequired")]
        public string PersonalNumber { get; set; }

        public string LoaderInfo { get; set; }
        public Login()
        {

        }
    }
}
