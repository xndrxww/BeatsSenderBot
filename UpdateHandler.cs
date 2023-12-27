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

                if (message.Text.ToLower() == "/start")
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
                                InlineKeyboardButton.WithCallbackData(text: "Разослать", callbackData: "mailing")
                            }
                        });

                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Пользователь {userName} запустил бота!", replyMarkup: inlineKeyBoard);

                    Console.WriteLine($"Сообщение отправлено. Username пользователя: {userName}");
                }
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                var buttonCode = update.CallbackQuery.Data;

                if (buttonCode == "mailing")
                {
                    EmailHelper.SendEmail();
                }
            }
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Произошла ошибка: {JsonSerializer.Serialize(exception)}");
        }
    }
}
