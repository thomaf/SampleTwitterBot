using System;
using System.IO;
using Newtonsoft.Json;

namespace TwitterApp
{

    class Program
    {
        static void Main(string[] args)
        {
            var config = GetConfig();
            new TwitterBot(config.TwitterAuth);
        }

        static Config GetConfig(){
            var fileContent="";
            using (StreamReader reader = File.OpenText("config.json"))
            {
                fileContent = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<Config>(fileContent);
        }

    }
}