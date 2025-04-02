using System.Data;
using AIChatClient_BE.Models;
using AIChatClient_BE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;

namespace AIChatClient_BE.Controllers
{
    [ApiController]
    public class AIController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IChatClient _chatClient;
        public AIController(IChatService chatService, IChatClient chatClient)
        {
            _chatService = chatService;
            _chatClient = chatClient;
        }


        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromForm] PromptRequestModel chatPrompt)
        {
            if(string.IsNullOrEmpty(chatPrompt.Message))
            {
                return BadRequest(new GeneralResponse
                {
                    Success = false,
                    Message = "Message cannot be empty"
                });
            }

            _chatService.AddToChatHistory(ChatRole.User, chatPrompt);

            var chatHistory = await _chatService.GetChatHistoryByChatId(chatPrompt.ChatId);

            Thread.Sleep(1000);

            var response = await _chatClient.GetResponseAsync(chatHistory);

            var assistantMessage = new PromptRequestModel
            {
                ChatId = chatPrompt.ChatId,
                Message = response.Text
            };

            _chatService.AddToChatHistory(ChatRole.Assistant, assistantMessage);

            return Ok(new GeneralResponse
            {
                Success = true,
                Message = "Response from AI",
                Data = response.Text
            });
        }

        [HttpGet("newchat")]
        public async Task<IActionResult> NewChat([FromBody] string chatId)
        {
            return Ok();
        }

        //[HttpPost("streamchat")]
        //public async IAsyncEnumerable<string> StreamChat([FromBody] string chatPrompt)
        //{
        //    _chatService.AddToChatHistory(ChatRole.User, chatPrompt);

        //    var chatResponse = "";
        //    await foreach (var chunk in _chatClient.GetStreamingResponseAsync(_chatService.GetChatHistory()))
        //    {
        //        chatResponse += chunk.Text;
        //        yield return chunk.Text;
        //    }

        //    _chatService.AddToChatHistory(ChatRole.Assistant, chatResponse);
        //}
    }
}
