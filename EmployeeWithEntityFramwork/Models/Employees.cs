using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeWithEntityFramwork.Models
{
    public class Employees
    {
        public int Id { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FirstNameRequired")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Department", ResourceType = typeof(Resource))]
        public string Department { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmployeeImage")]
        [NotMapped]
        public HttpPostedFileBase EmployeeImage { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string Image { get; set; }

        public List<Employees> AllEmployees { get; set; }
    }
}