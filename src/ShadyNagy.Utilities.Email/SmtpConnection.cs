using System.Net.Mail;

namespace ShadyNagy.Utilities.Email
{
    public class SmtpConnection
    {

        public string SmtpClient { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsSsl { get; set; }

        private readonly SmtpClient _smtpClient;

        public SmtpConnection(string userName, string password)
        {
            UserName = userName;
            Password = password;
            SmtpClient = "smtp.gmail.com";
            IsSsl = true;
            Port = 587;

            _smtpClient = new SmtpClient(SmtpClient)
            {
                UseDefaultCredentials = false,
                Port = Port,
                Host = SmtpClient,
                Credentials = new System.Net.NetworkCredential(UserName, Password),
                EnableSsl = IsSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

        }

        public SmtpConnection(string userName, string password, string smtpClient)
        {
            UserName = userName;
            Password = password;
            SmtpClient = smtpClient;
            IsSsl = true;
            Port = 587;

            _smtpClient = new SmtpClient(SmtpClient)
            {
                UseDefaultCredentials = false,
                Port = Port,
                Host = SmtpClient,
                Credentials = new System.Net.NetworkCredential(UserName, Password),
                EnableSsl = IsSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        public SmtpConnection(string userName, string password, string smtpClient, int port)
        {
            UserName = userName;
            Password = password;
            SmtpClient = smtpClient;
            IsSsl = true;
            Port = port;

            _smtpClient = new SmtpClient(SmtpClient)
            {
                UseDefaultCredentials = false,
                Port = Port,
                Host = SmtpClient,
                Credentials = new System.Net.NetworkCredential(UserName, Password),
                EnableSsl = IsSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        public SmtpConnection(string userName, string password, string smtpClient, int port, bool isSsl)
        {
            UserName = userName;
            Password = password;
            SmtpClient = smtpClient;
            IsSsl = isSsl;
            Port = port;

            _smtpClient = new SmtpClient(SmtpClient)
            {
                UseDefaultCredentials = false,
                Port = Port,
                Host = SmtpClient,
                Credentials = new System.Net.NetworkCredential(UserName, Password),
                EnableSsl = IsSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        public SmtpConnection(string userName, string password, string smtpClient, bool isSsl)
        {
            UserName = userName;
            Password = password;
            SmtpClient = smtpClient;
            IsSsl = isSsl;
            Port = 587;

            _smtpClient = new SmtpClient(SmtpClient)
            {
                UseDefaultCredentials = false,
                Port = Port,
                Host = SmtpClient,
                Credentials = new System.Net.NetworkCredential(UserName, Password),
                EnableSsl = IsSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        public SmtpClient GetSmtpServer()
        {
            return _smtpClient;
        }
    }
}
