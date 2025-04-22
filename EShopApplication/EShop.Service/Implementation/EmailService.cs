using EShop.Domain;
using EShop.Service.Interface;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public Boolean SendEmailAsync(EmailMessage allMails)
        {
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress("EShop Application", "zorica.karapancheva@finki.ukim.mk"),
                Subject = allMails.Subject
            };

            emailMessage.From.Add(new MailboxAddress("EShop Application", "zorica.karapancheva@finki.ukim.mk"));

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain) {
                Text = allMails.Content };

            emailMessage.To.Add(new MailboxAddress(allMails.MailTo, allMails.MailTo));

            try
            {
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    var socketOptions = SecureSocketOptions.Auto;

                    smtp.Connect(_mailSettings.SmtpServer, 587, socketOptions);

                    if (!string.IsNullOrEmpty(_mailSettings.SmtpUserName))
                    {
                        smtp.Authenticate(_mailSettings.SmtpUserName, _mailSettings.SmtpPassword);
                    }
                    smtp.Send(emailMessage);


                    smtp.Disconnect(true);

                    return true;
                }
            }
            catch (SmtpException ex)
            {
                throw ex;
            }
        }
    }
}
