using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace EmployeeWithEntityFramwork
{
    [Table("Employee")]
    public partial class Employee
    {
        [Key]
        [Column(Order = 0, TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        
        [Column(Order = 1)]
        [StringLength(50)]
        public string Name { get; set; }

       
        [Column(Order = 2)]
        [StringLength(150)]
        public string Department { get; set; }

        
        [Column(Order = 3)]
        [MaxLength]
        public string Image { get; set; }
    }
}
