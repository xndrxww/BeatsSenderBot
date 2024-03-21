using Newtonsoft.Json;
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
                        InlineKeyboardButton.WithCallbackData(text: $"Почты {GetIcon(Resource.Emails)}", callbackData: "email"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: $"Авторизация {GetIcon(Resource.Authorization)}", callbackData: "authorization"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: $"Рассылка битов {GetIcon(Resource.SendBeats)}", callbackData: "mailing")
                    }
                });
            await botClient.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: inlineKeyBoard);
        }

        /// <summary>
        /// Вывод кнопки "Отправить". Для раздела "Рассылка битов".
        /// </summary>
        public static async void SendEmailButtons(ITelegramBotClient botClient, long chatId)
        {
            var inlineKeyBoard = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: $"Отправить {GetIcon(Resource.Send)}", callbackData: "sendAttachments"),
                    }
                });
            await botClient.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: inlineKeyBoard);
        }

        /// <summary>
        /// Вывод кнопок "Продолжить" и "Прикрепить ещё". Для раздела "Рассылка битов".
        /// </summary>
        public static async void SendAttachmentButtons(ITelegramBotClient botClient, long chatId, string fileName)
        {
            var inlineKeyBoard = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: $"Продолжить {GetIcon(Resource.Continue)}", callbackData: "continue"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: $"Прикрепить ещё {GetIcon(Resource.AddMore)}", callbackData: "addMoreAttachments"),
                    }
                });
            await botClient.SendTextMessageAsync(chatId, $"Файл '{fileName}' успешно прикреплён", replyMarkup: inlineKeyBoard);
        }

        private static string GetIcon(string unicodeStringFromResources)
        {
            return JsonConvert.DeserializeObject<string>($"\"{unicodeStringFromResources}\"");
        }
    }
}
