using BeatsSenderBot.Constants;
using BeatsSenderBot.Helpers;
using BeatsSenderDb.Extensions;
using BeatsSenderDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BeatsSenderBot.Handlers
{
    public static class DocumentMessageHandler
    {
        public static async Task HandleIncomingDocumentMessage(Update update, ITelegramBotClient botClient)
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

                client.MailFile = GetDocument(chatId);

                dbContext.SaveChanges();
            }
        }

        private static byte[] GetDocument(long chatId)
        {
            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FolderConstants.EmailsFolderName);
            var filePaths = Directory.GetFiles(Path.Combine(folderPath, chatId.ToString()));

            if (!filePaths.Any())
            {
                throw new Exception($"Не удалось получить файл с почтами. Идентификатор чата: {chatId}");
            }

            return System.IO.File.ReadAllBytes(filePaths[0]);
        }
    }
}
