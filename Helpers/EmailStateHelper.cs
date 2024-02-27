using BeatsSenderBot.Enums;

namespace BeatsSenderBot.Helpers
{
    public static class EmailStateHelper
    {
        public static EmailState GetEmailState(Dictionary<long, EmailState> emailStateDic, long chatId)
        {
            if (emailStateDic.ContainsKey(chatId))
                return emailStateDic[chatId];

            return EmailState.AwaitAttachments;
        }

        public static void SaveEmailState(Dictionary<long, EmailState> emailStateDic, long chatId, EmailState state)
        {
            emailStateDic[chatId] = state;
        }

        public static void ResetEmailState(Dictionary<long, EmailState> emailStateDic, long chatId)
        {
            emailStateDic.Remove(chatId);
        }
    }
}
