using AIChatClient_BE.Models;
using AIChatClient_BE.Services.Interface;
using Microsoft.Extensions.AI;

namespace AIChatClient_BE.Services
{
    public class ChatService : IChatService
    {
        private static ChatMessage _systemMessage = new(ChatRole.System, "Your name is GeineX. Your task is to help people by providing the information they needed. You're always up for helping people out. You are very good at progrmming and other general-daily tasks. You can also analyse the images if given by user. You smartly analyse the images and provides a structured response.");

        private static readonly Dictionary<string, List<ChatMessage>> _chatHistories = new Dictionary<string, List<ChatMessage>>();

        //private static List<ChatMessage> _chatHistory = new()
        //{
        //    new ChatMessage(ChatRole.System, "Your name is GeineX. Your task is to help people by providing the information they needed. You're always up for helping people out. You are very good at progrmming and other general-daily tasks. You can also analyse the images if given by user. You smartly analyse image and provides a structured response including possible description along with title.")
        //};
        private readonly IChatClient _chatClient;
        public ChatService(IChatClient chatClient)
        {
            chatClient = _chatClient;
        }
        public async void AddToChatHistory(ChatRole role, PromptRequestModel prompt)
        {
            var chatId = prompt.ChatId;
            var chatHistory = await GetChatHistoryByChatId(chatId);
            if (chatHistory == null)
            {
                chatHistory = NewChat(chatId);
            }

            ChatMessage newMessage = new(role, prompt.Message);

            if (prompt.File != null)
            {
                using var memoryStream = new MemoryStream();
                await prompt.File.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                newMessage.Contents.Add(new DataContent(fileBytes, prompt.File.ContentType));
            }

            chatHistory.Add(newMessage);
        }
        public List<ChatMessage> NewChat(string chatId)
        {
            _chatHistories.Add(chatId, new List<ChatMessage> { _systemMessage });
            return _chatHistories[chatId];
        }
        public async Task<List<ChatMessage>> GetChatHistoryByChatId(string chatId)
        {
            if (_chatHistories.ContainsKey(chatId))
            {
                return _chatHistories[chatId];
            }
            else
            {
                return null;
            }
        }
    }
}
