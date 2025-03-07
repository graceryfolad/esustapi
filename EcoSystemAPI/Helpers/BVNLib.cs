using System.Text.Json;
using System.Text;
using Newtonsoft.Json;

namespace EcoSystemAPI.Helpers
{
    public class BVNLib
    {
        private readonly HttpClient _httpClient;
        public string token =string.Empty;

        public BVNLib()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetTokenAsync()
        {
            try
            {
                var requestBody = new { clientId = "NWUH751DGHTSAEVI91RC", secret = "f1b2603ca33f46e2b9342fa0f7c5d215" };
                var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://api.qoreid.com/token", content);
                response.EnsureSuccessStatusCode();
                 
                string resp =     await response.Content.ReadAsStringAsync();
                BVNTokenResponse xres = JsonConvert.DeserializeObject<BVNTokenResponse>(resp);

                return xres.accessToken;

            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<Bvn> VerifyBVNAsync(string bvn, string firstName, string lastName)
        {
            try
            {
                var url = $"https://api.qoreid.com/v1/ng/identities/bvn-premium/{bvn}";
                var requestBody = new { firstname = firstName, lastname = lastName };
                var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this.token);
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string verifyresp =  await response.Content.ReadAsStringAsync();

                BVNVerifyResponse xres = JsonConvert.DeserializeObject<BVNVerifyResponse>(verifyresp);

                return xres.bvn;
            }
            catch (Exception ex)
            {
                return null ;
            }
        }
    }


    public class BVNTokenResponse
    {
        public string accessToken { get; set; }
        public int expiresIn { get; set; }
        public string tokenType { get; set; }
    }




    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Applicant
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string paymentMethodCode { get; set; }
    }

    public class Bvn
    {
        public string bvn { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string birthdate { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public string photo { get; set; }
        public string lga_of_origin { get; set; }
        public string lga_of_residence { get; set; }
        public string marital_status { get; set; }
        public string nationality { get; set; }
        public string residential_address { get; set; }
        public string state_of_origin { get; set; }
        public string state_of_residence { get; set; }
        public string email { get; set; }
        public string enrollment_bank { get; set; }
        public string enrollment_branch { get; set; }
        public string name_on_card { get; set; }
        public string nin { get; set; }
        public string level_of_account { get; set; }
        public string phone2 { get; set; }
        public string registration_date { get; set; }
        public string watch_listed { get; set; }
    }

    public class BvnCheck
    {
        public string status { get; set; }
        public FieldMatches fieldMatches { get; set; }
    }

    public class FieldMatches
    {
        public bool firstname { get; set; }
        public bool lastname { get; set; }
    }

    public class BVNVerifyResponse
    {
        public int id { get; set; }
        public Applicant applicant { get; set; }
        public Summary summary { get; set; }
        public Status status { get; set; }
        public Bvn bvn { get; set; }
    }

    public class Status
    {
        public string state { get; set; }
        public string status { get; set; }
    }

    public class Summary
    {
        public BvnCheck bvn_check { get; set; }
    }


}
