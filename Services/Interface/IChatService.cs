using AIChatClient_BE.Models;
using Microsoft.Extensions.AI;

namespace AIChatClient_BE.Services.Interface
{
    public interface IChatService
    {
        void AddToChatHistory(ChatRole role, PromptRequestModel prompt);
        List<ChatMessage> NewChat(string chatId);
        Task<List<ChatMessage>> GetChatHistoryByChatId(string chatId);
    }
}
