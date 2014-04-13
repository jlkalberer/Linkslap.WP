namespace Linkslap.WP.Utils
{
    using System;
    using System.Windows.Navigation;

    /// <summary>
    /// The linkslap uri mapper.
    /// </summary>
    internal class LinkslapUriMapper : UriMapperBase
    {
        /// <summary>
        /// The map uri.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <returns>
        /// The <see cref="Uri"/>.
        /// </returns>
        public override Uri MapUri(Uri uri)
        {
            var tempUri = System.Net.HttpUtility.UrlDecode(uri.ToString()).ToLowerInvariant();

            // URI association launch for contoso.
            if (tempUri.Contains("linkslap:home"))
            {
                // Get the category ID (after "CategoryID=").
                // int categoryIdIndex = tempUri.IndexOf("CategoryID=") + 11;
                // string categoryId = tempUri.Substring(categoryIdIndex);

                // Map the show products request to ShowProducts.xaml
                // return new Uri("/ShowProducts.xaml?CategoryID=" + categoryId, UriKind.Relative);
                return new Uri("/Views/Home.xaml", UriKind.Relative);
            }

            // Otherwise perform normal launch.
            return uri;
        }
    }
}
