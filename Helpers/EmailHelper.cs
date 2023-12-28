using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BeatsSenderBot.Helpers
{
    public static class EmailHelper
    {
        public static void SendEmail()
        {
            try
            {
                var message = CreateMessage();

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate("blacksilverforg00gle@gmail.com", "nhvx eogs zlhf vcfd");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"При отправке писем произошла ошибка: '{e.Message}'");
            }
        }

        private static MimeMessage CreateMessage()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("From", "blacksilverforg00gle@gmail.com"));
            message.To.Add(new MailboxAddress("blacksilver@ro.ru", "blacksilver@ro.ru"));
            message.Subject = "Cooбщение от BeatsSenderBot";
            message.Body = CreateMessageBody();

            return message;
        }

        private static MimeEntity CreateMessageBody()
        {
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "Тестовое письмо от BeatsSenderBot с вложением";
            bodyBuilder.Attachments.Add(@"C:\\Users\\Andrew\\Desktop\\BeatSender.txt");

            return bodyBuilder.ToMessageBody();
        }
    }
}
