using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using FuelSDK;
using System.Threading.Tasks;

namespace WSAutomation.Class
{
    public class ClassEmail
    {
        private string requestTokenUrl = "https://auth.exacttargetapis.com/v1/requestToken";
        private string EmailInjectUrl = "https://www.exacttargetapis.com/interaction/v1/events";
        private string exactTargetClientId = "5km7yq4alnae44ri6n0vpjlq";
        private string exactTargetClientSecret = "5MXPLHlbcpw4bzZOqshKBC8p";

        class Authentoken
        {
            public string accessToken { get; set; }
            public int expiresIn { get; set; }
        }
        public class DataJSONDetail
        {
            [JsonProperty("firstname")]
            public string firstname { get; set; }
            [JsonProperty("lastname")]
            public string lastname { get; set; }
            [JsonProperty("phone")]
            public string phone { get; set; }
            [JsonProperty("email")]
            public string email { get; set; }
            [JsonProperty("dealer_name")]
            public string dealer_name { get; set; }
            [JsonProperty("branch_name")]
            public string branch_name { get; set; }
            [JsonProperty("model")]
            public string model { get; set; }
        }

        public class DataJSON
        {
            [JsonProperty("ContactKey")]
            public string ContactKey { get; set; }
            [JsonProperty("EventDefinitionKey")]
            public string EventDefinitionKey { get; set; }
            [JsonProperty("EstablishContactKey")]
            public bool EstablishContactKey { get; set; }
            [JsonProperty("Data")]
            public DataJSONDetail Data { get; set; }
        }

        public string ThankyouEmail2(string JBkey, string email, string phone, string firstname, string lastname, string dealer_name, string branch_name, string model)
        {

            string msgs = "";
            try
            {
                var values = new Dictionary<string, string>
            {
              {"clientId", exactTargetClientId},
              {"clientSecret", exactTargetClientSecret}
            };
                Authentoken Authentoken = new Authentoken();
                using (var client = new HttpClient())
                {
                    try
                    {
                        // request exacttarget a token access. 1
                        var content = new FormUrlEncodedContent(values);
                        var response = client.PostAsync(requestTokenUrl, content).Result;
                        // done 1
                        msgs = msgs + "Get Token";
                        if (response.IsSuccessStatusCode)
                        {
                            Task<string> responseString = response.Content.ReadAsStringAsync();
                            string outputJson = responseString.Result;
                            var res = JsonConvert.DeserializeObject<Authentoken>(outputJson);

                            if (res.accessToken != "")
                            {
                                msgs = msgs + ">have Token";
                                HttpClient clientmsg = new HttpClient();

                                {
                                    var payloadsendDetail = new DataJSONDetail
                                    {
                                        firstname = firstname,
                                        lastname = lastname,
                                        phone = phone,
                                        email = email,
                                        dealer_name = dealer_name,
                                        branch_name = branch_name,
                                        model = model,
                                    };

                                    var payloadsend = new DataJSON
                                    {
                                        ContactKey = email,
                                        EventDefinitionKey = JBkey,
                                        EstablishContactKey = true,
                                        Data = payloadsendDetail,
                                    };

                                    msgs = msgs + ">set json payload";
                                    // Inject to ET by using payloadsend. 3
                                    HttpClient clientsend = new HttpClient();
                                    var stringPayloadsend = Task.Run(() => JsonConvert.SerializeObject(payloadsend)).Result;
                                    clientsend.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", res.accessToken);
                                    var httpContentsend = new StringContent(stringPayloadsend, Encoding.UTF8, "application/json");
                                    var responsesend = clientsend.PostAsync(EmailInjectUrl, httpContentsend).Result;
                                    //done 3
                                    msgs = msgs + ">send msg";
                                    if (responsesend.IsSuccessStatusCode)
                                    {
                                        return "Success";
                                    }
                                    else
                                    {
                                        Task<string> responseStringsend = responsesend.Content.ReadAsStringAsync();
                                        string outputJsonsend = responseStringsend.Result;
                                        return outputJsonsend;
                                    }
                                }
                            }
                            //Access Token = null
                            else
                            {
                                return "Access Token is null";
                            }
                        }
                        // Error getting access token
                        else
                        {
                            return "Error getting access token";
                        }
                    }
                    catch (Exception ex)
                    {
                        msgs = msgs + "," + ex.Message;
                        return "Connection Failed plese check your Internet Connection";
                        //Debug.WriteLine(ex.ToString()); //"Invalid URI: The Uri string is too long."
                    }
                }
            }
            catch (Exception ex)
            {
                msgs = msgs + "," + ex.Message;
                return "ClientID error";
            }
        }
    }
}