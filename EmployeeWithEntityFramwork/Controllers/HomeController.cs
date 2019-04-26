using EmployeeWithEntityFramwork.BankIdService;
using EmployeeWithEntityFramwork.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using System.Web.UI;
using Newtonsoft.Json;

namespace EmployeeWithEntityFramwork.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(Login loginModel)
        {

            if (loginModel.TempText != null)
            {
                ViewBag.Message = "User Logged In";
            }
            return View();
        }

        [HttpPost]
        public ContentResult GetData(Login loginModel)
        {
            var returnModel = new Login();
            if (loginModel.PersonalNumber == "198001018129")
            {
                returnModel.Client = new RpServicePortTypeClient();
                try
                {
                    returnModel.Order = Authenticate(loginModel.PersonalNumber);
                }
                catch (Exception ex)
                {
                    returnModel.TempText = ex.Message.ToString();
                    var templist = JsonConvert.SerializeObject(returnModel,
                        Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                        });

                    return Content(templist, "application/json");
                }

                var list = JsonConvert.SerializeObject(returnModel,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(list, "application/json");
            }
            else
            {
                returnModel.TempText = "Invalid_PN";
                var list = JsonConvert.SerializeObject(returnModel,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });
                return Content(list, "application/json");
            }

        }

        public Login AuthenticateWithBankId(string personalNumber)
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var order = Authenticate(personalNumber);
            return null;
        }


        public OrderResponseType Authenticate(string ssn)
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            using (var client = new RpServicePortTypeClient())
            {
                // RequirementType is optional
                // This will ensure only mobile BankID can be used
                // https://www.bankid.com/bankid-i-dina-tjanster/rp-info/guidelines
                RequirementType conditions = new RequirementType
                {
                    condition = new[]
                    {
                        new ConditionType()
                        {
                            key = "certificatePolicies",
                            value = new[] {"1.2.3.4.25"} // Mobile BankID
                        }}
                };

                // Set the parameters for the authentication
                AuthenticateRequestType authenticateRequestType = new AuthenticateRequestType()
                {
                    personalNumber = ssn,
                    requirementAlternatives = new[] { conditions }
                };
                // ...authenticate
                var test = client.Authenticate(authenticateRequestType);
                return test;
            }
        }

        [HttpPost]
        public ActionResult Collect(OrderResponseType order)
        {
            using (var client = new RpServicePortTypeClient())
            {
                RequirementType conditions = new RequirementType
                {
                    condition = new[]
                    {
                        new ConditionType()
                        {
                            key = "certificatePolicies",
                            value = new[] {"1.2.3.4.25"} // Mobile BankID
                        }}
                };
                AuthenticateRequestType authenticateRequestType = new AuthenticateRequestType()
                {
                    personalNumber = "198001018129",
                    requirementAlternatives = new[] { conditions }
                };

                var returnModel = new Login();
                returnModel.Client = client;
                try
                {
                    returnModel.Response = client.Collect(order.orderRef);
                }
                catch (Exception ex)
                {
                    returnModel.TempText = ex.Message.ToString();
                    var templist = JsonConvert.SerializeObject(returnModel,
                        Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                        });
                    return Content(templist, "application/json");
                }

                returnModel.ResponseCode = returnModel.Response.userInfo;
                var list = JsonConvert.SerializeObject(returnModel,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(list, "application/json");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact(Contact model)
        {
            if (ModelState.IsValid)
            {
                SendEmail(model);
                WriteEmailToFile(model);
                ViewBag.Message = "Email Sent Successfully";
                ModelState.Clear();
            }
            return View();
        }
        private void SendEmail(Contact model)
        {
            MailMessage message = new MailMessage("amolkumar.bambarde@transfergalaxy.com", model.EmailAddress);
            message.Subject = model.Message;
            message.Body = model.Message;
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential();
            client.Host = "smtp.gmail.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //Below line is commented,if username,password will be provided in EmailCredentials it will work.
            //client.Send(message);
        }
        private void WriteEmailToFile(Contact model)
        {
            var json = new JavaScriptSerializer().Serialize(model);
            System.IO.File.AppendAllText(@"C:/Users/Amol/Documents/EmailLog.txt", json);
        }
    }
}