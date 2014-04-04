namespace Linkslap.WP.Communication
{
    using System;
    using System.Net;

    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    using RestSharp;

    /// <summary>
    /// The rest.
    /// </summary>
    public class Rest
    {
        /// <summary>
        /// The resource.
        /// </summary>
        private readonly string resource;

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
        /// <param name="resource">
        /// The resource.
        /// </param>
        public Rest(string resource = null)
        {
            this.resource = resource;
            this.baseUrl = AppSettings.BaseUrl;

            var account = Storage.Load<Account>("account");

            if (account != null)
            {
                this.bearerToken = account.BearerToken;
            }
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <typeparam name="TParams">
        /// The parameters passed to the request.
        /// </typeparam>
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Get<TParams, TModel>(TParams parameters = null, Action<TModel> callback = null)
            where TModel : new()
            where TParams : class, new()
        {
            this.Execute(Method.GET, parameters, callback);
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <typeparam name="TParams">
        /// The parameters passed to the request.
        /// </typeparam>
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Post<TParams, TModel>(TParams parameters = null, Action<TModel> callback = null)
            where TModel : new()
            where TParams : class, new()
        {
            this.Execute(Method.POST, parameters, callback);
        }

        /// <summary>
        /// The put.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <typeparam name="TParams">
        /// The parameters passed to the request.
        /// </typeparam>
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Put<TParams, TModel>(TParams parameters = null, Action<TModel> callback = null)
            where TModel : new()
            where TParams : class, new()
        {
            this.Execute(Method.PUT, parameters, callback);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <typeparam name="TParams">
        /// The parameters passed to the request.
        /// </typeparam>
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Delete<TParams, TModel>(TParams parameters = null, Action<TModel> callback = null)
            where TModel : new()
            where TParams : class, new()
        {
            this.Execute(Method.DELETE, parameters, callback);
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <typeparam name="TParams">
        /// The parameters passed to the request.
        /// </typeparam>
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Execute<TParams, TModel>(Method method, TParams parameters = null, Action<TModel> callback = null)
            where TModel : new() where TParams : class, new()
        {
            var request = new RestRequest(this.resource);
            request.AddObject(parameters);
            request.Method = method;

            this.Execute(request, callback);
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="request">
        /// The request.
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
        public void Execute<TModel>(RestRequest request, Action<TModel> callback = null, Action error = null)
            where TModel : new()
        {
            var client = new RestClient { BaseUrl = this.baseUrl };

            if (!string.IsNullOrEmpty(this.bearerToken))
            {
                request.AddHeader("Authorization", string.Format("Bearer {0}", this.bearerToken));
            }

            client.ExecuteAsync(
                request,
                (IRestResponse<TModel> response) =>
                    {
                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            if (error != null)
                            {
                                error();
                            }
                        }
                        else if (callback != null)
                        {
                            callback(response.Data);
                        }
                    });
        }
    }
}
