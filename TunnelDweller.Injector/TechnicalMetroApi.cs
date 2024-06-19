using Newtonsoft.Json;
using SevenZip.Compression.LZMA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TechnicalApi.API.Shared;

namespace TunnelDweller.Injector
{
    public static class TechnicalMetroApi
    {
        public const string API_ENDPOINT = "http://api.technicaldifficulties.de/metro/tunneldweller/";
        public const string API_CORE = "builds/corefiles?releaseStream={RELEASE}";
        public const string API_RELEASESTREAMS = "builds/releasestreams";
        public const string API_INJECTORVERSION = "injector/version";
        //public const string API_INJECTORFILES = "injector/files";
        public const string API_NEWS = "news";

        public static string[] GetReleaseStreams()
        {
            var API_URL = API_ENDPOINT + API_RELEASESTREAMS;

            var response = GetResponseFromApi<MultiMessageResponse>(API_URL);
            
            if(response.Result != Result.Success)
                return new string[] { }; // return non null for safety!
            return response.Message;
        }

        public static string GetVersion()
        {
            var API_URL = API_ENDPOINT + API_INJECTORVERSION;

            var response = GetResponseFromApi<MessageResponse>(API_URL);

            if (response.Result != Result.Success)
                return "invalid"; // return non null for safety!
            return response.Message;
        }

        public static string GetNews()
        {
            var API_URL = API_ENDPOINT + API_NEWS;

            var response = GetResponseFromApi<MessageResponse>(API_URL);

            if (response.Result != Result.Success)
                return "Seems like there are no news!"; // return non null for safety!
            return response.Message;
        }

        public static List<TechnicalMetroApiFile> GetCores(string releaseStream)
        {
            var API_URL = API_ENDPOINT + API_CORE.Replace("{RELEASE}", releaseStream);

            var response = GetResponseFromApi<MultiMessageResponse>(API_URL);

            if (response.Result != Result.Success)
                return new List<TechnicalMetroApiFile>();

            List<TechnicalMetroApiFile> files = new List<TechnicalMetroApiFile>();

            for(int i = 0; i < response.Message.Length; i++)
            {
                var msgSplit = response.Message[i].Split(':');

                if (msgSplit.Length != 2)
                    continue;
                files.Add(new TechnicalMetroApiFile(msgSplit[0], LzmaHelper.Decompress(Convert.FromBase64String(msgSplit[1]))));


                //if (msgSplit[0].StartsWith("TunnelDweller.NetCore"))
                //{
                //    var data = Convert.FromBase64String(msgSplit[1]);
                //    if (data != null)
                //        netcore = LzmaHelper.Decompress(data);
                //}
                //if (msgSplit[0].StartsWith("TunnelDweller.Core"))
                //{
                //    var data = Convert.FromBase64String(msgSplit[1]);
                //    if (data != null)
                //        core = LzmaHelper.Decompress(data);
                //}
            }
            return files;
        }

        public static T GetResponseFromApi<T>(string api)
        {
            var request = WebRequest.CreateHttp(api); //obsolete but who gives a flying fuck. 
            var response = request.GetResponse();
            using (StreamReader r = new StreamReader(response.GetResponseStream()))
            {
                var str = r.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(str);
            }
        }
    }
}
