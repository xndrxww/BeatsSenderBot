using Telegram.Bot;
using Telegram.Bot.Types;

namespace BeatsSenderBot.Helpers
{
    public static class CallbackQueryHelper
    {
        public static async void GetMailing(ITelegramBotClient botClient, Update update)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            await botClient.SendTextMessageAsync(chatId, "Прикрепите файл для рассылки");
        }
    }
}
