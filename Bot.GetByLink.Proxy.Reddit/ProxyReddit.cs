using Newtonsoft.Json;
using Reddit;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Bot.GetByLink.Proxy.Reddit
{
    /// <summary>
    /// Апи реддита для получения поста по id.
    /// </summary>
    public class ProxyReddit
    {
        private const string UriString = "www.reddit.com";
        private readonly string appId;
        private readonly string secretId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyReddit"/> class.
        /// Создание класса Реддита.
        /// </summary>
        /// <param name="appId">Id приложения реддита.</param>
        /// <param name="secretId">Secret Id приложения реддита.</param>
        public ProxyReddit(string appId, string secretId)
        {
            this.appId = appId;
            this.secretId = secretId;
        }

        /// <summary>
        /// Метод для получения контента поста реддита по ид поста.
        /// </summary>
        /// <param name="postId">Ид поста.</param>
        /// <returns>Объект с текстом и ссылками на картинки и видео присутствующие в посте.</returns>
        /// <exception cref="ArgumentNullException">Возвращается в случае если был передан пустой postId.</exception>
        /// <exception cref="HttpRequestException">Возвращается если не получилось авторизоваться в реддите или он вернул пустой ответ.</exception>
        public TelegramMessage GetPostId(string postId)
        {
            if (string.IsNullOrWhiteSpace(postId))
            {
                throw new ArgumentNullException(nameof(postId), "Пустой id поста");
            }

            var client = new HttpClient();
            client.BaseAddress = new Uri($"https://{UriString}");
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/access_token");

            var byteArray = new UTF8Encoding().GetBytes($"{appId}:{secretId}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            formData.Add(new KeyValuePair<string, string>("device_id", appId));

            request.Content = new FormUrlEncodedContent(formData);
            var response = client.SendAsync(request).GetAwaiter().GetResult();

            var accsessTokenObject = JsonConvert.DeserializeObject<AccessTokenReddit>(response.Content.ReadAsStringAsync().Result);
            response.EnsureSuccessStatusCode();

            if (accsessTokenObject == null) throw new HttpRequestException($"Реддит вернул ошибку: {response.Content.ReadAsStringAsync().Result}");
            var redditAccess = new RedditClient(appId, appSecret: secretId, accessToken: accsessTokenObject.AccessToken, userAgent: "bot-get-by-link-web");
            var post = redditAccess.LinkPost($"t3_{postId}").Info();
            if (post.Listing.Media == null && !post.Listing.IsVideo && !post.Listing.IsRedditMediaDomain) return new TelegramMessage() { Text = $"{post.Listing.URL}\n\n{post.Listing.SelfText}" };
            if (Regex.IsMatch(post.Listing.URL, @"https?://\S+(?:jpg|jpeg|png)", RegexOptions.IgnoreCase)) return new TelegramMessage() { Text = post.Listing.SelfText, UrlPicture = post.Listing.URL };
            return new TelegramMessage() { Text = post.Listing.SelfText, UrlVideo = post.Listing.URL };
        }
    }
}