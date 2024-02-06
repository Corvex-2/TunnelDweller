using Newtonsoft.Json;

namespace TechnicalApi.API.Shared
{
    public class MessageResponse
    {
        public Result Result { get; set; }
        public string Message { get; set; }

        public MessageResponse() 
        { 
            Result = Result.Empty;
            Message = string.Empty;
        }
        public MessageResponse(Result Result)
        {
            this.Result = Result;
            Message = "SUCCESS";
        }
        public MessageResponse(Result result, string message)
        {
            Result = result;
            Message = message;
        }

        public override string ToString()
        {
            return $"Request Concluded with: {Result}\r\nMessage: {Message}";
        }

        public static string ToJson(MessageResponse result)
        {
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        public static MessageResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MessageResponse>(json);
        }
    }

    public static class MessageResponseEx
    {
        public static string ToJsonEx(this MessageResponse e)
        {
            return MessageResponse.ToJson(e);
        }
    }
}
