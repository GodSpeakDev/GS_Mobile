﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GodSpeak
{
	public class FakeWebApiService : IWebApiService
	{
		private int delay = 1000;

		public async Task<BaseResponse<ValidateCodeResponse>> ValidateCode(ValidateCodeRequest request)
		{
			return null;	
		}

		public async Task<BaseResponse<LoginResponse>> Login(LoginRequest request)
		{
			return null;
		}

		public async Task<BaseResponse<LogoutResponse>> Logout(LogoutRequest request)
		{
			return null;
		}

		public async Task<BaseResponse<GetCategoriesResponse>> GetCategories(GetCategoriesRequest request)
		{
			return null;
		}

		public async Task<BaseResponse<GetImpactResponse>> GetImpact(GetImpactRequest request)
		{
			return null;	
		}

		public async Task<BaseResponse<GetInvitesResponse>> GetInvites(GetInvitesRequest request)
		{
			return null;	
		}

		public async Task<BaseResponse<GetMessagesResponse>> GetMessages(GetMessagesRequest request)
		{
			await Task.Delay(delay);
			var content = new GetMessagesResponse()
            {
				Messages = new List<Message>()
				{
					new Message() 
					{
						Date = DateTime.Now,
						Text = "If you abide in Me, and My words abide in you, ask whatever you wish, and it will be done for you. - John 15:7 NASB",
						Id = 1,
						Image = ""
					},
					new Message()
					{
						Date = DateTime.Now,
						Text = "Be anxious for nothing, but in everything by prayer and supplication with thanksgiving let your requests be made known to God. - Philippians 4:6 NASB",
						Id = 2,
						Image = ""
					},
					new Message()
					{
						Date = DateTime.Now.AddDays(-1),
						Text = "Therefore I say to you, all things for which you pray and ask, believe that you have received them, and they will be granted you. - Mark 11:24 NASB",
						Id = 3,
						Image = ""
					},
					new Message()
					{
						Date = DateTime.Now.AddDays(-1),
						Text = "And when you are praying, do not use meaningless repetition as the Gentiles do, for they suppose that they will be heard for their many words. - Matthew 6:7 NASB",
						Id = 3,
						Image = ""
					},
					new Message()
					{
						Date = DateTime.Now.AddDays(-7),
						Text = "So I say to you, ask, and it will be given to you; seek, and you will find; knock, and it will be opened to you. - Luke 11:9 NASB",
						Id = 4,
						Image = ""
					},
				}
            };

			return new BaseResponse<GetMessagesResponse>() 
			{
			    Content = content,
			    StatusCode = System.Net.HttpStatusCode.OK
			};
		}

		public async Task<BaseResponse<ForgotPasswordResponse>> ForgotPassword(ForgotPasswordRequest request)
		{
			return null;
		}

		public async Task<BaseResponse<RegisterUserResponse>> RegisterUser(RegisterUserRequest request)
		{
			return null;
		}

		public async Task<BaseResponse<SaveCategoriesResponse>> SaveCategories(SaveCategoriesRequest request)
		{
			return null;
		}

		public async Task<BaseResponse<SetMessagesConfigResponse>> SetMessagesConfigUser(SetMessagesConfigRequest request)
		{
			return null;
		}
	}
}
