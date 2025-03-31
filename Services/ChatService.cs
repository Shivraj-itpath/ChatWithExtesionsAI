using AIChatClient_BE.Models;
using AIChatClient_BE.Services.Interface;
using Microsoft.Extensions.AI;

namespace AIChatClient_BE.Services
{
    public class ChatService : IChatService
    {
        private static List<ChatMessage> _chatHistory = new()
        {
            new ChatMessage(ChatRole.System, "Your name is GeineX. Your task is to help people by providing the information they needed. You're always up for helping people out. You are very good at progrmming and other general-daily tasks. You can also analyse the images if given by user. You smartly analyse image and provides a structured response including possible description along with title.")
        };
        private readonly IChatClient _chatClient;
        public ChatService(IChatClient chatClient)
        {
            chatClient = _chatClient;
        }
        public async Task<string> GetResponse(string message)
        {
            try
            {
                ChatResponse chatResponse = await _chatClient.GetResponseAsync(message);
                return chatResponse.Text;
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }
        public async void AddToChatHistory(ChatRole role, PromptRequestModel prompt)
        {
            //_chatHistory.Add(new ChatMessage(role, message));
            ChatMessage newMessage = new(role, prompt.Message);

            if (prompt.File != null)
            {
                using var memoryStream = new MemoryStream();
                await prompt.File.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                newMessage.Contents.Add(new DataContent(fileBytes, prompt.File.ContentType));
            }

            _chatHistory.Add(newMessage);
        }
        public List<ChatMessage> GetChatHistory()
        {
            return _chatHistory;
        }
    }
}
