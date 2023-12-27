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
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("From", "blacksilverforg00gle@gmail.com"));
                message.To.Add(new MailboxAddress("blacksilver@ro.ru", "blacksilver@ro.ru"));
                message.Subject = "Cooбщение от BeatsSenderBot";

                //TODO https://stackoverflow.com/questions/37853903/can-i-send-files-via-email-using-mailkit
                // Добавление вложения в письмо

                message.Body = new TextPart("plain")
                {
                    Text = @"Тестовое письмо от BeatsSenderBot без вложений"
                };

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
    }
}
