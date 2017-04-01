using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using MvvmCross.Platform.Platform;
using System.Text;
using System.Collections.Generic;
using GodSpeak.Api.Dtos;
using System.Net.Http.Headers;
using System.IO;
using PCLStorage;
using System.Linq;

namespace GodSpeak.Api
{
    public class AzureWebApiService : FakeWebApiService, IWebApiService
    {

        const string LoginMethodUri = "user/login";
        const string RecoverPasswordUri = "user/recoverpassword/";
        const string ProfileUri = "user";
        const string RegisterUri = "user";
        const string PhotoUploadUri = "user/photo";
        const string LogoutUri = "user/logout";

        const string CountriesUri = "geo/countries";
        const string PostalCodeExistsUri = "geo/postalCodeExists";

        const string ImpactDaysUri = "impact/days";
        const string ImpactMessageUri = "impact/message";
        const string ImpactDidYouKnowUri = "impact/didyouknow";

        const string MessagesQueueUri = "messages/queue";

        const string PurchaseInviteUri = "invite/purchase";
        const string AcceptedInviteUri = "invite/accepted";
        const string ValidateCodeUri = "invite/validate";
        const string InviteBundlesUri = "invite/bundles";
        const string RequestInviteUri = "invite/request";
        const string DonateInviteUri = "invite/donate";

        protected HttpClient client = new HttpClient ();
        readonly IMvxTrace tracer;

        public AzureWebApiService (IMvxTrace tracer)
        {
            this.tracer = tracer;
            client.BaseAddress = new Uri ("http://godspeak-staging.azurewebsites.net/api/");
        }

        public new async Task<ApiResponse<List<Country>>> GetCountries ()
        {
            return await DoGet<List<Country>> (CountriesUri);
        }

        public new async Task<ApiResponse<String>> ForgotPassword (ForgotPasswordRequest request)
        {
            return await DoGet<String> (RecoverPasswordUri, new Dictionary<string, string> () { { "emailAddress", request.Email } });
        }

        public new async Task<ApiResponse<User>> RegisterUser (RegisterUserRequest request)
        {
            return await DoPost<User> (RegisterUri, request);
        }

        public new async Task<ApiResponse<User>> GetProfile (TokenRequest request)
        {
            AddAuthToken (request.Token);
            return await DoGet<User> (ProfileUri);
        }

        public new async Task<ApiResponse<User>> SaveProfile (User user)
        {
            AddAuthToken (user.Token);
            return await DoPut<User> (ProfileUri, user);
        }

        public new async Task<ApiResponse<ValidateCodeResponse>> ValidateCode (ValidateCodeRequest request)
        {
            return await DoGet<ValidateCodeResponse> (ValidateCodeUri, new Dictionary<string, string> { { "inviteCode", request.Code } });
        }

        public new async Task<ApiResponse<List<InviteBundle>>> GetInviteBundles (GetInviteBundlesRequest request)
        {
            return await DoGet<List<InviteBundle>> (InviteBundlesUri);
        }

        public new async Task<ApiResponse<List<AcceptedInvite>>> GetAcceptedInvites (TokenRequest request)
        {
            AddAuthToken (request.Token);
            return await DoGet<List<AcceptedInvite>> (AcceptedInviteUri);
        }

        public new async Task<ApiResponse<string>> DonateInvite (TokenRequest request)
        {
            AddAuthToken (request.Token);
            return await DoPost<string> (DonateInviteUri, request);
        }

        public new async Task<ApiResponse<string>> PurchaseInvite (PurchaseInviteRequest request)
        {
            AddAuthToken (request.Token);
            return await DoPost<string> (PurchaseInviteUri, request);
        }

        public new async Task<ApiResponse<User>> Login (LoginRequest request)
        {
            return await DoPost<User> (LoginMethodUri, request);
        }

        public new async Task<ApiResponse<string>> GetDidYouKnow (TokenRequest request)
        {
            AddAuthToken (request.Token);
            return await DoGet<string> (ImpactDidYouKnowUri);
        }

        public new async Task<ApiResponse<User>> UploadPhoto (UploadPhotoRequest request)
        {
            AddAuthToken (request.Token);


            IFile file = await FileSystem.Current.GetFileFromPathAsync (request.FilePath);
            Stream imageStream = await file.OpenAsync (FileAccess.Read);



            MultipartFormDataContent multipartContent =
                new MultipartFormDataContent ();

            multipartContent.Add (
                new StreamContent (imageStream),
                "photo",
                file.Name);




            HttpResponseMessage response = await client.PostAsync (PhotoUploadUri, multipartContent);

            return await ParseResponse<User> (PhotoUploadUri, response);
        }

        private StreamContent CreateFileContent (Stream stream, string fileName, string contentType)
        {
            var fileContent = new StreamContent (stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue ("form-data") {
                Name = "\"files\"",
                FileName = "\"" + fileName + "\""
            }; // the extra quotes are key here
            fileContent.Headers.ContentType = new MediaTypeHeaderValue (contentType);
            return fileContent;
        }

        public new async Task<ApiResponse> Logout (LogoutRequest request)
        {
            AddAuthToken (request.Token);
            return await DoPost (LogoutUri);
        }

        protected async Task<ApiResponse> DoPost (string uri)
        {
            var apiResponse = await client.PostAsync (uri, null);

            return await ParseResponse (uri, apiResponse);
        }

        protected async Task<ApiResponse<T>> DoPost<T> (string uri, object request)
        {
            var jsonBody = JsonConvert.SerializeObject (request);


            tracer.Trace (MvxTraceLevel.Diagnostic, "api-post", $"METHOD: {uri}\rBODY: {jsonBody}");

            var apiResponse = await client.PostAsync (uri, new StringContent (jsonBody, Encoding.UTF8, "application/json"));

            return await ParseResponse<T> (uri, apiResponse);
        }

        protected async Task<ApiResponse<T>> DoPut<T> (string uri, object request)
        {
            var jsonBody = JsonConvert.SerializeObject (request);


            tracer.Trace (MvxTraceLevel.Diagnostic, "api-post", $"METHOD: {uri}\rBODY: {jsonBody}");

            var apiResponse = await client.PutAsync (uri, new StringContent (jsonBody, Encoding.UTF8, "application/json"));

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
                builder.Append ($"{pair.Key}={System.Net.WebUtility.UrlDecode (pair.Value)}");
            };

            var requestUri = uri + builder;
            tracer.Trace (MvxTraceLevel.Diagnostic, "api-get", $"METHOD: {requestUri}");
            var apiResponse = await client.GetAsync (requestUri);

            return await ParseResponse<T> (uri, apiResponse);

        }

        protected async Task<ApiResponse> ParseResponse (string uri, HttpResponseMessage apiResponse)
        {
            var json = await apiResponse.Content.ReadAsStringAsync ();
            var parsedResponse = JsonConvert.DeserializeObject<ApiResponse> (json);
            LogResponse (uri, parsedResponse, json);
            return parsedResponse;
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

        void LogResponse (string method, ApiResponse parsedResponse, string json)
        {
            tracer.Trace (parsedResponse.StatusCode == System.Net.HttpStatusCode.OK ? MvxTraceLevel.Diagnostic : MvxTraceLevel.Error, "api-response", $"METHOD: {method}\rSTATUS CODE: {parsedResponse.StatusCode}\rTITLE:{parsedResponse.Title}\rMESSAGE:{parsedResponse.Message}");
        }
    }
}
