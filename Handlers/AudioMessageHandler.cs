using BeatsSenderBot.Enums;
using BeatsSenderBot.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BeatsSenderBot.Handlers
{
    public static class AudioMessageHandler
    {
        /// <summary>
        /// Обработка и временное сохранение входящего бита
        /// </summary>
        public static async Task HandleIncomingAudioMessage(Update update, ITelegramBotClient botClient, Dictionary<long, SendBeatsState> emailStateDic)
        {
            var message = update.Message;
            var chatId = message.Chat.Id;

            var emailState = EmailStateHelper.GetEmailState(emailStateDic, chatId);
            var fileName = update.Message.Audio.FileName;

            if (emailState == SendBeatsState.AwaitAttachments)
            {
                await AttachmentHelper.SaveAttachmentFile(botClient, message, MessageType.Audio, fileName);
                KeyboardHelper.SendAttachmentButtons(botClient, chatId, fileName);
            }
        }
    }
}
