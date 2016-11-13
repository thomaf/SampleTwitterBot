using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace TwitterApp
{

    public struct News
    {
        public string title;
        public string link;

        public News(string title, string link)
        {
            this.title = title;
            this.link = link;
        }
    }

    class TwitterBot
    {
        Random rnd;

        public TwitterBot(
            TwitterAuth twitterAuth)
        {
            rnd = new Random();
            Auth.SetUserCredentials(twitterAuth.ConsumerKey,
                twitterAuth.ConsumerSecret,
                twitterAuth.UserAccessToken,
                twitterAuth.UserAccessSecret);
            new Task(this.CollectCreateAndPostMessage)
                .Start();
            Console.ReadLine();
        }

        async void CollectCreateAndPostMessage()
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            Console.WriteLine("-- Creating tweet message for: " + authenticatedUser);
            var news = await GetRssFrom("https://news.ycombinator.com/rss");
            var theme = GetNewsTheme(news);

            Console.WriteLine("-- Retrieving tweets about " + theme);
            var tweets = GetTweets(theme);

            Console.WriteLine("-- Finding tags");
            var tags = GetTags(tweets);

            Console.WriteLine("-- Finding users");
            var users = GetUsers(tweets);

            Console.WriteLine("-- Generating message");
            var message = GenerateMessage(tags, users, news);

            Console.WriteLine("-- Sending message: " + message);
            // Tweet.PublishTweet(message);
        }

        async Task<News> GetRssFrom(string url)
        {
            var client = new HttpClient();
            var stream = await client.GetStreamAsync(url);
            var document = XDocument.Load(stream);

            var items = (
                    from el in document.Root.Element("channel").Elements("item")
                    select el
                ).ToList();
            var item = items[rnd.Next(items.Count)];

            return new News(
                item.Element("title").Value,
                item.Element("link").Value);
        }

        string GetNewsTheme(News news)
        {
            var result = (
                from s in news.title.Split(' ')
                orderby s.Length descending
                select s)
                .First();
            return result;
        }
        List<ITweet> GetTweets(string query, int max = 100, LanguageFilter lang = LanguageFilter.English)
        {
            if (query == "")
            {
                throw new System.Exception();
            }
            var searchParameter = new SearchTweetsParameters(query)
            {
                Lang = lang,
                SearchType = SearchResultType.Popular,
                MaximumNumberOfResults = max
            };

            return (List<ITweet>)Search.SearchTweets(searchParameter);
        }

        List<string> GetTags(List<ITweet> tweets)
        {

            var tagExpression = new Regex(@"#[a-zA-Z]+");
            return MathExpression(tweets, tagExpression);
        }
        List<string> GetUsers(List<ITweet> tweets)
        {

            var userExpression = new Regex(@"@[a-zA-Z]+");
            return MathExpression(tweets, userExpression);
        }

        List<string> MathExpression(List<ITweet> tweets, Regex expression)
        {
            var result = new List<string>();
            tweets.ForEach(t =>
            {
                Match match = expression.Match(t.Text);
                if (match.Success && !result.Contains(match.Value))
                {
                    result.Add(match.Value);
                };
            });
            return result;
        }

        string GenerateMessage(List<string> tags, List<string> users, News news)
        {
            var zero = popRandom(users);
            var one = news.title;
            var two = news.link;
            var three = popRandom(tags);

            var pattern = GetPattern();
            // String interpolation 
            return String.Format(pattern, zero, one, two, three);
        }

        string GetPattern()
        {
            var patterns = new List<string>{
                "Hey {0} you should read this {2} {3}",
                "Had good time reading \"{1}\" ({2}) {3}",
                "Interresting ? {2} cc. {0}",
                "Heard lot about {3}. {2}",
                "Haha : {2}. {3}",
            };
            return patterns[rnd.Next(patterns.Count)];

        }

        string popRandom(List<string> list)
        {
            if (list.Count == 0)
            {
                return "";
            }

            return list[rnd.Next(list.Count)];
        }

    }
}