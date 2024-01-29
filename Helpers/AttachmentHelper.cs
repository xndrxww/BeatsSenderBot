using Telegram.Bot;
using Telegram.Bot.Types;

namespace BeatsSenderBot.Helpers
{
    public static class AttachmentHelper
    {
        private static readonly string FilesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");

        public static async Task SaveAttachmentFile(ITelegramBotClient botClient, Message message, string fileName)
        {
            var chatId = message.Chat.Id;

            try
            {
                var file = await botClient.GetFileAsync(message.Audio.FileId);
                var folderPath = Path.Combine(FilesFolderPath, chatId.ToString());

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, $"{fileName}.mp3");

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await botClient.DownloadFileAsync(file.FilePath, fs);
                }
            }
            catch (Exception e)
            {
                await botClient.SendTextMessageAsync(chatId, "При загрузке файла произошла ошибка");
                Console.WriteLine($"Ошибка при загрузке файла: '{e}'");
            }
        }
    }
}
