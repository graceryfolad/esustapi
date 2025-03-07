
using DataAccess.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CardData.Helpers
{
    //public class MailService
    //{
    //    public void SendMail(EmailMessage mailRequest)
    //    {
    //        try
    //        {

    //            var buider = new ConfigurationBuilder();
    //            buider.AddJsonFile("appsettings.json", optional: false);
    //            var config = buider.Build();
    //            string host = config["MailSettings:host"];
    //            string mailuser = config["MailSettings:username"];
    //            string mailpassword = config["MailSettings:password"];
    //            int mailport = Convert.ToInt32(config["MailSettings:port"]);
    //            string mailfrom = config["MailSettings:from"];

    //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
    //                                                   SecurityProtocolType.Tls12;
    //            // Create a new disposable instance of SmtpClient
    //            using (SmtpClient smtpClient = new SmtpClient(host, mailport))
    //            {
    //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
    //                                                       SecurityProtocolType.Tls12;

    //                smtpClient.EnableSsl = true;
    //                smtpClient.Credentials = new NetworkCredential(mailuser, mailpassword);


    //                // Create a new instance of MailMessage
    //                using (MailMessage mailMessage = new MailMessage())
    //                {
    //                    // Set the email details
    //                    mailMessage.From = new MailAddress(mailfrom);
    //                    //mailMessage.From = new MailAddress(mailRequest.From);
    //                    mailMessage.To.Add(mailRequest.To);
    //                    mailMessage.Subject = mailRequest.Subject;
    //                    mailMessage.Body = mailRequest.Body;
    //                    mailMessage.IsBodyHtml = true;
    //                    if (mailRequest.CarbonCopy != null && mailRequest.CarbonCopy.Count > 0)
    //                    {
    //                        mailMessage.CC.Add(string.Join(',', mailRequest.CarbonCopy));
    //                    }

    //                    // Send the email
    //                    smtpClient.Send(mailMessage);
    //                }
    //            }

    //            // Return a success response
    //            var response = new MailResponseModel
    //            {
    //                IsSuccess = true,
    //                Message = "Mail sent successfully."
    //            };


    //        }
    //        catch (Exception ex)
    //        {
    //            // Return an error response if there's an exception
    //            var response = new MailResponseModel
    //            {
    //                IsSuccess = false,
    //                Message = $"Failed to send mail. Error: {ex.Message}"
    //            };

    //            ErrorLogger.Log("Eror Sending Message " + ex.Message);

    //        }
    //    }
    //}
    public static class EmailTemplate
    {
        public static string Welcome(string username)
        {
            return username;
        }

        public static string ConfirmEmail(string username, string code)
        {
            string message = @$"<!DOCTYPE html>
                <html>
                <head>
                    <meta charset=""utf-8"">
                    <title>Welcome to NetExam </title>
                </head>
                <body style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;"">
                  

                    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                        <tr>
                            <td style=""background-color: #ffffff; padding: 40px;"">
                                <h1 style=""color:#1b2c6c"">Welcome to Quiz Framework</h1>
                                <p style=""font-weight: bold;"">Dear {username},</p>
                                <p>An account has been created for you on Quiz Framework. </p>
                                <p>To get started, please take a moment to verify your email address.</p>

                                <h2 style=""color: rgb(164, 71, 37);"">Username: {username}</h2>
                                <h2 style=""color: rgb(164, 71, 37);"">Email Verification Code: {code}</h2>

                                <p>Kindly verify your email address with the with the code above</p>
                               
                                <i>Note: Please keep your User Name and Password well.</i>

                                <p>Best regards,<br><span style=""color: #1b2c6c; font-weight: bold;"">Management</span><br></p>
                            </td>
                        </tr>
                    </table>


                    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                        <tr>
                            <td style=""padding: 20px 0; text-align: center; background-color: rgb(243, 207, 193);"">
                                <img src=""https://res.cloudinary.com/dshlliomy/image/upload/v1695772206/qf_aszcdr.png"" alt=""[Your Quiz Framework]"" width=""200"">
                            </td>
                        </tr>
                    </table>
                </body>
                </html>";
            return message;
        }

        public static string PasswordReset(string username, string code)
        {
            string message = @$"<!DOCTYPE html>
                <html>
                <head>
                    <meta charset=""utf-8"">
                    <title>Welcome to Quiz Framework</title>
                </head>
                <body style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;"">

                    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                        <tr>
                            <td style=""padding: 20px 0; text-align: center;"">
                                <img src=""https://res.cloudinary.com/dshlliomy/image/upload/v1695772206/qf_aszcdr.png"" alt=""Quiz Framework"" width=""200"">
                            </td>
                        </tr>
                    </table>

                    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                        <tr>
                            <td style=""background-color: #ffffff; padding: 40px;"">
                                <h1 style=""color:#1b2c6c"">Welcome to Quiz Framework</h1>
                                <p style=""font-weight: bold;"">Dear {username},</p>
                                <p>We have received a request to reset your password for your [Your Quiz Framework Application] account. 
                                    To proceed with the password reset, please follow the instructions below:</p>

                                <h2 style=""color: rgb(164, 71, 37);"">Email Verification Code: {code}</h2>

                                <p>To verify your email address and activate your account, follow these simple steps:</p>
                                <ol>
                                    <li>Click here to open password reset page.</li>
                                    <li>Enter your reset code in the box provided and click reset</li>
                                    <li>If you did not request a password reset, or if you believe this request is in error, please disregard this email. 
                                    Your account security is important to us, and no changes will be made without your consent.</li>
                                    <li>If you continue to experience issues or need further assistance, please don't hesitate to contact our support 
                                    team at [Support Email Address].</li>
                                </ol>

                                <i>Thank you for using [Your Quiz Framework Application]. We appreciate your trust in our platform.</i>
                                <p>Best regards,<br><span style=""color: #1b2c6c; font-weight: bold;"">Quiz Framework team</span><br></p>
                            </td>
                        </tr>
                    </table>


                    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                        <tr>
                            <td style=""padding: 20px 0; text-align: center; background-color: rgb(243, 207, 193);"">
                                <img src=""https://res.cloudinary.com/dshlliomy/image/upload/v1695772206/qf_aszcdr.png"" alt=""[Your Quiz Framework]"" width=""200"">
                            </td>
                        </tr>
                    </table>
                </body>
                </html>";
            return message;
        }

        public static string PasswordResetSuccessful(string username)
        {
            string message = @$"<!DOCTYPE html>
                <html>
                <head>
                    <meta charset=""utf-8"">
                    <title>Welcome to Quiz Framework</title>
                </head>
                <body style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;"">

                    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                        <tr>
                            <td style=""padding: 20px 0; text-align: center;"">
                                <img src=""https://res.cloudinary.com/dshlliomy/image/upload/v1695772206/qf_aszcdr.png"" alt=""Quiz Framework"" width=""200"">
                            </td>
                        </tr>
                    </table>

                    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                        <tr>
                            <td style=""background-color: #ffffff; padding: 40px;"">
                                <h1 style=""color:#1b2c6c"">Welcome to Quiz Framework</h1>
                                <p style=""font-weight: bold;"">Dear {username},</p>
                                <p>
                                    We're writing to inform you that your password has been successfully reset for your [Your Quiz Framework Application] account. Your account security is important to us, and we take the protection of your information seriously.
                                    If you initiated the password reset, you can now log in to your account using your new password. Please keep your login credentials secure and do not share them with anyone.
                                    If you did not request this password reset or believe it was done without your authorization, please contact our support team immediately at [Support Email Address]. We will investigate and take appropriate action to secure your account.
                                    For any further assistance or inquiries related to [Your Quiz Framework Application], please feel free to reach out to us at [Support Email Address].
                                </p>


                                <p>Best regards,<br><span style=""color: #1b2c6c; font-weight: bold;"">Quiz Framework team</span><br></p>
                            </td>
                        </tr>
                    </table>


                    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                        <tr>
                            <td style=""padding: 20px 0; text-align: center; background-color: rgb(243, 207, 193);"">
                                <img src=""https://res.cloudinary.com/dshlliomy/image/upload/v1695772206/qf_aszcdr.png"" alt=""[Your Quiz Framework]"" width=""200"">
                            </td>
                        </tr>
                    </table>
                </body>
                </html>";
            return message;
        }
    }

    public class EmailMessage
    {
       
        public string To { get; set; } = string.Empty;
       
        public string Subject { get; set; } = string.Empty;
       
        public string Body { get; set; } = string.Empty;
        
        public List<string> CarbonCopy { get; set; } = new();
    }
}
