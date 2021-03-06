﻿namespace Linkslap.WP.Communication
{
    using System;

    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    using Newtonsoft.Json;

    using Windows.Foundation;
    using Windows.Web.Http;
    using Windows.Web.Http.Headers;

    using HttpClient = Windows.Web.Http.HttpClient;
    using HttpMethod = Windows.Web.Http.HttpMethod;
    using HttpRequestMessage = Windows.Web.Http.HttpRequestMessage;
    using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

    /// <summary>
    /// The rest.
    /// </summary>
    public class Rest
    {
        /// <summary>
        /// The base url.
        /// </summary>
        private readonly string baseUrl;

        /// <summary>
        /// The bearer token.
        /// </summary>
        private readonly string bearerToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rest"/> class.
        /// </summary>
        public Rest()
            : this(AppSettings.BaseUrl)
        {
            var account = Storage.Load<Account>("account");

            if (account != null)
            {
                this.bearerToken = account.BearerToken;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rest"/> class.
        /// </summary>
        /// <param name="baseUrl">
        /// The base Url.
        /// </param>
        public Rest(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="resource">
        /// The resource.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Get<TModel>(string resource, Action<TModel> callback = null)
            where TModel : new()
        {
            this.Execute(HttpMethod.Get, resource, null, callback);
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="resource">
        /// The resource.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Get<TModel>(string resource, object parameters = null, Action<TModel> callback = null)
            where TModel : new()
        {
            this.Execute(HttpMethod.Get, resource, parameters, callback);
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="resource">
        /// The resource.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Post<TModel>(string resource, object parameters = null, Action<TModel> callback = null)
            where TModel : new()
        {
            this.Execute(HttpMethod.Post, resource, parameters, callback);
        }

        /// <summary>
        /// The put.
        /// </summary>
        /// <param name="resource">
        /// The resource.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Put<TModel>(string resource, object parameters = null, Action<TModel> callback = null)
            where TModel : new()
        {
            this.Execute(HttpMethod.Put, resource, parameters, callback);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="resource">
        /// The resource.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Delete<TModel>(string resource, object parameters = null, Action<TModel> callback = null)
            where TModel : new()
        {
            this.Execute(HttpMethod.Delete, resource, parameters, callback);
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="resource">
        /// The resource.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Execute<TModel>(HttpMethod method, string resource, object parameters = null, Action<TModel> callback = null, Action<HttpError> error = null)
            where TModel : new()
        {
            var message = new HttpRequestMessage();
            message.Method = method;
            
            this.Execute(message, resource, parameters, callback, error);
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="restResource">
        /// The rest resource.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="error">
        /// The error callback.
        /// </param>
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public async void Execute<TModel>(HttpRequestMessage message, string restResource, object parameters = null, Action<TModel> callback = null, Action<HttpError> error = null)
            where TModel : new()
        {
            var client = new HttpClient();
            
            message.Headers.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            message.RequestUri = new Uri(new Uri(this.baseUrl), restResource);

            if (parameters != null)
            {
                if (parameters is IHttpContent)
                {
                    message.Content = parameters as IHttpContent;
                }
                else
                {
                    message.Content = new HttpStringContent(
                        JsonConvert.SerializeObject(parameters),
                        UnicodeEncoding.Utf8,
                        "application/json");
                }
            }

            if (!string.IsNullOrEmpty(this.bearerToken))
            {
                message.Headers.Add("Authorization", string.Format("Bearer {0}", this.bearerToken));
            }

            client.SendRequestAsync(message).Completed = (info, status) =>
                {
                    try
                    {
                        if (status == AsyncStatus.Canceled)
                        {
                            return;
                        }

                        if (status == AsyncStatus.Error)
                        {
                            return;
                        }

                        var response = info.GetResults();

                        if (response == null || response.Content == null)
                        {
                            return;
                        }

                        var content = response.Content.ReadAsStringAsync();

                        content.Completed += (asyncInfo, asyncStatus) =>
                            {
                                if (status == AsyncStatus.Canceled)
                                {
                                    return;
                                }

                                if (status == AsyncStatus.Error)
                                {
                                    return;
                                }

                                var results = asyncInfo.GetResults();


                                if (!response.IsSuccessStatusCode)
                                {
                                    if (error != null)
                                    {
                                        try
                                        {
                                            var model = JsonConvert.DeserializeObject<HttpError>(results);
                                            error(model);
                                        }
                                        catch (Exception)
                                        {
                                           error(new HttpError { ErrorDescription = "Whoah!! There was an error on the server. We'll take care of that."});
                                        }
                                        
                                    }

                                    return;
                                }

                                if (callback != null)
                                {
                                    var model = JsonConvert.DeserializeObject<TModel>(results);
                                    callback(model);
                                }
                            };
                    }
                    catch (Exception ex)
                    {
                    }
                };
        }
    }
}
