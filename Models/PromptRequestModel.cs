namespace AIChatClient_BE.Models
{
    public class PromptRequestModel
    {
        public string Message { get; set; }
        public IFormFile? File { get; set; }
    }
}
