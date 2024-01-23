using BeatsSenderBot.Enums;
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
        private static Dictionary<long, EmailState> emailState = new Dictionary<long, EmailState>();

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;

                if (message.Text?.ToLower() == "/start")
                {
                    InlineKeyboardHelper.SendStartMessage(botClient, message);
                }
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                var buttonCode = update.CallbackQuery.Data;

                if (buttonCode == "mailing")
                {
                    var chatId = update.CallbackQuery.Message.Chat.Id;

                    var emailState = GetEmailState(chatId);

                    switch (emailState)
                    {
                        case EmailState.AwaitAttacments:
                            await botClient.SendTextMessageAsync(chatId, "Прикрепите файлы для рассылки");
                            SaveEmailState(chatId, EmailState.AwaitSendAttachmennts);
                            break;
                        case EmailState.AwaitSendAttachmennts:
                            {
                                var inlineKeyBoard = new InlineKeyboardMarkup(
                                    new[]
                                    {
                                    new[]
                                    {
                                        InlineKeyboardButton.WithCallbackData(text: "Отправить прикрепленные файлы", callbackData: "sendAttachmennts"),
                                    }
                                    });

                                await botClient.SendTextMessageAsync(chatId, "тест", replyMarkup: inlineKeyBoard);
                                SaveEmailState(chatId, EmailState.Completed);
                                break;
                            }
                        case EmailState.Completed:
                            await botClient.SendTextMessageAsync(chatId, "Прикрепленные файлы отправлены!");
                            ResetUserState(chatId);
                            break;


                    }
                }
                if (buttonCode == "sendAttachmennts")
                {
                    EmailHelper.SendAttachmennts();
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

            return EmailState.AwaitAttacments;
        }

        private static void SaveEmailState(long chatId, EmailState state)
        {
            emailState[chatId] = state;
        }

        private static void ResetUserState(long chatId)
        {
            emailState.Remove(chatId);
        }
    }
}
