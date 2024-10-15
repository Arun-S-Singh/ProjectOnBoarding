using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using ProjectOnBoarding.Models;
using MailKit.Net.Smtp;

namespace ProjectOnBoarding.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public EmailService(IConfiguration config, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;   
        }
        public void SendEmail(EmailDto emailRequest)
        {            
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:Username"]));
            email.To.Add(MailboxAddress.Parse(emailRequest.To));
            email.Subject = emailRequest.Subject;
            

            var builder = new BodyBuilder();            
            builder.HtmlBody = emailRequest.Body;

            string filePath = string.Empty;
            foreach (var attachment in emailRequest.Attachments) {
                filePath = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, "documents")
                        , attachment.UniqueFileName);
                builder.Attachments.Add(filePath);
            }
            
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_config["EmailSettings:Host"],Int32.Parse(_config["EmailSettings:Port"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
