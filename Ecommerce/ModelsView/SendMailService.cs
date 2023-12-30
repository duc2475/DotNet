using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Ecommerce.ModelsView
{
    public class SendMailService
    {
        MailSetting _mailSettings {  get; set; }

        MailContent _mailContent { get; set; }
        public SendMailService(IOptions<MailSetting> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task<string> SendMail(MailContent mailContent)
        {
            _mailContent = mailContent;
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.Subject = _mailContent.Subject;
            email.To.Add(new MailboxAddress(_mailContent.To, _mailContent.To));

            var builder = new BodyBuilder();
            builder.HtmlBody = _mailContent.Body;

            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);

                await smtp.SendAsync(email);

            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Gửi Lỗi";
            }

           

            smtp.Disconnect(true);
            return "Gửi Thành Công";
        }
    }
}
