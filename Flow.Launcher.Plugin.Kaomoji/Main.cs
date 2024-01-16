using System;
using System.Collections.Generic;
using System.Windows;
using Newtonsoft.Json;
using System.Linq;
using Flow.Launcher.Plugin;


namespace Flow.Launcher.Plugin.Kaomoji
{
    public class Kaomoji : IPlugin
    {
        private PluginInitContext _context;
        
        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            string jsonFilePath = _context.CurrentPluginMetadata.PluginDirectory + "\\Assets\\kaomoji.json";
            string jsonContent = System.IO.File.ReadAllText(jsonFilePath);

            KaomojiList kaomojiList = JsonConvert.DeserializeObject<KaomojiList>(jsonContent);
            List<KaomojiEntry> matchingKaomojis = SearchByKeyword(kaomojiList.Kaomojis, query.Search.ToLower());

            var results = new List<Result>();

            foreach(var kaomoji in matchingKaomojis)
            {
                var result = new Result
                {
                    Title = kaomoji.Text,
                    SubTitle = kaomoji.Keywords,
                    IcoPath = "Assets/icon.png",
                    Action = c => {
                        Clipboard.SetDataObject(kaomoji.Text);
                        return true;
                    }
                };

                results.Add(result);
            }

            return results;
        }

        static List<KaomojiEntry> SearchByKeyword(List<KaomojiEntry> kaomojis, string query)
        {
            List<KaomojiEntry> matchingKaomojis = new List<KaomojiEntry>();

            var keywords = query.Split(" ");

            foreach (var kaomoji in kaomojis)
            {
                kaomoji.matches = 0;

                foreach(var keyword in keywords)
                {
                    if (kaomoji.Keywords.Contains(keyword))
                    {
                        kaomoji.matches += 1;
                    }
                }
            }

            return kaomojis
                .Where(k => k.matches == keywords.Length)
                .OrderBy(k => k.matches)
                .ToList();
        }
    }

    class KaomojiList
    {   
        public List<KaomojiEntry> Kaomojis { get; set; }
    }

    class KaomojiEntry
    {
        public string Text { get; set; }
        public string Keywords { get; set; }
        public int matches { get; set; }
    }
}