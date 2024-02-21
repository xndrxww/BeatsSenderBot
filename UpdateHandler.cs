using BeatsSenderBot.Enums;
using BeatsSenderBot.Helpers;
using BeatsSenderDb.Extensions;
using BeatsSenderDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

                if (message.Type == MessageType.Audio) //Обработка входящего бита
                {
                    await HandleIncomingAudioMessage(update, botClient);
                }

                if (message.Type == MessageType.Document) //Обработка входящего документа со списком почт
                {
                    await HadleIncomingDocumentMessage(update, botClient);
                }
            }
            if (update.Type == UpdateType.CallbackQuery) //Обработка кнопок
            {
                var buttonCode = update.CallbackQuery.Data;
                var chatId = update.CallbackQuery.Message.Chat.Id;
                var emailState = GetEmailState(chatId);

                #region Обработка кнопок раздела "Рассылка"
                if (buttonCode == "mailing") //Нажатие на кнопку "Рассылка"
                {
                    await botClient.SendTextMessageAsync(chatId, "Прикрепите файлы для рассылки");
                    SaveEmailState(chatId, EmailState.AwaitAttachments);
                }

                if (buttonCode == "sendAttachments" && emailState == EmailState.AwaitSendAttachments) //Нажатие на кнопку "Отправить"
                {
                    EmailHelper.SendAttachments(chatId);
                    ResetEmailState(chatId);
                    await botClient.SendTextMessageAsync(chatId, "Письмо отправлено!");
                    KeyboardHelper.StartMessageButtons(botClient, chatId);
                }

                if (buttonCode == "continue") //Нажатие на кнопку "Продолжить"
                {
                    SaveEmailState(chatId, EmailState.AwaitSendAttachments);
                    KeyboardHelper.SendEmailButtons(botClient, chatId);
                }

                if (buttonCode == "addMoreAttachments") //Нажатие на кнопку "Прикрепить ещё"
                {
                    await botClient.SendTextMessageAsync(chatId, "Прикрепите файлы для рассылки");
                }
                #endregion

                #region Обработка кнопок раздела "Почты"
                if (buttonCode == "email") //Нажатие на кнопку "Почты"
                {
                    await botClient.SendTextMessageAsync(chatId, "Прикрепите текстовый файл со списком почт для рассылки");
                }
                #endregion

            }
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Произошла ошибка: {JsonSerializer.Serialize(exception)}");
        }

        #region Состояние "Рассылка"
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
        #endregion

        private async Task HadleIncomingDocumentMessage(Update update, ITelegramBotClient botClient)
        {
            var message = update.Message;
            var chatId = message.Chat.Id;
            var fileName = message.Document.FileName;

            await AttachmentHelper.SaveAttachmentFile(botClient, message, MessageType.Document, fileName);

            //Тестовое решение (вынести в отдельный метод)
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

            var connectionString = config.GetConnectionString("ConnectionString");

            var optionsBuilder = new DbContextOptionsBuilder<BeatsSenderDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            using (var dbContext = new BeatsSenderDbContext(optionsBuilder.Options))
            {
                var client = dbContext.Clients.FirstOrDefault(client => client.TelegramId == chatId.ToString());

                if (client == null)
                {
                    client = dbContext.CreateClient(chatId, message.From.Username);
                }

                //TODO добавить сохранение файла в БД

                dbContext.SaveChanges();
            }
        }

        private async Task HandleIncomingAudioMessage(Update update, ITelegramBotClient botClient)
        {
            var message = update.Message;
            var chatId = message.Chat.Id;

            var emailState = GetEmailState(chatId);
            var fileName = update.Message.Audio.FileName;

            if (emailState == EmailState.AwaitAttachments)
            {
                await AttachmentHelper.SaveAttachmentFile(botClient, message, MessageType.Audio, fileName);
                KeyboardHelper.SendAttachmentButtons(botClient, chatId, fileName);
            }
        }
    }
}
