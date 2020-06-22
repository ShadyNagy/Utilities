using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace ShadyNagy.Utilities.Email
{
    public class Email
    {
        private string FromName { get; set; }
        private string FromAddress { get; set; }
        private List<string> ToAddresses { get; set; }
        private List<string> CcAddresses { get; set; }
        private string Subject { get; set; }
        private string Body { get; set; }
        private List<Attachment> Attachments { get; set; }
        private MailMessage Mail { get; set; }

        public Email()
        {
            Attachments = new List<Attachment>();
            ToAddresses = new List<string>();
            CcAddresses = new List<string>();
            Mail = new MailMessage {IsBodyHtml = true};
        }

        public Email AddAttachment(Attachment file)
        {
            if(this.Attachments == null)
            {
                Attachments = new List<Attachment>();
            }
            Attachments.Add(file);

            var item = new System.Net.Mail.Attachment(new MemoryStream(file.Data), file.Name);
            Mail.Attachments.Add(item);

            return this;
        }

        public Email AddAttachment(string path, string fileName)
        {
            var fullPath = Path.Combine(path, fileName);

            if (this.Attachments == null)
            {
                Attachments = new List<Attachment>();
            }
            Attachments.Add(new Attachment(fullPath, null));

            System.Net.Mail.Attachment item = new System.Net.Mail.Attachment(fullPath);
            Mail.Attachments.Add(item);

            return this;
        }

        

        public Email AddCcAddress(string address)
        {
            if (CcAddresses == null)
            {
                CcAddresses = new List<string>();
            }

            CcAddresses.Add(address);

            Mail.CC.Add(address);

            return this;
        }

        public Email AddToAddress(string address)
        {
            if (ToAddresses == null)
            {
                ToAddresses = new List<string>();
            }

            ToAddresses.Add(address);

            Mail.To.Add(address);

            return this;
        }

        public Email SetFromAddress(string address)
        {
            FromAddress = address;

           Mail.From = new MailAddress(address, Mail.From?.DisplayName);          

            return this;
        }

        public Email SetFromName(string name)
        {
            FromName = name;

            Mail.From = new MailAddress(Mail.From?.Address, name);

            return this;
        }

        public Email SetSubject(string subject)
        {
            Subject = subject;

            Mail.Subject = subject;

            return this;
        }

        public Email SetBody(string body)
        {
            Body = body;

            Mail.Body = body;

            return this;
        }

        public MailMessage GetMailMessage()
        {
            return Mail;
        }
    }
}
