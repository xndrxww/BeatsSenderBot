using Telegram.Bot;
using Telegram.Bot.Types;

namespace BeatsSenderBot.Helpers
{
    public static class AttachmentHelper
    {
        private static readonly string FolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");

        public static async Task SaveAttachmentFile(ITelegramBotClient botClient, Message message)
        {
            try
            {
                var file = await botClient.GetFileAsync(message.Document.FileId);

                var fileName = $"BeatSenderFile_{Guid.NewGuid()}.txt";
                var filePath = Path.Combine(FolderPath, fileName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await botClient.DownloadFileAsync(file.FilePath, fs);
                }
            }
            catch (Exception e)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "При загрузке файла произошла ошибка");
                Console.WriteLine($"Ошибка при загрузке файла: '{e}'");
            }
        }
    }
}
