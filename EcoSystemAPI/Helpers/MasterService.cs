//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json.Linq;
//using System.Net.Http.Headers;
//using System.Net.Http.Json;

//namespace cardapi.Services
//{

//    public class MasterService
//    {
//        protected readonly IConfiguration configuration;
//        public string APIURL = String.Empty;
//        public string APIURLCallNPay = String.Empty;
//        public object returnobj;
//        public CustomResponse2 failedResponse;


//        public MasterService(IConfiguration iconfig)
//        {

//            configuration = iconfig;
//            APIURL = configuration["AppService:EmailService"];
//            APIURLCallNPay = configuration["AppService:CallNPayAPI"];

//        }
//        public async Task<string> PostRequest(string token, Object postdata, string endpoint, string service)
//        {

//            using (var client = new HttpClient())
//            {
//                if(service == "email")
//                {
//                    client.BaseAddress = new Uri(APIURL);
//                }
//                else if(service == "callnpay")
//                {
//                    client.BaseAddress = new Uri(APIURLCallNPay);
//                }
               
//                client.DefaultRequestHeaders.Accept.Clear();

//                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//                if(token != null)
//                {
//                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//                }
                

//                var response = await client.PostAsJsonAsync(endpoint, postdata);


//                if (response.IsSuccessStatusCode)
//                {

//                    string readTask = await response.Content.ReadAsStringAsync();

//                    //   CustomResponse2 xres = JsonConvert.DeserializeObject<CustomResponse2>(readTask);


//                    return readTask;

//                }
//                else
//                {
//                    string other = await response.Content.ReadAsStringAsync();

//                    // CustomResponse2 xres = JsonConvert.DeserializeObject<CustomResponse2>(other);


//                    return other;
//                }

//            }

//            //return null;
//        }

//        public async Task<string> PutRequest(string token, Object data, string endpoint)
//        {

//            using (var client = new HttpClient())
//            {
//                client.BaseAddress = new Uri(APIURL);
//                client.DefaultRequestHeaders.Accept.Clear();

//                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

//                var response = await client.PutAsJsonAsync(endpoint, data);


//                if (response.IsSuccessStatusCode)
//                {

//                    string readTask = await response.Content.ReadAsStringAsync();

//                    //   CustomResponse2 xres = JsonConvert.DeserializeObject<CustomResponse2>(readTask);


//                    return readTask;

//                }
//                else
//                {
//                    string other = await response.Content.ReadAsStringAsync();

//                    // CustomResponse2 xres = JsonConvert.DeserializeObject<CustomResponse2>(other);


//                    return other;
//                }

//            }

//            //return null;
//        }

//        public async Task<string> GetRequest(string _token, string endpoint, string service)
//        {

//            using (var client = new HttpClient())
//            {
//                if (service == "email")
//                {
//                    client.BaseAddress = new Uri(APIURL);
//                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
//                }
//                else if (service == "callnpay")
//                {
//                    client.BaseAddress = new Uri(APIURLCallNPay);
//                    client.DefaultRequestHeaders.Add("Api-Key", _token);
//                }

//               // client.BaseAddress = new Uri(APIURL);

//                client.DefaultRequestHeaders.Accept.Clear();

//                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               
              
//                var response = await client.GetAsync(endpoint);


//                if (response.IsSuccessStatusCode)
//                {

//                    string readTask = await response.Content.ReadAsStringAsync();

//                    return readTask;

//                }
//                else
//                {
//                    string readTask = await response.Content.ReadAsStringAsync();

//                    return readTask;
//                }

//            }

//            return null;
//        }

//        public List<CommonSer> GetRequest2(string _token, string endpoint)
//        {

//            using (var client = new HttpClient())
//            {
//                client.BaseAddress = new Uri(APIURL);
//                client.DefaultRequestHeaders.Accept.Clear();

//                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
//                var response = client.GetAsync(endpoint);
//                response.Wait();

//                if (response.IsCompleted)
//                {

//                    var result = response.Result;
//                    if (result.IsSuccessStatusCode)
//                    {

//                        var readTask = result.Content.ReadFromJsonAsync<CustomResponse3>();
//                        readTask.Wait();



//                        var rawResponse = readTask.Result;




//                        if (rawResponse.status == 200)
//                        {

//                            // string det = rawResponse.result;

//                            return rawResponse.result;
//                        }


//                    }
//                    else
//                    {
//                        var readTask = result.Content.ReadFromJsonAsync<CustomResponse2>();
//                        readTask.Wait();
//                        failedResponse = readTask.Result;
//                    }

//                }

//            }

//            return null;
//        }
//    }

  
//    public class CommonSer
//    {
//        public string key { get; set; }
//        public int value { get; set; }
//    }

//    public class CustomResponse3
//    {
//        public int status { get; set; }
//        public string message { get; set; }
//        public List<CommonSer> result { get; set; }
//        public object error { get; set; }
//    }
//}
