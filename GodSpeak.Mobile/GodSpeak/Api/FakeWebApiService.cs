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
			await Task.Delay(1000);

			if (request.Code == "123456")
			{
				return new BaseResponse<ValidateCodeResponse>()
				{
					StatusCode = System.Net.HttpStatusCode.OK,
				};
			}
			else
			{
				return new BaseResponse<ValidateCodeResponse>()
				{
					StatusCode = System.Net.HttpStatusCode.BadRequest,
					ErrorMessage = "Sorry, that code is not valid.",
					ErrorTitle = "Error Code"
				};	
			}
		}

		public async Task<BaseResponse<RequestInviteResponse>> RequestInvite(RequestInviteRequest request)
		{
			await Task.Delay(1000);

			return new BaseResponse<RequestInviteResponse>()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
			};
		}

		public async Task<BaseResponse<LoginResponse>> Login(LoginRequest request)
		{
			await Task.Delay(1000);

			if (request.Email == "godspeak@gmail.com" && request.Password == "123456")
			{
				return new BaseResponse<LoginResponse>()
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Content = new LoginResponse() 
					{
						Payload = GetTestPayload()
					}
				};
			}
			else
			{
				return new BaseResponse<LoginResponse>() 
				{
					StatusCode = System.Net.HttpStatusCode.BadRequest,
					ErrorTitle = "Invalid Credentials",
					ErrorMessage = "Your login or password doesn't match."
				};
			}
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
			await Task.Delay(delay);
			return new BaseResponse<GetInvitesResponse>()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Content = new GetInvitesResponse()
				{
					Payload = new List<Invite>() 
					{
						new Invite() 
						{
							InviteId = Guid.NewGuid(),
							Code = "1reJy567",
						},
						new Invite()
						{
							InviteId = Guid.NewGuid(),
							Code = "JUik34a5",
						},
						new Invite()
						{
							InviteId = Guid.NewGuid(),
							Code = "MnIiuo89",
							RedeemerEmail="jon.smith@gmail.com",
							Redeemed=true
						},
						new Invite()
						{
							InviteId = Guid.NewGuid(),
							Code = "OiUUytad",
						},
						new Invite()
						{
							InviteId = Guid.NewGuid(),
							Code = "12FjUIO",
							RedeemerEmail="dave.ortinau@gmail.com",
							Redeemed=true
						},
					}
				}
			};
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
			await Task.Delay(delay);
			return new BaseResponse<ForgotPasswordResponse>()
			{
				StatusCode = System.Net.HttpStatusCode.OK
			};
		}

		public async Task<BaseResponse<RegisterUserResponse>> RegisterUser(RegisterUserRequest request)
		{
			await Task.Delay(delay);
			return new BaseResponse<RegisterUserResponse>()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Content = new RegisterUserResponse()
				{
					Payload = GetTestPayload()
				}
			};
		}

		public async Task<BaseResponse<SaveCategoriesResponse>> SaveCategories(SaveCategoriesRequest request)
		{
			return null;
		}

		public async Task<BaseResponse<SetMessagesConfigResponse>> SetMessagesConfigUser(SetMessagesConfigRequest request)
		{
			return null;
		}

		public async Task<BaseResponse<GetInviteBundlesResponse>> GetInviteBundles(GetInviteBundlesRequest request)
		{
			await Task.Delay(delay);
			return new BaseResponse<GetInviteBundlesResponse>()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Content = new GetInviteBundlesResponse()
				{
					Payload = new List<InviteBundle>()
					{
						new InviteBundle() 
						{
							InviteBundleId=Guid.NewGuid(),
							Cost = 1.99m,
							NumOfInvites=1,
							ItunesId="1",
							PlaystoreId="1"
						},
						new InviteBundle()
						{
							InviteBundleId=Guid.NewGuid(),
							Cost = 3.99m,
							NumOfInvites=5,
							ItunesId="2",
							PlaystoreId="2"
						},
						new InviteBundle()
						{
							InviteBundleId=Guid.NewGuid(),
							Cost = 6.99m,
							NumOfInvites=10,
							ItunesId="3",
							PlaystoreId="3"
						},
					}
				}
			};
		}

		public async Task<BaseResponse<PurchaseInviteResponse>> PurchaseInvite(PurchaseInviteRequest request)
		{
			await Task.Delay(delay);
			return new BaseResponse<PurchaseInviteResponse>()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
			};
		}

		private User GetTestPayload()
		{
			return new User()
			{
				Id = Guid.NewGuid(),
				Email = "godspeak@gmail.com",
				City = "St. Louis",
				State = "MO",
				Credits = 15.0m,
				FirstName = "GodSpeak",
				LastName = "GodSpeak",
				PhotoUrl = "http://images.clipartpanda.com/happy-man-images-A-Happy-Man.jpg",
				Token = Guid.NewGuid().ToString(),
				SelectedCategories = new List<MessageCategory>()
				{

				},
				DayOfWeekSettings = new List<DayOfWeekSettings>()
				{

				}
			};
		}
	}
}