using System;
using System.Net.Mail;

namespace ShadyNagy.Utilities.Email
{
    public class EmailManager
    {
        private readonly SmtpClient _smtpClient;
        public EmailManager(string userName, string password)
        {
            _smtpClient = new SmtpConnection(userName, password).GetSmtpServer();
        }
        public EmailManager(string userName, string password, string smtpClient, int port, bool isSsl)
        {
            _smtpClient = new SmtpConnection(userName, password, smtpClient, port, isSsl).GetSmtpServer();
        }

        public bool SendEmail(Email email)
        {
            try
            {
                _smtpClient.Send(email.GetMailMessage());
                return (true);
            }
            catch (Exception)
            {
                return (false);
            }
            
        }

        public void SendEmailAsync(Email email, object token=null)
        {
            _smtpClient.SendAsync(email.GetMailMessage(), token);
        }
    }
}
