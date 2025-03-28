using AIChatClient_BE.Models;
using Microsoft.Extensions.AI;

namespace AIChatClient_BE.Services.Interface
{
    public interface IChatService
    {
        Task<string> GetResponse(string message);
        void AddToChatHistory(ChatRole role, PromptRequestModel prompt);
        List<ChatMessage> GetChatHistory();
    }
}
