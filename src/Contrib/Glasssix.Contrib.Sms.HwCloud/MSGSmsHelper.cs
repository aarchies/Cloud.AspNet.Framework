//using System.Collections.Specialized;
//using System.Net;
//using System.Text;
//using System.Web;

//namespace Glasssix.Contrib.Sms.HwCloud
//{
//    public static class MSGSmsHelper
//    {
//        static void Main(string[] args)
//        {
//            //必填,请参考"开发准备"获取如下数据,替换为实际值
//            string apiAddress = "https://smsapi.cn-north-4.myhuaweicloud.com:443/sms/batchSendSms/v1"; //APP接入地址(在控制台"应用管理"页面获取)+接口访问URI
//            string appKey = "c8RWg3ggEcyd4D3p94bf3Y7x1Ile"; //APP_Key
//            string appSecret = "q4Ii87BhST9vcs8wvrzN80SfD7Al"; //APP_Secret
//            string sender = "csms12345678"; //国内短信签名通道号或国际/港澳台短信通道号
//            string templateId = "8ff55eac1d0b478ab3c06c3c6a492300"; //模板ID

//            //条件必填,国内短信关注,当templateId指定的模板类型为通用模板时生效且必填,必须是已审核通过的,与模板类型一致的签名名称
//            //国际/港澳台短信不用关注该参数
//            //string signature = "华为云短信测试"; //签名名称

//            //必填,全局号码格式(包含国家码),示例:+86151****6789,多个号码之间用英文逗号分隔
//            string receiver = "+86151****6789,+86152****7890"; //短信接收人号码

//            //选填,短信状态报告接收地址,推荐使用域名,为空或者不填表示不接收状态报告
//            string statusCallBack = "";

//            /*
//             * 选填,使用无变量模板时请赋空值 string templateParas = "";
//             * 单变量模板示例:模板内容为"您的验证码是${1}"时,templateParas可填写为"[\"369751\"]"
//             * 双变量模板示例:模板内容为"您有${1}件快递请到${2}领取"时,templateParas可填写为"[\"3\",\"人民公园正门\"]"
//             * 模板中的每个变量都必须赋值，且取值不能为空
//             * 查看更多模板和变量规范:产品介绍>模板和变量规范
//             */
//            string templateParas = "[\"369751\"]"; //模板变量，此处以单变量验证码短信为例，请客户自行生成6位验证码，并定义为字符串类型，以杜绝首位0丢失的问题（例如：002569变成了2569）。

//            try
//            {
//                //为防止因HTTPS证书认证失败造成API调用失败,需要先忽略证书信任问题
//                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
//                //使用Tls1.2 = 3072
//                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | (SecurityProtocolType)3072;

//                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(apiAddress);
//                //请求方法
//                myReq.Method = "POST";
//                //请求Headers
//                myReq.ContentType = "application/x-www-form-urlencoded";
//                myReq.Headers.Add("Authorization", "WSSE realm=\"SDP\",profile=\"UsernameToken\",type=\"Appkey\"");
//                myReq.Headers.Add("X-WSSE", BuildWSSEHeader(appKey, appSecret));
//                //请求Body
//                NameValueCollection keyValues = new NameValueCollection
//                {
//                    {"from", sender},
//                    {"to", receiver},
//                    {"templateId", templateId},
//                    {"templateParas", templateParas},
//                    {"statusCallback", statusCallBack},
//                    //{"signature", signature } //使用国内短信通用模板时,必须填写签名名称
//                };
//                string body = BuildQueryString(keyValues);

//                //发送请求数据
//                StreamWriter req = new StreamWriter(myReq.GetRequestStream());
//                req.Write(body);
//                req.Close();

//                //获取响应数据
//                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
//                StreamReader resp = new StreamReader(myResp.GetResponseStream());
//                string result = resp.ReadToEnd();
//                myResp.Close();
//                resp.Close();

//                Console.WriteLine(result);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.StackTrace);
//                Console.WriteLine(e.Message);
//            }
//        }

//        /// <summary>
//        /// 构造X-WSSE参数值
//        /// </summary>
//        /// <param name="appKey"></param>
//        /// <param name="appSecret"></param>
//        /// <returns></returns>
//        static string BuildWSSEHeader(string appKey, string appSecret)
//        {
//            string now = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"); //Created
//            string nonce = Guid.NewGuid().ToString().Replace("-", ""); //Nonce

//            byte[] material = Encoding.UTF8.GetBytes(nonce + now + appSecret);
//            byte[] hashed = SHA256Managed.Create().ComputeHash(material);
//            string hexdigest = BitConverter.ToString(hashed).Replace("-", "");
//            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(hexdigest)); //PasswordDigest

//            return string.Format("UsernameToken Username=\"{0}\",PasswordDigest=\"{1}\",Nonce=\"{2}\",Created=\"{3}\"",
//                            appKey, base64, nonce, now);
//        }

//        /// <summary>
//        /// 构造请求body
//        /// </summary>
//        /// <param name="keyValues"></param>
//        /// <returns></returns>
//        static string BuildQueryString(NameValueCollection keyValues)
//        {
//            StringBuilder temp = new StringBuilder();
//            foreach (string item in keyValues.Keys)
//            {
//                temp.Append(item).Append("=").Append(HttpUtility.UrlEncode(keyValues.Get(item))).Append("&");
//            }
//            return temp.Remove(temp.Length - 1, 1).ToString();
//        }
//    }
//}