using Newtonsoft.Json;

namespace Glasssix.Contrib.Scheduler.xxljob.Extensions.Dto
{
    public class BuilderOption
    {
        public string AdminAddresses { get; set; }

        public string ApiUrl { get; set; }

        [JsonProperty("_job_group")]
        public string JobGroup { get; set; }

        [JsonProperty("_password")]
        public string Password { get; set; }

        [JsonProperty("_userName")]
        public string UserName { get; set; }
    }
}