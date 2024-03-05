using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BeatsSenderBot.Helpers
{
    public static class KeyboardHelper
    {
        /// <summary>
        /// Вывод списка стартовых кнопок
        /// </summary>
        public static async void StartMessageButtons(ITelegramBotClient botClient, long chatId)
        {
            var inlineKeyBoard = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Почты \ud83d\udcc4", callbackData: "email"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Авторизация \ud83d\udcad", callbackData: "authorization"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Рассылка \ud83d\udce7", callbackData: "mailing")
                    }
                });
            await botClient.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: inlineKeyBoard);
        }

        /// <summary>
        /// Вывод кнопки "Отправить". Для отправки битов.
        /// </summary>
        public static async void SendEmailButtons(ITelegramBotClient botClient, long chatId)
        {
            var inlineKeyBoard = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Отправить \u2705", callbackData: "sendAttachments"),
                    }
                });
            await botClient.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: inlineKeyBoard);
        }

        public static async void SendAttachmentButtons(ITelegramBotClient botClient, long chatId, string fileName)
        {
            var inlineKeyBoard = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Продолжить \u25b6", callbackData: "continue"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Прикрепить ещё \u2795", callbackData: "addMoreAttachments"),
                    }
                });
            await botClient.SendTextMessageAsync(chatId, $"Файл '{fileName}' успешно прикреплён", replyMarkup: inlineKeyBoard);
        }
    }
}
