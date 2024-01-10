using BeatsSenderBot.Helpers;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BeatsSenderBot
{
    public class UpdateHandler : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                var userName = message.Chat.Username;

                if (message.Text?.ToLower() == "/start")
                {
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

                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Пользователь {userName} запустил бота!", replyMarkup: inlineKeyBoard, cancellationToken: cancellationToken);

                    Console.WriteLine($"Сообщение отправлено. Username пользователя: {userName}");
                }

                if (message.Type == MessageType.Document)
                {
                    var path = @"D:\C# Projects\BeatsSenderBot\Files\"; //TODO изменить

                    var file = await botClient.GetFileAsync(message.Document.FileId, cancellationToken);
                    var fileName = "BeatSender.txt";

                    var filePath = Path.Combine(path, fileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await botClient.DownloadFileAsync(file.FilePath, fs);
                    }

                    EmailHelper.SendEmail(filePath);

                    await botClient.SendTextMessageAsync(message.Chat.Id, "Файл отправлен", cancellationToken: cancellationToken);
                }
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                var buttonCode = update.CallbackQuery.Data;

                if (buttonCode == "mailing")
                {
                    var chatId = update.CallbackQuery.Message.Chat.Id;

                    await botClient.SendTextMessageAsync(chatId, "Прикрепите файл для рассылки", cancellationToken: cancellationToken);
                }
            }
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Произошла ошибка: {JsonSerializer.Serialize(exception)}");
        }
    }
}
