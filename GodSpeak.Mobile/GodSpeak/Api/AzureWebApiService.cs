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

        const string MessagesQueueUri = "message/queue";

        const string PurchaseInviteUri = "invite/purchase";
        const string AcceptedInviteUri = "invite/accepted";
        const string ValidateCodeUri = "invite/validate";
        const string InviteBundlesUri = "invite/bundles";
        const string RequestInviteUri = "invite/request";
        const string DonateInviteUri = "invite/donate";

		private ISettingsService _settingsService;
        protected HttpClient client = new HttpClient ();
        readonly IMvxTrace tracer;

        protected string ServerUrl = "http://godspeak-staging.azurewebsites.net/";

        public AzureWebApiService (IMvxTrace tracer, ISettingsService settingsService)
        {
            this.tracer = tracer;

			_settingsService = settingsService;
            client.BaseAddress = new Uri (ServerUrl + "api/");
        }

        protected List<Message> CachedMessages = new List<Message> ();

        public new async Task<ApiResponse<List<Message>>> GetMessages ()
        {
            AddAuthToken (_settingsService.Token);
            var apiResponse = await DoGet<List<Message>> (MessagesQueueUri);

            CachedMessages = apiResponse.Payload;
            return apiResponse;
        }

        public new async Task<ApiResponse<GetMessageResponse>> GetMessage (GetMessageRequest request)
        {
            var content = new GetMessageResponse () {
                Payload = CachedMessages.First (x => x.Id == request.MessageId)
            };

            return new ApiResponse<GetMessageResponse> () {
                Payload = content,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

		public new async Task<ApiResponse<List<ImpactDay>>> GetImpact(GetImpactRequest request)
		{			
            AddAuthToken(_settingsService.Token);
            return await DoGet<List<ImpactDay>>(ImpactDaysUri, new Dictionary<string, string> () { { "inviteCode", request.InviteCode } });
	   	}

		public new async Task<ApiResponse<string>> RecordMessageDelivered(RecordMessageDeliveredRequest request)
		{
			AddAuthToken(_settingsService.Token);
			return await DoPost<string>(ImpactMessageUri, request);
		}

        public new async Task<ApiResponse<List<Country>>> GetCountries ()
        {
            var countriesList = await DoGet<List<Country>> (CountriesUri);

			var userCountry = countriesList.Payload.FirstOrDefault(x => x.Code == "US");

			countriesList.Payload.Remove(userCountry);
			countriesList.Payload = new List<Country>(countriesList.Payload.OrderBy(x => x.Title));
			countriesList.Payload.Insert(0, userCountry);

			return countriesList;
        }

        public new async Task<ApiResponse<String>> ForgotPassword (ForgotPasswordRequest request)
        {
            return await DoGet<String> (RecoverPasswordUri, new Dictionary<string, string> () { { "emailAddress", request.Email } });
        }

        public new async Task<ApiResponse<User>> RegisterUser (RegisterUserRequest request)
        {
            var response = await DoPost<User> (RegisterUri, request);

			if (response.IsSuccess)
			{
				_settingsService.Token = response.Payload.Token;
			}

			return response;
        }

        public new async Task<ApiResponse<User>> GetProfile ()
        {
            AddAuthToken (_settingsService.Token);
            return await DoGet<User> (ProfileUri);
        }

        public new async Task<ApiResponse<User>> SaveProfile (User user)
        {
            AddAuthToken (_settingsService.Token);
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

        public new async Task<ApiResponse<List<AcceptedInvite>>> GetAcceptedInvites ()
        {
            AddAuthToken (_settingsService.Token);
            return await DoGet<List<AcceptedInvite>> (AcceptedInviteUri);
        }

        public new async Task<ApiResponse<User>> DonateInvite ()
        {
            AddAuthToken (_settingsService.Token);
            return await DoPost<User> (DonateInviteUri, null);
        }

        public new async Task<ApiResponse<string>> PurchaseInvite (PurchaseInviteRequest request)
        {
            AddAuthToken (_settingsService.Token);
            return await DoPost<string> (PurchaseInviteUri, request);
        }

        public new async Task<ApiResponse<User>> Login (LoginRequest request)
        {
            var response = await DoPost<User> (LoginMethodUri, request);

			if (response.IsSuccess)
			{
				_settingsService.Token = response.Payload.Token;
			}

			return response;
        }

        public new async Task<ApiResponse<string>> GetDidYouKnow ()
        {
            AddAuthToken (_settingsService.Token);
            return await DoGet<string> (ImpactDidYouKnowUri);
        }

        public new async Task<ApiResponse<User>> UploadPhoto (UploadPhotoRequest request)
        {
            AddAuthToken (_settingsService.Token);

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

        public new async Task<ApiResponse> Logout ()
        {
            AddAuthToken (_settingsService.Token);
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

            var userResponse = parsedResponse as ApiResponse<User>;
			if (userResponse != null && userResponse.Payload != null && !string.IsNullOrEmpty(userResponse.Payload.PhotoUrl) && !userResponse.Payload.PhotoUrl.Contains(ServerUrl))
			{
				userResponse.Payload.PhotoUrl = ServerUrl + userResponse.Payload.PhotoUrl;
			}

			var acceptedInviteResponse = parsedResponse as ApiResponse<List<AcceptedInvite>>;
			if (acceptedInviteResponse != null && acceptedInviteResponse.Payload != null)
			{
				foreach (var item in acceptedInviteResponse.Payload)
				{
					item.ImageUrl = ServerUrl + item.ImageUrl;
				}
			}

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
