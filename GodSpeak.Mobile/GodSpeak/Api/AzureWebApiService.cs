﻿using System;
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
using System.Net;

namespace GodSpeak.Api
{
	public class AzureWebApiService : FakeWebApiService, IWebApiService
	{
		const string LoginMethodUri = "user/login";
		const string RecoverPasswordUri = "user/recoverpassword/";
		const string ReferralUri = "user/referral/";
		const string ProfileUri = "user";
		const string RegisterUri = "user";
		const string PhotoUploadUri = "user/photo";
		const string LogoutUri = "user/logout";

		const string CountriesUri = "geo/countries";
		const string PostalCodeExistsUri = "geo/postalCodeExists";

		const string ImpactDaysUri = "user/impact";
		const string ImpactMessageUri = "impact/message";
		const string ImpactDidYouKnowUri = "impact/didyouknow";

		const string MessagesQueueUri = "message/queue";

		const string PurchaseInviteUri = "invite/purchase";
		const string AcceptedInviteUri = "shared";
		const string ShareUri = "share";
		const string ValidateCodeUri = "invite/validate";
		const string InviteBundlesUri = "invite/bundles";
		const string RequestInviteUri = "invite/request";
		const string DonateInviteUri = "invite/donate";

		private ISettingsService _settingsService;
		protected HttpClient client = new HttpClient();
		readonly IMvxTrace tracer;

		//protected string ServerUrl = "http://godspeak-staging.azurewebsites.net/";
		protected string ServerUrl = "http://givegodspeak.azurewebsites.net/";

		public AzureWebApiService(ILogManager logManager, ISettingsService settingsService)
		{
			this.tracer = logManager.GetLog();

			_settingsService = settingsService;

			client.BaseAddress = new Uri(ServerUrl + "api/");
		}

		protected List<Message> CachedMessages = new List<Message>();

		public new async Task<ApiResponse<List<Message>>> GetMessages()
		{
			AddAuthToken(_settingsService.Token);
			var apiResponse = await DoGet<List<Message>>(MessagesQueueUri);

			CachedMessages = apiResponse.Payload;

			return apiResponse;
		}

		public new async Task<ApiResponse<List<ImpactDay>>> GetImpact()
		{
			AddAuthToken(_settingsService.Token);
			return await DoGet<List<ImpactDay>>(ImpactDaysUri);
		}

		public new async Task<ApiResponse<string>> Share(ShareRequest request)
		{
			AddAuthToken(_settingsService.Token);
			return await DoPost<string>(ShareUri, request);
		}

		public new async Task<ApiResponse<string>> SendReferral(SendReferralRequest request)
		{
			AddAuthToken(_settingsService.Token);
			return await DoPost<string>(ReferralUri, request);
		}

		public new async Task<ApiResponse<string>> RecordMessageDelivered(RecordMessageDeliveredRequest request)
		{
			AddAuthToken(_settingsService.Token);
			var response = await DoPost<string>(ImpactMessageUri, request);

			if (response.IsSuccess)
			{
				var verses = _settingsService.DeliveredVerseCodes;
				verses.Add(request.VerseCode);
				_settingsService.DeliveredVerseCodes = new List<string>(verses);
			}

			return response;
		}

		public new async Task<ApiResponse<List<Country>>> GetCountries()
		{
			var countriesList = await DoGet<List<Country>>(CountriesUri);

			if (countriesList.IsSuccess)
			{
				if (countriesList.Payload != null)
				{
					var userCountry = countriesList.Payload.FirstOrDefault(x => x.Code == "US");

					if (userCountry != null)
					{
						countriesList.Payload.Remove(userCountry);
						countriesList.Payload = new List<Country>(countriesList.Payload.OrderBy(x => x.Title));
						countriesList.Payload.Insert(0, userCountry);
					}
				}
			}

			return countriesList;
		}

		public new async Task<ApiResponse<String>> ForgotPassword(ForgotPasswordRequest request)
		{
			return await DoGet<String>(RecoverPasswordUri, new Dictionary<string, string>() { { "emailAddress", request.Email } });
		}

		public new async Task<ApiResponse<User>> RegisterUser(RegisterUserRequest request)
		{
			var response = await DoPost<User>(RegisterUri, request);

			if (response.IsSuccess)
			{
				_settingsService.Token = response.Payload.Token;
				_settingsService.Email = response.Payload.Email;
				_settingsService.Latitude = (float)response.Payload.Latitude;
				_settingsService.Longitude = (float)response.Payload.Longitude;
			}

			return response;
		}

		public new async Task<ApiResponse<User>> GetProfile()
		{
			AddAuthToken(_settingsService.Token);
			return await DoGet<User>(ProfileUri);
		}

