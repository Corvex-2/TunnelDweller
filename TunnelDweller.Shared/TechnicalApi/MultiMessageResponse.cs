using Newtonsoft.Json;

namespace TechnicalApi.API.Shared
{
    public class MultiMessageResponse
    {
        public Result Result { get; set; }
        public string[] Message { get; set; }

        public MultiMessageResponse()
        {
            Result = Result.Empty;
            Message = new string[] { };
        }
        public MultiMessageResponse(Result Result)
        {
            this.Result = Result;
            Message = new string[] { Result.ToString(), };
        }
        public MultiMessageResponse(Result result, string[] messages)
        {
            Result = result;
            Message = messages;
        }

        public override string ToString()
        {
            return $"Request Concluded with: {Result}\r\nMessage: {Message}";
        }

        public static string ToJson(MultiMessageResponse result)
        {
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        public static MultiMessageResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MultiMessageResponse>(json);
        }
    }

    public static class MultiMessageResponseEx
    {
        public static string ToJsonEx(this MultiMessageResponse e)
        {
            return MultiMessageResponse.ToJson(e);
        }
    }
}
