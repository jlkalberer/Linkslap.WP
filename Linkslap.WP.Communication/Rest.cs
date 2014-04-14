namespace Linkslap.WP.Communication
{
    using System;
    using System.Net;

    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    using Newtonsoft.Json;

    using RestSharp;

    using JsonSerializer = RestSharp.Serializers.JsonSerializer;

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
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Get<TModel>(Action<TModel> callback = null)
            where TModel : new()
        {
            this.Execute(Method.GET, null, callback);
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
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Get<TModel>(object parameters = null, Action<TModel> callback = null)
            where TModel : new()
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
        public void Post<TModel>(object parameters = null, Action<TModel> callback = null)
            where TModel : new()
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
        /// <typeparam name="TModel">
        /// The type of model returned from the request.
        /// </typeparam>
        public void Delete<TModel>(object parameters, Action<TModel> callback)
            where TModel : new()
        {
            this.Execute(Method.DELETE, parameters, callback);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public void Delete(object parameters)
        {
            this.Execute(Method.DELETE, parameters, (Action<object>)null);
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
        public void Execute<TModel>(Method method, object parameters = null, Action<TModel> callback = null)
            where TModel : new()
        {
            var request = new RestRequest(this.resource);
            if (parameters != null)
            {
                request.AddObject(parameters);
            }

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
            request.JsonSerializer = new JsonSerializer();
            if (!string.IsNullOrEmpty(this.bearerToken))
            {
                request.AddHeader("Authorization", string.Format("Bearer {0}", this.bearerToken));
            }

            client.ExecuteAsync(
                request,
                response =>
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
                            var model = JsonConvert.DeserializeObject<TModel>(response.Content);
                            callback(model);
                        }
                    });
        }
    }
}
