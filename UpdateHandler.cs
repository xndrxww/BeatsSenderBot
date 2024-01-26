using BeatsSenderBot.Enums;
using BeatsSenderBot.Helpers;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BeatsSenderBot
{
    public class UpdateHandler : IUpdateHandler
    {
        private static Dictionary<long, EmailState> emailState = new Dictionary<long, EmailState>();

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                var chatId = message.Chat.Id;

                if (message.Text?.ToLower() == "/start")
                {
                    KeyboardHelper.StartMessageButtons(botClient, chatId);
                }

                //Сохранение прикрепленных файлов
                if (message.Type == MessageType.Audio)
                {
                    var emailState = GetEmailState(chatId);

                    if (emailState == EmailState.AwaitAttachments)
                    {
                        await AttachmentHelper.SaveAttachmentFile(botClient, message);
                        SaveEmailState(chatId, EmailState.AwaitSendAttachments);
                        KeyboardHelper.SendEmailButtons(botClient, chatId);
                    }
                }
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                var buttonCode = update.CallbackQuery.Data;
                var chatId = update.CallbackQuery.Message.Chat.Id;
                var emailState = GetEmailState(chatId);

                if (buttonCode == "mailing") //Нажатие на кнопку "Рассылка"
                {
                    await botClient.SendTextMessageAsync(chatId, "Прикрепите файлы для рассылки");
                    SaveEmailState(chatId, EmailState.AwaitAttachments);
                }

                if (buttonCode == "sendAttachments" && emailState == EmailState.AwaitSendAttachments) //Нажатие на кнопку "Отправить"
                {
                    EmailHelper.SendAttachments(chatId);
                    ResetEmailState(chatId);
                    KeyboardHelper.StartMessageButtons(botClient, chatId);
                }
            }
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Произошла ошибка: {JsonSerializer.Serialize(exception)}");
        }

        private static EmailState GetEmailState(long chatId)
        {
            if (emailState.ContainsKey(chatId))
                return emailState[chatId];

            return EmailState.AwaitAttachments;
        }

        private static void SaveEmailState(long chatId, EmailState state)
        {
            emailState[chatId] = state;
        }

        private static void ResetEmailState(long chatId)
        {
            emailState.Remove(chatId);
        }
    }
}
