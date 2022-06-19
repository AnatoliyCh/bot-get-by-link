using Bot.GetByLink.Common.Infrastructure;
using Bot.GetByLink.Common.Infrastructure.Abstractions;
using Newtonsoft.Json;
using Reddit;
using System.Net.Http.Headers;
using System.Text;
using Telegram.Bot.Types;
using System.Text.RegularExpressions;

namespace Bot.GetByLink.Proxy.Reddit
{
    /// <summary>
    /// Апи реддита для получения контента поста по id или url.
    /// </summary>
    public class ProxyReddit : ProxyService
    {
        private const string UriString = "www.reddit.com";
        private readonly string appId;
        private readonly string secretId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyReddit"/> class.
        /// </summary>
        /// <param name="appId">Id application Reddit.</param>
        /// <param name="secretId">Secret Id application Reddit.</param>
        /// <param name="regexUrl">Regex url for proxy.</param>
        public ProxyReddit(string[] regexUrl, string appId, string secretId)
            : base(regexUrl)
        {
            this.appId = appId;
            this.secretId = secretId;
        }

        /// <summary>
        /// Метод для получения контента поста реддита по url поста.
        /// </summary>
        /// <param name="url">Url на пост реддита формата https?://www.reddit.com/r/S+/comments/S+.</param>
        /// <returns>Объект с текстом и ссылками на картинки и видео присутствующие в посте.</returns>
        public override async Task<TelegramMessage> GetContentUrl(string url)
        {
            var cutUrlPost = url[(url.IndexOf("comments/") + "comments/".Length)..];
            var postId = cutUrlPost[..cutUrlPost.IndexOf("/")];
            return await GetContentId(postId);
        }

        /// <summary>
        /// Метод для получения контента поста реддита по ид поста.
        /// </summary>
        /// <param name="postId">Ид поста.</param>
        /// <returns>Объект с текстом и ссылками на картинки и видео присутствующие в посте.</returns>
        /// <exception cref="ArgumentNullException">Возвращается в случае если был передан пустой postId.</exception>
        public async Task<TelegramMessage> GetContentId(string postId)
        {
            if (string.IsNullOrWhiteSpace(postId))
            {
                throw new ArgumentNullException(nameof(postId), "Пустой id поста");
            }

            var accsessToken = await GetAccessToken();
            var redditAccess = new RedditClient(appId, appSecret: secretId, accessToken: accsessToken, userAgent: "bot-get-by-link-web");
            var post = redditAccess.LinkPost($"t3_{postId}").Info();
            if (post.Listing.Media == null && !post.Listing.IsVideo && !post.Listing.IsRedditMediaDomain) return new TelegramMessage() { Text = $"{post.Listing.URL}\n\n{post.Listing.SelfText}" };
            if (Regex.IsMatch(post.Listing.URL, @"https?://\S+(?:jpg|jpeg|png)", RegexOptions.IgnoreCase)) return new TelegramMessage() { Text = post.Listing.SelfText, UrlPicture = post.Listing.URL };
            return new TelegramMessage() { Text = post.Listing.SelfText, UrlVideo = post.Listing.URL };
        }

        /// <summary>
        /// Function for get accsess token from reddit.
        /// </summary>
        /// <returns>Accsess token reddit.</returns>
        /// <exception cref="HttpRequestException">Возвращается если не получилось авторизоваться в реддите или он вернул пустой ответ.</exception>
        private async Task<string> GetAccessToken()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri($"https://{UriString}");
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/access_token");

            var byteArray = new UTF8Encoding().GetBytes($"{appId}:{secretId}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            formData.Add(new KeyValuePair<string, string>("device_id", appId));

            request.Content = new FormUrlEncodedContent(formData);
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var accsessTokenObject = JsonConvert.DeserializeObject<AccessTokenReddit>(await response.Content.ReadAsStringAsync());
            if (accsessTokenObject == null || string.IsNullOrWhiteSpace(accsessTokenObject.AccessToken)) throw new HttpRequestException($"Реддит вернул ошибку: {response.Content.ReadAsStringAsync().Result}");
            return accsessTokenObject.AccessToken;
        }
    }
}