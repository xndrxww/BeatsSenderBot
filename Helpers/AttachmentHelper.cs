using Telegram.Bot;
using Telegram.Bot.Types;

namespace BeatsSenderBot.Helpers
{
    public static class AttachmentHelper
    {
        private static readonly string FolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");

        public static async Task SaveAttachmentFile(ITelegramBotClient botClient, Message message)
        {
            var chatId = message.Chat.Id;

            try
            {
                var file = await botClient.GetFileAsync(message.Audio.FileId);

                var fileName = $"BeatSenderFile_{Guid.NewGuid()}.mp3";
                var filePath = Path.Combine(FolderPath, fileName);

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
