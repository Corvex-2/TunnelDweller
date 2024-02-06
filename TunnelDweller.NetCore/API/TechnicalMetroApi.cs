using Newtonsoft.Json;
using SevenZip.Compression.LZMA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TechnicalApi.API.Shared;

namespace TunnelDweller.NetCore.API
{
    internal static class TechnicalMetroApi
    {
        public static string RELEASESTREAM { get; set; } = "{RELEASE}";

        public const string API_ENDPOINT = "http://api.technicaldifficulties.de/metro/tunneldweller/";
        public const string API_MODULES = "builds/modulefiles?releaseStream={RELEASE}";

        public static List<(string moduleName, byte[] moduleData)> GetModules()
        {
            var modules = new List<(string moduleName, byte[] moduleData)>();

            var API_URL = API_ENDPOINT + API_MODULES.Replace("{RELEASE}", RELEASESTREAM);

            var response = GetResponseFromApi<MultiMessageResponse>(API_URL);

            if (response.Result != Result.Success)
                return modules;


            for(int i = 0; i < response.Message.Length; i++)
            {
                var split = response.Message[i].Split(':');
                if (split.Length != 2)
                    continue;
                var name = split[0];
                var value = LzmaHelper.Decompress(Convert.FromBase64String(split[1]));
                modules.Add((name, value));
            }

            return modules;
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
