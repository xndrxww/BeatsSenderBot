using Telegram.Bot;
using Telegram.Bot.Polling;

namespace BeatsSenderBot
{
    public class Program
    {
        private static readonly ITelegramBotClient _botClient = new TelegramBotClient("6367612655:AAH8VBioHZrYKkNPOoObXQ6ZsVgzFo9PQ8U"); //TODO вынести в конфиг

        static void Main(string[] args)
        {
            StartInfo();

            var handler = new UpdateHandler();
            var cancellationToken = new CancellationToken();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } //принимает все изменения
            };

            _botClient.StartReceiving(handler, receiverOptions, cancellationToken);
            Console.ReadLine();
        }

        private static void StartInfo()
        {
            var botInfo = _botClient.GetMeAsync().Result;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Запуск бота: {botInfo.FirstName}");
            Console.WriteLine($"Информация: {botInfo}");
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