		public new async Task<ApiResponse<User>> SaveProfile(User user)
		{
			AddAuthToken(_settingsService.Token);
			return await DoPut<User>(ProfileUri, user);
		}

		public new async Task<ApiResponse<ValidateCodeResponse>> ValidateCode(ValidateCodeRequest request)
		{
			return await DoGet<ValidateCodeResponse>(ValidateCodeUri, new Dictionary<string, string> { { "inviteCode", request.Code } });
		}

		public new async Task<ApiResponse<List<InviteBundle>>> GetInviteBundles(GetInviteBundlesRequest request)
		{
			return await DoGet<List<InviteBundle>>(InviteBundlesUri);
		}

		public new async Task<ApiResponse<List<AcceptedInvite>>> GetAcceptedInvites()
		{
			AddAuthToken(_settingsService.Token);
			return await DoGet<List<AcceptedInvite>>(AcceptedInviteUri);
		}

		public new async Task<ApiResponse<User>> DonateInvite()
		{
			AddAuthToken(_settingsService.Token);
			return await DoPost<User>(DonateInviteUri, null);
		}

		public new async Task<ApiResponse<string>> PurchaseInvite(PurchaseInviteRequest request)
		{
			AddAuthToken(_settingsService.Token);
			return await DoPost<string>(PurchaseInviteUri, request);
		}

		public new async Task<ApiResponse<User>> Login(LoginRequest request)
		{
			var response = await DoPost<User>(LoginMethodUri, request);

			if (response.IsSuccess)
			{
				_settingsService.Token = response.Payload.Token;
				_settingsService.Email = response.Payload.Email;
				_settingsService.Latitude = (float)response.Payload.Latitude;
				_settingsService.Longitude = (float)response.Payload.Longitude;
			}

			return response;
		}

		public new async Task<ApiResponse<string>> GetDidYouKnow()
		{
			AddAuthToken(_settingsService.Token);
			return await DoGet<string>(ImpactDidYouKnowUri);
		}

		public new async Task<ApiResponse<User>> UploadPhoto(UploadPhotoRequest request)
		{
			AddAuthToken(_settingsService.Token);

			IFile file = await FileSystem.Current.GetFileFromPathAsync(request.FilePath);
			using (Stream imageStream = await file.OpenAsync(FileAccess.Read))
			{

				MultipartFormDataContent multipartContent =
					new MultipartFormDataContent();

				multipartContent.Add(
					new StreamContent(imageStream),
					"photo",
					file.Name);

				HttpResponseMessage response = await client.PostAsync(PhotoUploadUri, multipartContent);

				return await ParseResponse<User>(PhotoUploadUri, response);
			}
		}

		private StreamContent CreateFileContent(Stream stream, string fileName, string contentType)
		{
			var fileContent = new StreamContent(stream);
			fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
			{
				Name = "\"files\"",
				FileName = "\"" + fileName + "\""
			}; // the extra quotes are key here
			fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
			return fileContent;
		}

		public new async Task<ApiResponse> Logout()
		{
			AddAuthToken(_settingsService.Token);
			return await DoPost(LogoutUri);
		}

		protected async Task<ApiResponse> DoPost(string uri)
		{
			try
			{
				var apiResponse = await client.PostAsync(uri, null);

				return await ParseResponse(uri, apiResponse);
			}
			catch (Exception ex)
			{
				return HandleException(ex, uri);
			}
		}

		protected async Task<ApiResponse<T>> DoPost<T>(string uri, object request)
		{
			try
			{
				var jsonBody = JsonConvert.SerializeObject(request);

				tracer.Trace(MvxTraceLevel.Diagnostic, "api-post", $"METHOD: {uri}\rBODY: {jsonBody}");

				var apiResponse = await client.PostAsync(uri, new StringContent(jsonBody, Encoding.UTF8, "application/json"));

				return await ParseResponse<T>(uri, apiResponse);
			}
			catch (Exception ex)
			{
				return HandleException<T>(ex, uri);
			}
		}

		protected async Task<ApiResponse<T>> DoPut<T>(string uri, object request)
		{
			try
			{
				var jsonBody = JsonConvert.SerializeObject(request);

				tracer.Trace(MvxTraceLevel.Diagnostic, "api-post", $"METHOD: {uri}\rBODY: {jsonBody}");

				var apiResponse = await client.PutAsync(uri, new StringContent(jsonBody, Encoding.UTF8, "application/json"));

				return await ParseResponse<T>(uri, apiResponse);
			}
			catch (Exception ex)
			{
				return HandleException<T>(ex, uri);
			}
		}

