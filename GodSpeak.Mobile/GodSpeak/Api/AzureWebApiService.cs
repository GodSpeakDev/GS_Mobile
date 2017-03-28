using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using MvvmCross.Platform.Platform;
using System.Text;

namespace GodSpeak.Api
{
    public class AzureWebApiService : FakeWebApiService, IWebApiService
    {
        private const string LoginMethodUri = "user/login";
        protected HttpClient client = new HttpClient ();
        readonly IMvxTrace tracer;

        public AzureWebApiService (IMvxTrace tracer)
        {
            this.tracer = tracer;
            client.BaseAddress = new Uri ("http://godspeak-staging.azurewebsites.net/api/");

        }


        public new async Task<ApiResponse<LoginResponse>> Login (LoginRequest request)
        {

            return await DoPost<LoginResponse> (LoginMethodUri, request);
        }

        protected async Task<ApiResponse<T>> DoPost<T> (string uri, object request)
        {
            var jsonBody = JsonConvert.SerializeObject (request);


            tracer.Trace (MvxTraceLevel.Diagnostic, "api-post", $"METHOD: {uri}\rBODY: {jsonBody}");

            var apiResponse = await client.PostAsync (uri, new StringContent (jsonBody, Encoding.UTF8, "application/json"));
            var json = await apiResponse.Content.ReadAsStringAsync ();
            var parsedResponse = JsonConvert.DeserializeObject<ApiResponse<T>> (json);
            parsedResponse.StatusCode = apiResponse.StatusCode;
            LogResponse (LoginMethodUri, parsedResponse, json);
            return parsedResponse;
        }

        void LogResponse<T> (string method, ApiResponse<T> parsedResponse, string json)
        {

            tracer.Trace (parsedResponse.StatusCode == System.Net.HttpStatusCode.OK ? MvxTraceLevel.Diagnostic : MvxTraceLevel.Error, "api-response", $"METHOD: {method}\rSTATUS CODE: {parsedResponse.StatusCode}\rTITLE:{parsedResponse.Title}\rMESSAGE:{parsedResponse.Message}\rPAYLOAD:{JsonConvert.SerializeObject (parsedResponse.Payload)}");



        }


    }
}
