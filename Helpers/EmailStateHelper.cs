using BeatsSenderBot.Enums;

namespace BeatsSenderBot.Helpers
{
    public static class EmailStateHelper
    {
        public static SendBeatsState GetEmailState(Dictionary<long, SendBeatsState> emailStateDic, long chatId)
        {
            if (emailStateDic.ContainsKey(chatId))
                return emailStateDic[chatId];

            return SendBeatsState.AwaitAttachments;
        }

        public static void SaveEmailState(Dictionary<long, SendBeatsState> emailStateDic, long chatId, SendBeatsState state)
        {
            emailStateDic[chatId] = state;
        }

        public static void ResetEmailState(Dictionary<long, SendBeatsState> emailStateDic, long chatId)
        {
            emailStateDic.Remove(chatId);
        }
    }
}
