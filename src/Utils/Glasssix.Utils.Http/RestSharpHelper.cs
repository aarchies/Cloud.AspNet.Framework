using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;

namespace Glasssix.Utils.Http
{
    public static class RestSharpHelper
    {
        public static RestResponse BaseRequest(string url, RestRequest request, HttpBasicAuthenticator httpBasicAuthenticator = null)
        {
            RestClientOptions options = new RestClientOptions(url);
            if (httpBasicAuthenticator != null)
                options.Authenticator = httpBasicAuthenticator;
            options.MaxTimeout = 6000;
            options.RemoteCertificateValidationCallback =
            (sender, certificate, chain, sslPolicyErrors) => true;

            var client = new RestClient(options);
            RestResponse response = client.Execute(request);
            return response;
        }

        public static RestResponse Delete(string url, string useranme, string password, object obj, Dictionary<string, string> head_dic = null, int timeout = 6000)
        {
            RestRequest request = CreateDeleteRestRequest(obj, head_dic, timeout);
            HttpBasicAuthenticator auth = new HttpBasicAuthenticator(useranme, password);
            return BaseRequest(url, request, httpBasicAuthenticator: auth);
        }

        public static RestResponse Get(string url, Dictionary<string, string> head_dic = null, Dictionary<string, object> parameter_dic = null, int timeout = 6000)
        {
            RestRequest request = CreateGetRestRequest(head_dic, parameter_dic, timeout);
            return BaseRequest(url, request);
        }

        public static RestResponse Get(string url, string useranme, string password, Dictionary<string, string> head_dic = null, Dictionary<string, object> parameter_dic = null, int timeout = 6000)
        {
            RestRequest request = CreateGetRestRequest(head_dic, parameter_dic, timeout);
            HttpBasicAuthenticator auth = new HttpBasicAuthenticator(useranme, password);
            return BaseRequest(url, request, httpBasicAuthenticator: auth);
        }

        public static RestResponse Post(string url, object obj, Dictionary<string, string> head_dic = null, int timeout = 60000)
        {
            RestRequest request = CreatePostRestRequest(obj, head_dic, timeout);
            return BaseRequest(url, request);
        }

        public static RestResponse Post(string url, string useranme, string password, object obj, Dictionary<string, string> head_dic = null, int timeout = 6000)
        {
            RestRequest request = CreatePostRestRequest(obj, head_dic, timeout);
            HttpBasicAuthenticator auth = new HttpBasicAuthenticator(useranme, password);
            return BaseRequest(url, request, httpBasicAuthenticator: auth);
        }

        public static RestResponse PostFormBody(string url, object obj, Dictionary<string, string> head_dic = null, int timeout = 60000)
        {
            RestRequest request = CreatePostFormBodyRestRequest(obj, head_dic, timeout);
            return BaseRequest(url, request);
        }

        private static RestRequest CreateDeleteRestRequest(object obj, Dictionary<string, string> head_dic = null, int timeout = 6000)
        {
            RestRequest request = new RestRequest("", Method.Delete);
            request.RequestFormat = DataFormat.Json;
            request.Timeout = timeout;
            if (head_dic != null)
            {
                foreach (var item in head_dic)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }
            if (obj != null)
                request.AddJsonBody(obj);
            return request;
        }

        private static RestRequest CreateGetRestRequest(Dictionary<string, string> head_dic = null, Dictionary<string, object> parameter_dic = null, int timeout = 6000)
        {
            RestRequest request = new RestRequest("", Method.Get);
            request.Timeout = timeout;
            if (head_dic != null)
            {
                foreach (var item in head_dic)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }
            if (parameter_dic != null && parameter_dic.Count > 0)
            {
                foreach (var item in parameter_dic)
                {
                    request.AddParameter(item.Key, item.Value.ToString());
                }
            }
            return request;
        }

        private static RestRequest CreatePostFormBodyRestRequest(object obj, Dictionary<string, string> head_dic = null, int timeout = 6000)
        {
            RestRequest request = new RestRequest("", Method.Post);
            request.RequestFormat = DataFormat.Json;
            request.Timeout = timeout;
            if (head_dic != null)
            {
                foreach (var item in head_dic)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }
            if (obj != null)
                request.AddObject(obj);
            return request;
        }

        private static RestRequest CreatePostRestRequest(object obj, Dictionary<string, string> head_dic = null, int timeout = 6000)
        {
            RestRequest request = new RestRequest("", Method.Post);
            request.RequestFormat = DataFormat.Json;
            request.Timeout = timeout;
            if (head_dic != null)
            {
                foreach (var item in head_dic)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }
            if (obj != null)
                request.AddJsonBody(obj);
            return request;
        }
    }
}