using System;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace PUPAcadPortal.Utils
{
    public static class EmailService
    {
        private const string SenderEmail = "pupacademicportal@gmail.com";
        private const string SenderPassword = "ryob acox atcb mmnq";

        public static async Task SendWelcomeEmailAsync(string studentPersonalEmail, string firstName, string officialEmail, string tempPassword, string studentNumber, string username)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(SenderEmail, SenderPassword),
                    EnableSsl = true,
                };

                string htmlBody = $@"
                <!DOCTYPE html>
                <html>
                <body style='margin: 0; padding: 0; font-family: Arial, Helvetica, sans-serif; background-color: #f4f4f4;'>
                    <table role='presentation' width='100%' border='0' cellspacing='0' cellpadding='0' style='background-color: #f4f4f4; padding: 20px 0;'>
                        <tr>
                            <td align='center'>
                                <table role='presentation' width='600' border='0' cellspacing='0' cellpadding='0' style='background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.1); max-width: 600px; width: 100%;'>
                                    
                                    <tr>
                                        <td align='center' style='background-color: #880000; padding: 30px 20px; border-bottom: 5px solid #FDB813;'>
                                            <img src='cid:pupLogo' alt='PUP Logo' width='80' style='display: block; margin-bottom: 15px;' />
        
                                            <h1 style='color: #ffffff; margin: 0; font-size: 24px; letter-spacing: 1px; font-family: Cambria, Georgia, serif;'>POLYTECHNIC UNIVERSITY</h1>
                                            <h2 style='color: #ffffff; margin: 5px 0 0 0; font-size: 18px; font-weight: normal; font-family: Cambria, Georgia, serif;'>OF THE PHILIPPINES</h2>
        
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style='padding: 40px 30px; color: #333333; line-height: 1.6;'>
                                            <h2 style='color: #880000; margin-top: 0;'>Welcome, Iskolar ng Bayan!</h2>
                                            <p style='font-size: 16px;'>Dear <strong>{firstName}</strong>,</p>
                                            <p style='font-size: 16px;'>Congratulations! Your registration has been successfully processed. Below are your official credentials:</p>
                                            
                                            <table role='presentation' width='100%' border='0' cellspacing='0' cellpadding='0' style='background-color: #fff9e6; border-left: 5px solid #FDB813; border-radius: 4px; margin: 25px 0;'>
                                                <tr>
                                                    <td style='padding: 20px;'>
                                                        <p style='margin: 0 0 10px 0; font-size: 15px;'><strong>Student Number:</strong> <span style='color: #880000; font-size: 16px;'>{studentNumber}</span></p>
                                                        <p style='margin: 0 0 10px 0; font-size: 15px;'><strong>Institutional Email:</strong> <a href='mailto:{officialEmail}' style='color: #0056b3; text-decoration: none;'>{officialEmail}</a></p>
                                                        <p style='margin: 0 0 10px 0; font-size: 15px;'><strong>Username</strong> <span style='color: #880000; font-size: 16px;'>{username}</span></p>
                                                        <p style='margin: 0; font-size: 15px;'><strong>Temporary Password:</strong> <span style='font-family: monospace; background-color: #e2e3e5; padding: 3px 6px; border-radius: 3px; font-size: 15px;'>{tempPassword}</span></p>
                                                    </td>
                                                </tr>
                                            </table>

                                            <h3 style='color: #880000; font-size: 16px; border-bottom: 1px solid #eeeeee; padding-bottom: 5px;'>Action Required</h3>
                                            <ul style='padding-left: 20px; font-size: 15px;'>
                                                <li style='margin-bottom: 10px;'>Log in to the Academic Portal immediately.</li>
                                                <li style='margin-bottom: 10px;'>Navigate to your account settings and change your temporary password to a secure one.</li>
                                            </ul>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(SenderEmail, "PUP Admissions Office"),
                    Subject = "Action Required: Your Official PUP Credentials",
                };
                mailMessage.To.Add(studentPersonalEmail);

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

                // 3. LOAD THE IMAGE FROM YOUR COMPILED RESOURCES
                // We use a MemoryStream to pull the image directly out of the .exe file
                using (MemoryStream ms = new MemoryStream())
                {
                    Properties.Resources.PUPemaillogo.Save(ms, ImageFormat.Png);
                    ms.Position = 0; // Reset the stream to the beginning

                    LinkedResource logo = new LinkedResource(ms, "image/png")
                    {
                        ContentId = "pupLogo"
                    };

                    htmlView.LinkedResources.Add(logo);
                    mailMessage.AlternateViews.Add(htmlView);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email Failed to Send: {ex.Message}");
            }
        }

            public static async Task SendPasswordResetEmailAsync(string targetEmail, string firstName, string resetPin)
            {
            try
            {
                var mailMessage = new System.Net.Mail.MailMessage();
                // NOTE: Replace these with your actual SMTP credentials from your existing methods
                mailMessage.From = new System.Net.Mail.MailAddress("pupacademicportal@gmail.com");
                mailMessage.To.Add(targetEmail);
                mailMessage.Subject = "PUP Portal - Password Reset Verification Code";

                mailMessage.Body = $@"
            <h3>Hello {firstName},</h3>
            <p>We received a request to reset the password for your PUP Academic Portal account.</p>
            <p>Your 6-digit password reset code is: <b><span style='font-size: 24px;'>{resetPin}</span></b></p>
            <p>This code will expire in 15 minutes.</p>
            <p>If you did not request this, please ignore this email.</p>
            <br/>
            <p>PUP IT Support Team</p>";

                mailMessage.IsBodyHtml = true;

                using (var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
                {
                    // Replace with your actual credentials
                    smtpClient.Credentials = new System.Net.NetworkCredential("pupacademicportal@gmail.com", "ryob acox atcb mmnq");
                    smtpClient.EnableSsl = true;
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send reset email: {ex.Message}");
            }
        }
    }
}