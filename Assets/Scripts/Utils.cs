using System;

namespace Utils
{
    public static class Url
    {
        /// <summary>
        /// Generates media url with optional cache usage for texture images
        /// </summary>
        /// <param name="mediaStaticUrl">global static media url header</param>
        /// <param name="url">original url</param>
        /// <param name="useCache">Whether to use cache for the texture request</param>
        /// <returns>Read-to-use URL for requests</returns>
        public static string GenerateURL(string mediaStaticUrl, string url, bool useCache = true)
        {
            if (!useCache)
            {
                TimeSpan timeSpan = DateTime.UtcNow - new DateTime(2020, 1, 1, 1, 1, 1);
                return string.Concat(
                    mediaStaticUrl,
                    url,
                    "?timestamp=",
                    timeSpan.TotalSeconds.ToString()
                );
            }
            return string.Concat(mediaStaticUrl, url);
        }
    }
}
