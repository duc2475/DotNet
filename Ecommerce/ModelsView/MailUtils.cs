using System.Net;
using System.Net.Mail;

namespace Ecommerce.ModelsView
{
    public class MailUtils
    {
        public static async Task<string> SendMail(string _from, string _to, string _subject, string _body)
        {
            MailMessage message = new MailMessage(_from, _to, _subject, _body);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            message.ReplyToList.Add(new MailAddress(_from));

            using var SmtpClient = new SmtpClient("localhost");

            try
            {
                await SmtpClient.SendMailAsync(message);
                return "Gửi Mail Thành Công";

            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Gửi Mail Không Thành Công";
            }
        }

        public static async Task<string> SendGmailMail(string _from, string _to, string _subject, string _body, string _gmail, string _password)
        {
            MailMessage message = new MailMessage(_from, _to, _subject, _body);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            message.ReplyToList.Add(new MailAddress(_from));

            using var SmtpClient = new SmtpClient("smtp.gmail.com");

            SmtpClient.Port = 587;
            SmtpClient.EnableSsl = true;
            SmtpClient.Credentials = new NetworkCredential(_gmail, _password);

            try
            {
                await SmtpClient.SendMailAsync(message);
                return "Gửi Mail Thành Công";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Gửi Mail Không Thành Công";
            }
        }
    }
}
