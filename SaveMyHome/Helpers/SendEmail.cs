using SaveMyHome.Abstract;
using SaveMyHome.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SaveMyHome.Helpers
{
    public class EmailSettings
    {
        public string MailFromAddress = "andrylebortovo@gmail.com";
        public bool UseSsl = true;
        public string Username = "andrylebortovo@gmail.com";
        public string Password = "10[cyl-14";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 25;
    }

    public class EmailNotifyProcessor : INotifyProcessor
    {
        private EmailSettings emailSettings;
        public EmailNotifyProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessNotify(Message message, IEnumerable<Apartment> apartments)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                //Текст email-сообщения
                string body = new StringBuilder()
                    .AppendLine($"{message.User.FullName}, кв №{message.User.ApartmentNumber}:")
                    .AppendLine(message.Text)
                    .AppendLine(message.Time.ToString())
                    .ToString();

                //Список email-адресов для рассылки
                IEnumerable<string> emails = apartments.SelectMany(a => a.Users.Select(u => u.Email));

                foreach (string mailToAddress in emails)
                    smtpClient.Send(new MailMessage(emailSettings.MailFromAddress,
                                    mailToAddress, "Внимание!!!", body));
            }
        }
    }
}