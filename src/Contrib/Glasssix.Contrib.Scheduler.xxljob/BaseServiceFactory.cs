using Glasssix.Contrib.Scheduler.xxljob.Abstaractions;
using Glasssix.Contrib.Scheduler.xxljob.Extensions.Dto;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Scheduler.xxljob
{
    public class BaseServiceFactory : IBaseServiceFactory
    {
        private readonly ILogger<BaseServiceFactory> _logger;
        private RestClient _restClient;

        public BaseServiceFactory(BuilderOption config, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BaseServiceFactory>();
            _config = config;
            if (cookie == null)
            {
                _restClient = new RestClient(config.AdminAddresses);
                Login();
            }
        }

        private BuilderOption _config { get; set; }
        private Cookie cookie { get; set; }

        public async Task<Result<TaskOption>> GetTaskPageList(TaskQueyInput input)
        {
            var entity = new TaskOption();
            if (input.jobGroup == 0 || string.IsNullOrEmpty(input.executorHandler))
                return new Result<TaskOption>(StateCode.Fail, "请输入工作组Id或执行器Handler!");
            RestRequest restRequest = new RestRequest(_config.ApiUrl + "pageList", Method.Post);
            var pairs = input.ConvertToDeviceParameterJson();
            restRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
            restRequest.AddParameter("jobGroup", _config.JobGroup);
            foreach (var item in pairs)
            {
                restRequest.AddParameter(item.Key, item.Value);
            }

            restRequest.AddCookie(cookie.Name, cookie.Value, "", "");
            var result = (await _restClient.ExecuteAsync(restRequest))?.Content;
            if (!string.IsNullOrWhiteSpace(result))
                entity = JsonConvert.DeserializeObject<TaskOption>(result);

            return new Result<TaskOption>(StateCode.Ok, "成功!", entity);
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="url"></param>
        public void Invoke(string taskId, TaskStateType type)
        {
            try
            {
                var url = string.Empty;
                switch (type)
                {
                    case TaskStateType.Start:
                        url = _config.ApiUrl + "start";
                        break;

                    case TaskStateType.Stop:
                        url = _config.ApiUrl + "stop";
                        break;

                    case TaskStateType.Remove:
                        url = _config.ApiUrl + "remove";
                        break;

                    case TaskStateType.trigger:
                        url = _config.ApiUrl + "trigger";
                        break;
                }

                RestRequest restRequest = new RestRequest(url, Method.Post);
                restRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
                restRequest.AddParameter("id", taskId);
                restRequest.AddCookie(cookie.Name, cookie.Value, "", "");
                var result = _restClient.Execute(restRequest)?.Content;
            }
            catch (Exception e)
            {
                _logger.LogError($"Job Invoke Error! TaskId:{taskId} {e.Message}");
            }
        }

        public void Login()
        {
            RestRequest restRequest = new RestRequest("login", Method.Post);
            restRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
            restRequest.AddParameter("userName", _config.UserName);
            restRequest.AddParameter("password", _config.Password);
            var result = _restClient.Execute(restRequest);
            cookie = result.Cookies[0];
        }

        public async ValueTask<string> TryAdd(VisitorXxlJobOption option)
        {
            string taskId = string.Empty;
            try
            {
                var pairs = option.ConvertToDeviceParameterJson();

                RestRequest restRequest = new RestRequest(_config.ApiUrl + "add", Method.Post);
                restRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
                restRequest.AddParameter("jobGroup", _config.JobGroup);
                foreach (var item in pairs)
                {
                    restRequest.AddParameter(item.Key, item.Value);
                }
                restRequest.AddCookie(cookie.Name, cookie.Value, "", "");
                var result = (await _restClient.ExecuteAsync(restRequest))?.Content;
                if (!string.IsNullOrWhiteSpace(result) && JObject.Parse(result)["code"].ToString() == "200")
                    taskId = JObject.Parse(result)["content"].ToString();

                if (!string.IsNullOrWhiteSpace(taskId))
                    Invoke(taskId, TaskStateType.Start);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return taskId;
        }

        public async ValueTask<xxlJobResult> TryUpdate(VisitorXxlJobOption option)
        {
            var entity = new xxlJobResult();
            try
            {
                var pairs = option.ConvertToDeviceParameterJson();
                RestRequest restRequest = new RestRequest(_config.ApiUrl + "update", Method.Post);
                restRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
                restRequest.AddParameter("jobGroup", _config.JobGroup);
                foreach (var item in pairs)
                    restRequest.AddParameter(item.Key, item.Value);
                restRequest.AddCookie(cookie.Name, cookie.Value, "", "");
                var result = (await _restClient.ExecuteAsync(restRequest))?.Content;
                if (!string.IsNullOrWhiteSpace(result))
                    entity = JsonConvert.DeserializeObject<xxlJobResult>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return entity;
        }
    }
}