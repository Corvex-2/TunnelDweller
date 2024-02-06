using Newtonsoft.Json;
using SevenZip.Compression.LZMA;
using System.Net;
using System.Security.Cryptography;
using TechnicalApi.API.Shared;

namespace TunnelDweller.Updater
{
    public class Updater
    {
        public const string API_ENDPOINT = "http://api.technicaldifficulties.de/metro/tunneldweller/";
        public const string API_INJECTORVERSION = "injector/version";
        public const string API_INJECTORFILES = "injector/files";

        public static void Main(string[] args)
        {
            Console.Title = "TunnelDweller - Updater";

            Console.WriteLine("Checking for Injector File...");
            if (!File.Exists("TunnelDweller.Injector.exe"))
            {
                Console.WriteLine("Unable to locate current injector. Downloading files from Server...");

                var files = GetInjectorFilesFromRemote();

                if(files.Count == 0)
                {
                    Console.WriteLine("Unable to connect to update server.");
                    Thread.Sleep(2500);
                    return;
                }
                else
                    Console.WriteLine("Download from Server successful.");
                foreach (var file in files)
                {
                    Console.WriteLine($"Creating file {file}");
                    File.Create(file.fileName).Close();
                    File.WriteAllBytes(file.fileName, file.fileData);
                }
                Console.WriteLine("Download complete.");

                Thread.Sleep(2500);
                return;
            }
            else
            {
                Console.WriteLine("Checking for updates...");
                var remote = GetRemoteHash();
                var local = GetHash("TunnelDweller.Injector.exe");

                Console.WriteLine(remote);
                Console.WriteLine(local);

                if (local == remote)
                {
                    Console.WriteLine("Your injector does not require any updates!");
                    Thread.Sleep(2500);
                    return;
                }
                Console.WriteLine("Update found! Downloading files from Server...");

                var files = GetInjectorFilesFromRemote();

                if (files.Count == 0)
                {
                    Console.WriteLine("Unable to connect to update server.");
                    Thread.Sleep(2500);
                    return;
                }
                else
                    Console.WriteLine("Download from Server successful.");

                foreach (var file in files)
                {
                    Console.WriteLine($"Writing or creating file {file}");
                    File.Create(file.fileName).Close();
                    File.WriteAllBytes(file.fileName, file.fileData);
                }

                Console.WriteLine("Download complete.");
                Thread.Sleep(2500);
            }
        }

        public static string GetHash(string fileName)
        {
            return Convert.ToBase64String(SHA256.Create().ComputeHash(File.ReadAllBytes(fileName)));
        }

        public static string GetRemoteHash()
        {
            var resp = GetResponseFromApi<MessageResponse>(API_ENDPOINT + API_INJECTORVERSION);
            return resp.Message;
        }

        public static List<(string fileName, byte[] fileData)> GetInjectorFilesFromRemote()
        {
            var modules = new List<(string moduleName, byte[] moduleData)>();

            var API_URL = API_ENDPOINT + API_INJECTORFILES;

            var response = GetResponseFromApi<MultiMessageResponse>(API_URL);

            if (response.Result != Result.Success)
                return modules;


            for (int i = 0; i < response.Message.Length; i++)
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



