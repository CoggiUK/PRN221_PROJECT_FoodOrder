using System.Net.Mail;
using System.Net;

namespace PRN221_PROJECT_FoodOrder.Utils
{
    public class EmailService
    {
        private static readonly string SmtpHost = "smtp.gmail.com";
        private static readonly int SmtpPort = 587;
        private static readonly string SmtpUsername = "tunglamnguyen22112002@gmail.com";
        private static readonly string SmtpPassword = "Lamhandsome";

        public static void SendResetPasswordEmail(string toEmail, string resetLink)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(SmtpUsername);
            message.To.Add(toEmail);
            message.Subject = "Reset Password - QuickFood";
            message.Body = $"Click the link below to reset your password:\n\n{resetLink}";

            SmtpClient smtpClient = new SmtpClient(SmtpHost, SmtpPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
            smtpClient.EnableSsl = true;

            smtpClient.Send(message);
        }
    }
}
