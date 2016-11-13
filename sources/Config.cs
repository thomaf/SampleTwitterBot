using Newtonsoft.Json;

public class Config
{
    [JsonProperty("twitterAuth")]
    public TwitterAuth TwitterAuth { get; set; }
}

public class TwitterAuth
{

    [JsonProperty("consumerKey")]
    public string ConsumerKey { get; set; }
    [JsonProperty("consumerSecret")]
    public string ConsumerSecret { get; set; }
    [JsonProperty("userAccessToken")]
    public string UserAccessToken { get; set; }
    [JsonProperty("userAccessSecret")]
    public string UserAccessSecret { get; set; }
}