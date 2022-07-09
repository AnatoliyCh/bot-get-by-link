using Bot.GetByLink.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.GetByLink.Client.Telegram.Polling
{
    /// <summary>
    /// Class for formating message.
    /// </summary>
    public class FormaterMessage
    {
        /// <summary>
        /// Gets text messages.
        /// </summary>
        public List<string> Messages { get; }

        /// <summary>
        /// Gets media messages.
        /// </summary>
        public List<IAlbumInputMedia> AlbumInputMedias { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormaterMessage"/> class.
        /// </summary>
        /// <param name="responseContent">Proxy content.</param>
        public FormaterMessage(ProxyResponseContent responseContent)
        {
            // TODO Потестить отдельно гифки
            (var textUrl, var urlPictures, var urlVideo) = GetTextUrlAndValidUrl(responseContent.UrlPicture, responseContent.UrlVideo);
            var hasMedia = urlPictures.Length > 0 || urlVideo.Length > 0;
            var textMessage = $"{textUrl}{responseContent.Text}";
            var captionLength = hasMedia ? Math.Min(1024, textMessage.Length) : 0;
            var captionMedia = textMessage[..captionLength];

            AlbumInputMedias = GetPhotoInputMedia(urlPictures, captionMedia);
            AlbumInputMedias.AddRange(GetVideoInputMedia(urlVideo, captionMedia, AlbumInputMedias.Count < 1));
            Messages = GetMessageArray(textMessage, captionLength);
        }

        /// <summary>
        ///     Function get text url that much max size (5mb pic and 20 video) and valid url.
        /// </summary>
        /// <param name="urlPicture">Array url pictures.</param>
        /// <param name="urlVideo">Array url viedos.</param>
        /// <returns>Text with url and array valid url.</returns>
        private static (string MutableUrlText, string[] MutableUrlPicture, string[] MutableUrlVideo) GetTextUrlAndValidUrl(string[] urlPicture, string[] urlVideo)
        {
            var urlText = string.Empty;
            var mutableUrlPicture = new List<string>();
            var mutableUrlVideo = new List<string>();
            var client = new HttpClient();
            foreach (var inputMediaPhoto in urlPicture)
            {
                long result = GetSizeContentUrl(client, inputMediaPhoto);
                if (result < 0 || result > 5) urlText = $"{inputMediaPhoto}\n{urlText}";
                else mutableUrlPicture.Add(inputMediaPhoto);
            }

            foreach (var inputMediaVideo in urlVideo)
            {
                long result = GetSizeContentUrl(client, inputMediaVideo);
                if (result < 0 || result > 20) urlText = $"{inputMediaVideo}\n{urlText}";
                else mutableUrlVideo.Add(inputMediaVideo);
            }

            return (urlText, mutableUrlPicture.ToArray(), mutableUrlVideo.ToArray());
        }

        /// <summary>
        ///     Function for get size content from url.
        /// </summary>
        /// <param name="client">Http client.</param>
        /// <param name="url">Url for get size content.</param>
        /// <returns>Size content.</returns>
        private static long GetSizeContentUrl(HttpClient client, string url)
        {
            long result = -1;
            client.BaseAddress = new Uri(url);
            var request = new HttpRequestMessage(HttpMethod.Head, string.Empty);
            var message = client.SendAsync(request).GetAwaiter().GetResult();
            if (long.TryParse(message.Content.Headers.FirstOrDefault(h => h.Key.Equals("Content-Length")).Value.FirstOrDefault(), out long contentLength))
            {
                result = contentLength == 0 ? contentLength : contentLength / (1024 * 1024);
            }

            return result;
        }

        /// <summary>
        ///     Function for get formating list IAlbumInputMedia from array picture.
        /// </summary>
        /// <param name="urlPictures">Array url picture.</param>
        /// <returns>Formating list IAlbumInputMedia.</returns>
        private static List<IAlbumInputMedia> GetPhotoInputMedia(string[] urlPictures, string captionMedia)
        {
            var albumInputMedias = new List<IAlbumInputMedia>();
            if (urlPictures.Length > 0)
            {
                var listInputMediaPhoto = new List<InputMediaPhoto>();
                foreach (var inputMediaPhoto in urlPictures)
                {
                    listInputMediaPhoto.Add(new InputMediaPhoto(inputMediaPhoto));
                }

                listInputMediaPhoto[0].Caption = captionMedia;
                albumInputMedias = listInputMediaPhoto.ToList<IAlbumInputMedia>();
            }

            return albumInputMedias;
        }

        /// <summary>
        ///     Function for get formating list IAlbumInputMedia from array video.
        /// </summary>
        /// <param name="urlVideo">Array url video.</param>
        /// <returns>Formating list IAlbumInputMedia.</returns>
        private static List<IAlbumInputMedia> GetVideoInputMedia(string[] urlVideo, string captionMedia, bool isFirstMedia)
        {
            var albumInputMedias = new List<IAlbumInputMedia>();
            if (urlVideo.Length > 0)
            {
                var listInputMediaVideo = new List<InputMediaVideo>();
                foreach (var inputMediaVideo in urlVideo)
                {
                    listInputMediaVideo.Add(new InputMediaVideo(inputMediaVideo));
                }

                if (isFirstMedia) listInputMediaVideo[0].Caption = captionMedia;

                albumInputMedias.AddRange(listInputMediaVideo.ToList<IAlbumInputMedia>());
            }

            return albumInputMedias;
        }

        /// <summary>
        ///     Function for get formating message array..
        /// </summary>
        /// <param name="textPost">Text post.</param>
        /// <param name="captionLength">Lenght text caption picture or video.</param>
        /// <returns>Formating list IAlbumInputMedia.</returns>
        private static List<string> GetMessageArray(string textPost, int captionLength)
        {
            var messages = new List<string>();
            if (textPost.Length > captionLength)
            {
                textPost = textPost[captionLength..];
                for (int i = 0; textPost.Length > 4096; i++)
                {
                    messages.Add(textPost.Remove(4096));
                    textPost = textPost[4096..];
                }

                messages.Add(textPost);
            }

            return messages;
        }
    }
}