		protected async Task<ApiResponse<T>> DoGet<T>(string uri)
		{
			try
			{
				tracer.Trace(MvxTraceLevel.Diagnostic, "api-get", $"METHOD: {uri}");
				var apiResponse = await client.GetAsync(uri);

				return await ParseResponse<T>(uri, apiResponse);
			}
			catch (Exception ex)
			{
				return HandleException<T>(ex, uri);
			}
		}

		protected async Task<ApiResponse<T>> DoGet<T>(string uri, Dictionary<string, string> args)
		{
			try
			{
				var builder = new StringBuilder("?");

				foreach (var pair in args)
				{
					if (builder.Length != 1)
						builder.Append("&");
					builder.Append($"{pair.Key}={System.Net.WebUtility.UrlDecode(pair.Value)}");
				};

				var requestUri = uri + builder;
				tracer.Trace(MvxTraceLevel.Diagnostic, "api-get", $"METHOD: {requestUri}");
				var apiResponse = await client.GetAsync(requestUri);

				return await ParseResponse<T>(uri, apiResponse);
			}
			catch (Exception ex)
			{
				return HandleException<T>(ex, uri);
			}
		}

		private ApiResponse<T> HandleException<T>(Exception ex, string uri)
		{
			if (ex is TaskCanceledException)
			{
				tracer.Trace(MvxTraceLevel.Error, uri, ex.Message);
				return new ApiResponse<T>()
				{
					StatusCode = System.Net.HttpStatusCode.RequestTimeout
				};
			}
			else if (ex is WebException || ex is HttpRequestException)
			{
				tracer.Trace(MvxTraceLevel.Error, uri, ex.Message);
				return new ApiResponse<T>()
				{
					StatusCode = System.Net.HttpStatusCode.RequestTimeout
				};
			}
			else
			{
				tracer.Trace(MvxTraceLevel.Error, uri, ex.Message);
				return new ApiResponse<T>()
				{
					StatusCode = System.Net.HttpStatusCode.InternalServerError
				};
			}
		}

		private ApiResponse HandleException(Exception ex, string uri)
		{
			if (ex is TaskCanceledException)
			{
				tracer.Trace(MvxTraceLevel.Error, uri, ex.Message);
				return new ApiResponse()
				{
					StatusCode = System.Net.HttpStatusCode.RequestTimeout
				};
			}
			else if (ex is WebException || ex is HttpRequestException)
			{
				tracer.Trace(MvxTraceLevel.Error, uri, ex.Message);
				return new ApiResponse()
				{
					StatusCode = System.Net.HttpStatusCode.RequestTimeout
				};
			}
			else
			{
				tracer.Trace(MvxTraceLevel.Error, uri, ex.Message);
				return new ApiResponse()
				{
					StatusCode = System.Net.HttpStatusCode.InternalServerError
				};
			}
		}


		protected async Task<ApiResponse> ParseResponse(string uri, HttpResponseMessage apiResponse)
		{
			var json = await apiResponse.Content.ReadAsStringAsync();
			var parsedResponse = JsonConvert.DeserializeObject<ApiResponse>(json);
			LogResponse(uri, parsedResponse, json);
			return parsedResponse;
		}

		protected async Task<ApiResponse<T>> ParseResponse<T>(string uri, HttpResponseMessage apiResponse)
		{
			var json = await apiResponse.Content.ReadAsStringAsync();
			var parsedResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(json);
			parsedResponse.StatusCode = apiResponse.StatusCode;
			LogResponse(uri, parsedResponse, json);

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

		void AddAuthToken(string token)
		{
			if (client.DefaultRequestHeaders.Contains("token"))
				client.DefaultRequestHeaders.Remove("token");

			client.DefaultRequestHeaders.Add("token", token);
		}

		void LogResponse<T>(string method, ApiResponse<T> parsedResponse, string json)
		{
			tracer.Trace(parsedResponse.StatusCode == System.Net.HttpStatusCode.OK ? MvxTraceLevel.Diagnostic : MvxTraceLevel.Error, "api-response", $"METHOD: {method}\rSTATUS CODE: {parsedResponse.StatusCode}\rTITLE:{parsedResponse.Title}\rMESSAGE:{parsedResponse.Message}\rPAYLOAD:{JsonConvert.SerializeObject(parsedResponse.Payload)}");
		}

		void LogResponse(string method, ApiResponse parsedResponse, string json)
		{
			tracer.Trace(parsedResponse.StatusCode == System.Net.HttpStatusCode.OK ? MvxTraceLevel.Diagnostic : MvxTraceLevel.Error, "api-response", $"METHOD: {method}\rSTATUS CODE: {parsedResponse.StatusCode}\rTITLE:{parsedResponse.Title}\rMESSAGE:{parsedResponse.Message}");
		}
	}
}