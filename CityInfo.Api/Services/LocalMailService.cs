using System.Net;
using System.Net.Mail;

namespace CityInfo.Api.Services
{
    public class LocalMailService
    {
        string _mailTo = "mohammadmot@gmail.com";
        string _mailFrom = "m.motieian@aranuma.com";

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}," +
                $" with {nameof(LocalMailService)}");

            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message {message}");

            Email(subject, _mailFrom, _mailTo, message);
        }

        public static void Email(string subject,
            string mailFrom, 
            string mailTo,
            string htmlString)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();

            message.From = new MailAddress(mailFrom); // FromMailAddress
            message.To.Add(new MailAddress(mailTo)); // "ToMailAddress"
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = htmlString;

            smtpClient.Port = 587;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("username", "pass");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpClient.Send(message);
        }
    }
}
