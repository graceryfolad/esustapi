using CardData.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net.Http.Headers;
using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using QFDataAccess.Helpers;

namespace EcoSystemAPI.Helpers
{
    public class KudaLib
    {
        private string BaseUrl = "https://kuda-openapi.kuda.com/v2.1";
        private string clientaccountnumber = "3002233225";
        private string Sender = "EcoSystem";
        //
        public string token = string.Empty;
        public async Task<object> GetTokenAsync()
        {
            string link = $"{this.BaseUrl}/Account/GetToken";
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("");
            request.AddHeader("Content-Type", "application/json");
            request.Method = Method.Post;
            var requestData = new
            {
                email = "Fasasi.ecosystem@gmail.com",
                apiKey = "NXThMZD4fj7VY83ibSEF"
            };



            request.AddJsonBody(requestData);

            RestResponse response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                // throw new Exception($"Error: {response.StatusCode} - {response.Content}");
            }

            return response.Content;
        }

        public async Task<string> getToken2Async()
        {
            using (HttpClient client2 = new HttpClient())
            {
                // Set the URL
                string link = $"{this.BaseUrl}/Account/GetToken";
                //  string url = "https://kuda-openapi.kuda.com/v2.1/Account/GetToken";

                // Set the JSON payload
                //    string jsonPayload = @"{
                //    ""email"": ""Fasasi.ecosystem@gmail.com"",
                //    ""apikey"": ""NXThMZD4fj7VY83ibSEF""
                //}";

                var requestData = new
                {
                    email = "Fasasi.ecosystem@gmail.com",
                    apiKey = "NXThMZD4fj7VY83ibSEF"
                };

                string jsonPayload = JsonConvert.SerializeObject(requestData);

                // Create the HTTP content
                HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Send the POST request
                HttpResponseMessage response = await client2.PostAsync(link, content);

                // Read the response
                string responseData = await response.Content.ReadAsStringAsync();

                return responseData;
            }
        }


        public async Task<BankResponse> getBankAsync()
        {
            using (HttpClient client2 = new HttpClient())
            {
                string card = GenerateCardHelper.GenerateVoucherCode(4);

                var requestData = new
                {
                    serviceType = "BANK_LIST",
                    requestRef = card
                };
                
                string jsonPayload = JsonConvert.SerializeObject(requestData);


                var request = new HttpRequestMessage(HttpMethod.Post, this.BaseUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json")
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client2.SendAsync(request);
              //  response.EnsureSuccessStatusCode();

                // Create the HTTP content
               // HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Send the POST request
              //  HttpResponseMessage response = await client2.PostAsync(this.BaseUrl, content);

                // Read the response
                string responseData = await response.Content.ReadAsStringAsync();
               BankResponse  xres = JsonConvert.DeserializeObject<BankResponse>(responseData);

                return xres;
            }
        }


        public async Task<NameEquiryResponse> NameEnquiry(NameEnquiry enquiry)
        {
           

            using (HttpClient client2 = new HttpClient())
            {
                string card = GenerateCardHelper.GenerateVoucherCode(4);

                var requestData = new
                {
                    serviceType = "NAME_ENQUIRY",
                    requestRef = card,
                    Data = enquiry
                };

                string jsonPayload = JsonConvert.SerializeObject(enquiry);


                var request = new HttpRequestMessage(HttpMethod.Post, this.BaseUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json")
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client2.SendAsync(request);
               
                string responseData = await response.Content.ReadAsStringAsync();
                KudaResponse<NameEquiryResponse> xres = JsonConvert.DeserializeObject<KudaResponse<NameEquiryResponse>>(responseData);

                var name_eqy = xres.data;

                return name_eqy;
            }
        }

        public async Task<SingleTransferResponse> SingleTransfer(SingleTransferRequest transfer)
        {


            using (HttpClient client2 = new HttpClient())
            {
                string card = GenerateCardHelper.GenerateVoucherCode(4);

                SingleTransferPayload payload = new SingleTransferPayload()
                {
                    beneficiaryBankCode = transfer.beneficiaryBankCode,
                    beneficiaryName = transfer.beneficiaryName,
                    amount = transfer.amount * 100,   
                    ClientAccountNumber = clientaccountnumber,
                    SenderName = Sender,
                    BeneficiaryAccount = transfer.BeneficiaryAccount,
                    narration = transfer.narration,
                    ClientFeeCharge =0

                }
                    ;

                var requestData = new
                {
                    serviceType = "SINGLE_FUND_TRANSFER",
                    requestRef = card,
                    Data = payload,
                };

                string jsonPayload = JsonConvert.SerializeObject(requestData);


                var request = new HttpRequestMessage(HttpMethod.Post, this.BaseUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json")
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client2.SendAsync(request);

                string responseData = await response.Content.ReadAsStringAsync();
                SingleTransferResponse xres = JsonConvert.DeserializeObject<SingleTransferResponse>(responseData);

              

                return xres;
            }
        }


        public async Task<SingleTransferResponse> ConfirmTransfer(NameEnquiry enquiry)
        {


            using (HttpClient client2 = new HttpClient())
            {
                string card = GenerateCardHelper.GenerateVoucherCode(4);

                var requestData = new
                {
                    serviceType = "NAME_ENQUIRY",
                    requestRef = card,
                    Data = enquiry
                };

                string jsonPayload = JsonConvert.SerializeObject(enquiry);


                var request = new HttpRequestMessage(HttpMethod.Post, this.BaseUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json")
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client2.SendAsync(request);

                string responseData = await response.Content.ReadAsStringAsync();
                KudaResponse<SingleTransferResponse> xres = JsonConvert.DeserializeObject<KudaResponse<SingleTransferResponse>>(responseData);

                var name_eqy = xres.data;

                return name_eqy;
            }
        }
    }

    public class KBank
    {
        public string bankName { get; set; }
        public string bankCode { get; set; }
        public string bankAvailability { get; set; }
        public string bankStatus { get; set; }
    }


    public class Data
    {
        public List<KBank> banks { get; set; }
    }

    public class BankResponse
    {
        public string message { get; set; }
        public bool status { get; set; }
        public Data data { get; set; }
    }

    public class NameEnquiry
    {
        public string BeneficiaryAccountNumber { get; set; } 
        public string BeneficiaryBankCode { get; set; }

    }

    public class KudaResponse<T>
    {
        public string message { get; set; }
        public bool status { get; set; }
        public T data { get; set; }
    }


    public class NameEquiryResponse
    {
        public string beneficiaryAccountNumber { get; set; }
        public string beneficiaryName { get; set; }
        public object senderAccountNumber { get; set; }
        public object senderName { get; set; }
        public int beneficiaryCustomerID { get; set; }
        public string beneficiaryBankCode { get; set; }
        public int nameEnquiryID { get; set; }
        public object responseCode { get; set; }
        public int transferCharge { get; set; }
        public string sessionID { get; set; }
    }

    public class SingleTransferRequest
    {
        public string BeneficiaryAccount { get; set; }
        public string beneficiaryBankCode { get; set; }
        public Single amount { get; set; }
       
        public string beneficiaryName { get; set; }
        public string narration { get; set; }
        
    }

    public class SingleTransferPayload : SingleTransferRequest
    {
        
        public string ClientAccountNumber { get; set; }
       
        public int ClientFeeCharge { get; set; }
        public string SenderName { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class SingleTransferResponse
    {
        public string RequestReference { get; set; }
        public string TransactionReference { get; set; }
        public string ResponseCode { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }



}
