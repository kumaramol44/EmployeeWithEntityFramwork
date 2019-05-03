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

        public CollectResponseType Response { get; set; }
        public UserInfoType ResponseCode { get; set; }
        public RpServicePortTypeClient Client { get; set; }
        public OrderResponseType Order { get; set; }
        public List<ProgressStatusType> AllProgressStatusTypes { get; set; }
        public string TempText { get; set; }
        public bool OpenMobileApp { get; set; }
        public string OrderRef { get; set; }

    }
}
