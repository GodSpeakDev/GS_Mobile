using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using MvvmCross.Platform.Platform;
using System.Text;
using System.Collections.Generic;

namespace GodSpeak.Api
{
    public class AzureWebApiService : FakeWebApiService, IWebApiService
    {
        const string ValidateCodeUri = "invite/validate";
        const string LoginMethodUri = "user/login";
        const string InviteBundlesUri = "invite/bundles";
        const string ProfileUri = "user";
        const string PurchaseInviteUri = "invite/purchase";

        protected HttpClient client = new HttpClient ();
        readonly IMvxTrace tracer;

        public AzureWebApiService (IMvxTrace tracer)
        {
            this.tracer = tracer;
            client.BaseAddress = new Uri ("http://godspeak-staging.azurewebsites.net/api/");

        }

        public new async Task<ApiResponse<GetCategoriesResponse>> GetCategories (GetCategoriesRequest request)
        {
            AddAuthToken (request.Token);
            var profileResponse = await DoGet<UserResponse> (ProfileUri);
            var catResponse = new ApiResponse<GetCategoriesResponse> ();
            catResponse.Message = profileResponse.Message;
            catResponse.Title = profileResponse.Title;
            catResponse.Payload = new GetCategoriesResponse () { Payload = profileResponse.Payload.Payload.MessageCategorySettings };
            catResponse.StatusCode = profileResponse.StatusCode;
            return catResponse;

        }

        public new async Task<ApiResponse<GetMessageConfigResponse>> GetMessageConfig (GetMessageConfigRequest request)
        {
            AddAuthToken (request.Token);
            var profileResponse = await DoGet<UserResponse> (ProfileUri);
            var configResponse = new ApiResponse<GetMessageConfigResponse> ();
            configResponse.Message = profileResponse.Message;
            configResponse.Title = profileResponse.Title;
            configResponse.Payload = new GetMessageConfigResponse () { Payload = profileResponse.Payload.Payload.MessageDayOfWeekSettings };
            configResponse.StatusCode = profileResponse.StatusCode;
            return configResponse;
        }

        public new async Task<ApiResponse<ValidateCodeResponse>> ValidateCode (ValidateCodeRequest request)
        {
            return await DoGet<ValidateCodeResponse> (ValidateCodeUri, new Dictionary<string, string> { { "inviteCode", request.Code } });
        }

        public new async Task<ApiResponse<List<InviteBundle>>> GetInviteBundles (GetInviteBundlesRequest request)
        {
            return await DoGet<List<InviteBundle>> (InviteBundlesUri);
        }

        public new async Task<ApiResponse<PurchaseInviteResponse>> PurchaseInvite (PurchaseInviteRequest request)
        {
            AddAuthToken (request.Token);
            return await DoPost<PurchaseInviteResponse> (PurchaseInviteUri, request);

        }

        public new async Task<ApiResponse<UserResponse>> Login (LoginRequest request)
        {

            return await DoPost<UserResponse> (LoginMethodUri, request);
        }

        protected async Task<ApiResponse<T>> DoPost<T> (string uri, object request)
        {
            var jsonBody = JsonConvert.SerializeObject (request);


            tracer.Trace (MvxTraceLevel.Diagnostic, "api-post", $"METHOD: {uri}\rBODY: {jsonBody}");

            var apiResponse = await client.PostAsync (uri, new StringContent (jsonBody, Encoding.UTF8, "application/json"));

            return await ParseResponse<T> (uri, apiResponse);
        }

        protected async Task<ApiResponse<T>> DoGet<T> (string uri)
        {
            tracer.Trace (MvxTraceLevel.Diagnostic, "api-get", $"METHOD: {uri}");
            var apiResponse = await client.GetAsync (uri);

            return await ParseResponse<T> (uri, apiResponse);
        }

        protected async Task<ApiResponse<T>> DoGet<T> (string uri, Dictionary<string, string> args)
        {

            var builder = new StringBuilder ("?");


            foreach (var pair in args) {
                if (builder.Length != 1)
                    builder.Append ("&");
                builder.Append ($"{pair.Key}={System.Net.WebUtility.UrlDecode (pair.Value)}")
                       ;
            };


            var requestUri = uri + builder;
            tracer.Trace (MvxTraceLevel.Diagnostic, "api-get", $"METHOD: {requestUri}");
            var apiResponse = await client.GetAsync (requestUri);

            return await ParseResponse<T> (uri, apiResponse);

        }

        protected async Task<ApiResponse<T>> ParseResponse<T> (string uri, HttpResponseMessage apiResponse)
        {

            var json = await apiResponse.Content.ReadAsStringAsync ();
            var parsedResponse = JsonConvert.DeserializeObject<ApiResponse<T>> (json);
            parsedResponse.StatusCode = apiResponse.StatusCode;
            LogResponse (uri, parsedResponse, json);
            return parsedResponse;
        }

        void AddAuthToken (string token)
        {
            if (client.DefaultRequestHeaders.Contains ("token"))
                client.DefaultRequestHeaders.Remove ("token");

            client.DefaultRequestHeaders.Add ("token", token);
        }

        void LogResponse<T> (string method, ApiResponse<T> parsedResponse, string json)
        {

            tracer.Trace (parsedResponse.StatusCode == System.Net.HttpStatusCode.OK ? MvxTraceLevel.Diagnostic : MvxTraceLevel.Error, "api-response", $"METHOD: {method}\rSTATUS CODE: {parsedResponse.StatusCode}\rTITLE:{parsedResponse.Title}\rMESSAGE:{parsedResponse.Message}\rPAYLOAD:{JsonConvert.SerializeObject (parsedResponse.Payload)}");



        }


    }
}
