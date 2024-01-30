using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace DevBlog.Services.MailService
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSetting _mailSetting;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<MailSetting> options, ILogger<EmailSender> logger)
        {
            _mailSetting = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string emailTo, string subject, string htmlMessage)
        {
            var mimeMessage = new MimeMessage();
            var mailboxAddress = new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Mail);
            mimeMessage.Sender = mailboxAddress;
            mimeMessage.From.Add(mailboxAddress);
            mimeMessage.To.Add(MailboxAddress.Parse(emailTo));
            mimeMessage.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            mimeMessage.Body = builder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                try
                {
                    smtp.Connect(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSetting.Mail, _mailSetting.Password);
                    await smtp.SendAsync(mimeMessage);
                }
                catch (Exception ex)
                {
                    string path = "Services/MailService/ErrorMailSave";
                    Directory.CreateDirectory(path);
                    var emailSaveFile = string.Format(@"{0}.eml", Guid.NewGuid());
                    await mimeMessage.WriteToAsync(emailSaveFile);

                    _logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailSaveFile);
                    _logger.LogInformation(ex.Message);
                }
                _logger.LogInformation("send mail to " + emailTo);
            }
        }
    }
}
