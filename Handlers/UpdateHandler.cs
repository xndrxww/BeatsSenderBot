using BeatsSenderBot.Enums;
using BeatsSenderBot.Helpers;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BeatsSenderBot.Handlers
{
    public class UpdateHandler : IUpdateHandler
    {
        private static Dictionary<long, SendBeatsState> emailStateDic = new Dictionary<long, SendBeatsState>();

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //Обработка сообщений
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                var chatId = message.Chat.Id;

                //Обработка стартового сообщения
                if (message.Text?.ToLower() == "/start")
                {
                    KeyboardHelper.StartMessageButtons(botClient, chatId);
                }

                //Обработка входящего бита
                if (message.Type == MessageType.Audio)
                {
                    await AudioMessageHandler.HandleIncomingAudioMessage(update, botClient, emailStateDic);
                }

                //Обработка входящего файла со списком почт
                if (message.Type == MessageType.Document)
                {
                    await DocumentMessageHandler.HandleIncomingDocumentMessage(update, botClient);
                }
            }

            //Обработка кнопок
            if (update.Type == UpdateType.CallbackQuery)
            {
                await CallbackQueryHandler.HandleCallbackQuery(update, botClient, emailStateDic);
            }
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Произошла ошибка: {JsonSerializer.Serialize(exception)}");
        }
    }
}
