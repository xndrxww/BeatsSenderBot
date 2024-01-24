using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BeatsSenderBot.Helpers
{
    public static class KeyboardHelper
    {
        public static async void StartMessageButtons(ITelegramBotClient botClient, long chatId)
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
            await botClient.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: inlineKeyBoard);
        }

        public static async void SendEmailButtons(ITelegramBotClient botClient, long chatId)
        {
            var inlineKeyBoard = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Отправить", callbackData: "sendAttachments"),
                    }
                });
            await botClient.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: inlineKeyBoard);
        }
    }
}
