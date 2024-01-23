using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BeatsSenderBot.Helpers
{
    public static class InlineKeyboardHelper
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
            await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите действие", replyMarkup: inlineKeyBoard);
        }
    }
}
