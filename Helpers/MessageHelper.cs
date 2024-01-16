using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace BeatsSenderBot.Helpers
{
    public static class MessageHelper
    {
        public static async void SendStartMessage(ITelegramBotClient botClient, Message message)
        {
            var userName = message.Chat.Username;

            var inlineKeyBoard = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Почты", callbackData: "email"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Авторизация", callbackData: "authorization"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Рассылка", callbackData: "mailing")
                    }
                });

            await botClient.SendTextMessageAsync(message.Chat.Id, $"Пользователь {userName} запустил бота!", replyMarkup: inlineKeyBoard);
        }

        public static async void SendDocument(ITelegramBotClient botClient, Message message)
        {
            var file = await botClient.GetFileAsync(message.Document.FileId);

            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");
            var fileName = $"BeatSenderFile_{Guid.NewGuid()}.txt";
            var filePath = Path.Combine(folderPath, fileName);

            try
            {
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await botClient.DownloadFileAsync(file.FilePath, fs);
                }

                //EmailHelper.SendEmail(filePath);

                await botClient.SendTextMessageAsync(message.Chat.Id, "Файл отправлен");
            }
            catch (Exception e)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "При загрузке/отправке файла произошла ошибка");
                Console.WriteLine($"Ошибка при загрузке файла {e}");
            }
            finally
            {
                string[] filePaths = Directory.GetFiles(folderPath);

                foreach (var fp in filePaths)
                {
                    if (File.Exists(fp))
                        File.Delete(fp);
                }
            }
        }
    }
}
