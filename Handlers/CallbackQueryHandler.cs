using BeatsSenderBot.Enums;
using BeatsSenderBot.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BeatsSenderBot.Handlers
{
    public static class CallbackQueryHandler
    {
        public static async Task HandleCallbackQuery(Update update, ITelegramBotClient botClient, Dictionary<long, EmailState> emailStateDic)
        {
            var buttonCode = update.CallbackQuery.Data;
            var chatId = update.CallbackQuery.Message.Chat.Id;
            var emailState = EmailStateHelper.GetEmailState(emailStateDic, chatId);

            #region Обработка кнопок раздела "Рассылка"
            //Нажатие на кнопку "Рассылка"
            if (buttonCode == "mailing")
            {
                await botClient.SendTextMessageAsync(chatId, "Прикрепите файлы для рассылки");
                EmailStateHelper.SaveEmailState(emailStateDic, chatId, EmailState.AwaitAttachments);
            }

            //Нажатие на кнопку "Отправить"
            if (buttonCode == "sendAttachments" && emailState == EmailState.AwaitSendAttachments)
            {
                EmailHelper.SendAttachments(chatId);
                EmailStateHelper.ResetEmailState(emailStateDic, chatId);
                await botClient.SendTextMessageAsync(chatId, "Письмо отправлено!");
                KeyboardHelper.StartMessageButtons(botClient, chatId);
            }

            //Нажатие на кнопку "Продолжить"
            if (buttonCode == "continue")
            {
                EmailStateHelper.SaveEmailState(emailStateDic, chatId, EmailState.AwaitSendAttachments);
                KeyboardHelper.SendEmailButtons(botClient, chatId);
            }

            //Нажатие на кнопку "Прикрепить ещё"
            if (buttonCode == "addMoreAttachments")
            {
                await botClient.SendTextMessageAsync(chatId, "Прикрепите файлы для рассылки");
            }
            #endregion

            #region Обработка кнопок раздела "Почты"
            //Нажатие на кнопку "Почты"
            if (buttonCode == "email")
            {
                await botClient.SendTextMessageAsync(chatId, "Прикрепите текстовый файл со списком почт для рассылки");
            }
            #endregion
        }
    }
}
