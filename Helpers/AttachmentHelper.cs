using BeatsSenderBot.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BeatsSenderBot.Helpers
{
    public static class AttachmentHelper
    {
        public static async Task SaveAttachmentFile(ITelegramBotClient botClient, Message message, MessageType messageType, string fileName)
        {
            var chatId = message.Chat.Id;

            try
            {
                var file = messageType == MessageType.Audio
                    ? await botClient.GetFileAsync(message.Audio.FileId)
                    : await botClient.GetFileAsync(message.Document.FileId);

                var folderName = messageType == MessageType.Audio
                    ? FolderConstants.BeatsFolderName
                    : FolderConstants.EmailsFolderName;

                var folderPath = Path.Combine(folderName, chatId.ToString());

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);

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
