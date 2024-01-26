using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BeatsSenderBot.Helpers
{
    public static class EmailHelper
    {
        private static readonly string FilesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");

        public static void SendAttachments(long chatId)
        {
            var filePaths = Directory.GetFiles(Path.Combine(FilesFolderPath, chatId.ToString()));

            try
            {
                var message = CreateMessage(filePaths);

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
            finally
            {
                Directory.Delete(Path.Combine(FilesFolderPath, chatId.ToString()), true);
            }
        }

        private static MimeMessage CreateMessage(string[] filePaths)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("BeatsSenderBot", "blacksilverforg00gle@gmail.com"));
            message.To.Add(new MailboxAddress("blacksilver@ro.ru", "blacksilver@ro.ru"));
            message.Subject = "Cooбщение от BeatsSenderBot";
            message.Body = CreateMessageBody(filePaths);

            return message;
        }

        private static MimeEntity CreateMessageBody(string[] filePaths)
        {
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "Письмо от BeatsSenderBot с вложением";

            foreach (var filePath in filePaths)
            {
                bodyBuilder.Attachments.Add(filePath);
            }

            return bodyBuilder.ToMessageBody();
        }
    }
}
