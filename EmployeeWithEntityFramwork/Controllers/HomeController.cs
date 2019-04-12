using EmployeeWithEntityFramwork.BankIdService;
using EmployeeWithEntityFramwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
            if (loginModel != null)
            {
                if (loginModel.PersonalNumber != null)
                {
                    if (AuthenticateWithBankId(loginModel.PersonalNumber))
                    {
                        ViewBag.Message = "User is Logged In!!";
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Credentials!!";
                    }
                }
                
            }
            return View();
        }

        private bool AuthenticateWithBankId(string personalNumber)
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            try
            {
                var order = Authenticate(personalNumber);

                // collect the result
                var response = Collect(order);
                if (response.progressStatus == ProgressStatusType.COMPLETE)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static OrderResponseType Authenticate(string ssn)
        {
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
                return client.Authenticate(authenticateRequestType);
            }
        }

        public CollectResponseType Collect(OrderResponseType order)
        {
            using (var client = new RpServicePortTypeClient())
            {
                Console.WriteLine("{0}Start the BankID application and sign in", Environment.NewLine);

                CollectResponseType result = null;

                // Wait for the client to sign in 
                do
                {
                    // ...collect the response
                    result = client.Collect(order.orderRef);

                    Console.WriteLine(result.progressStatus);
                    System.Threading.Thread.Sleep(1000);

                } while (result.progressStatus != ProgressStatusType.COMPLETE);


                do
                {
                    return result;
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
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