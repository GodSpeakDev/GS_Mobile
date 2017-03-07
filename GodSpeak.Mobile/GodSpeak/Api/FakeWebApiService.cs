using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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
			await Task.Delay(delay);
			return new BaseResponse<GetCategoriesResponse>()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Content = new GetCategoriesResponse() 
				{
					Payload = _categories
				}
			};
		}

		public async Task<BaseResponse<GetImpactResponse>> GetImpact(GetImpactRequest request)
		{
			await Task.Delay(delay);
			return new BaseResponse<GetImpactResponse>()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Content = new GetImpactResponse()
				{
					Payload = GetImpactedDays()
				}
			};
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
						DateTimeToDisplay = DateTime.Now.AddSeconds(30),
						Text = "If you abide in Me, and My words abide in you, ask whatever you wish, and it will be done for you. - John 15:7 NASB",
						MessageId = Guid.NewGuid(),
						Image = ""
					},
					new Message() 
					{
						DateTimeToDisplay = DateTime.Now,
						Text = "If you abide in Me, and My words abide in you, ask whatever you wish, and it will be done for you. - John 15:7 NASB",
						MessageId = Guid.NewGuid(),
						Image = ""
					},
					new Message()
					{
						DateTimeToDisplay = DateTime.Now,
						Text = "Be anxious for nothing, but in everything by prayer and supplication with thanksgiving let your requests be made known to God. - Philippians 4:6 NASB",
						MessageId = Guid.NewGuid(),
						Image = ""
					},
					new Message()
					{
						DateTimeToDisplay = DateTime.Now.AddDays(-1),
						Text = "Therefore I say to you, all things for which you pray and ask, believe that you have received them, and they will be granted you. - Mark 11:24 NASB",
						MessageId = Guid.NewGuid(),
						Image = ""
					},
					new Message()
					{
						DateTimeToDisplay = DateTime.Now.AddDays(-1),
						Text = "And when you are praying, do not use meaningless repetition as the Gentiles do, for they suppose that they will be heard for their many words. - Matthew 6:7 NASB",
						MessageId = Guid.NewGuid(),
						Image = ""
					},
					new Message()
					{
						DateTimeToDisplay = DateTime.Now.AddDays(-7),
						Text = "So I say to you, ask, and it will be given to you; seek, and you will find; knock, and it will be opened to you. - Luke 11:9 NASB",
						MessageId = Guid.NewGuid(),
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
			await Task.Delay(delay);

			foreach (var category in _categories)
			{
				category.Enabled = (request.Payload.FirstOrDefault(x => x.Id == category.Id)?.Enabled).GetValueOrDefault();
			}

			return new BaseResponse<SaveCategoriesResponse>()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Content = new SaveCategoriesResponse()
			};
		}

		public async Task<BaseResponse<SetMessagesConfigResponse>> SetMessagesConfigUser(SetMessagesConfigRequest request)
		{
			await Task.Delay(delay);
			_dailySettings = request.Settings;
			return new BaseResponse<SetMessagesConfigResponse>()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Content = new SetMessagesConfigResponse() 
				{
					Payload = _dailySettings
				}
			};
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

		public async Task<BaseResponse<GetMessageConfigResponse>> GetMessageConfig(GetMessageConfigRequest request)
		{
			await Task.Delay(delay);
			return new BaseResponse<GetMessageConfigResponse>()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Content = new GetMessageConfigResponse()
				{
					Payload = _dailySettings
				}
			};
		}

		private User GetTestPayload()
		{
			return new User()
			{
				Id = Guid.NewGuid(),
				Email = "godspeak@gmail.com",
				ZipCode="63017",
				Country="USA",
				Credits = 15.0m,
				FirstName = "GodSpeak",
				LastName = "GodSpeak",
				PhotoUrl = "http://images.clipartpanda.com/happy-man-images-A-Happy-Man.jpg",
				Token = Guid.NewGuid().ToString(),
				SelectedCategories = _categories,
				DayOfWeekSettings = new List<DayOfWeekSettings>()
				{
					
				}
			};
		}

		private List<ImpactDay> GetImpactedDays()
		{
			var random = new Random();

			var impactedDays = new List<ImpactDay>();

			for (int i = 0; i < 30; i++)
			{
				Task.Delay(100);
				var impactDay = new ImpactDay()
				{
					Date = DateTime.Now.AddDays(-i),
					InvitesClaimed = 1,
					ScripturesDelivered = 1,
					MapPoints = new List<MapPoint>()
					{
						new MapPoint()
						{
							Title = DateTime.Now.AddDays(-i).ToString(),
							Latitude = (float) (38.4 + (random.Next(0, 3000) / 10000.0)),
							Longitude = (float) (-90.1 + (random.Next(0, 3000) / 10000.0)),
							MapPointId = Guid.NewGuid()
						},
						new MapPoint()
						{
							Title = DateTime.Now.AddDays(-i).ToString(),
							Latitude =  (float) (38.4 + (random.Next(0, 3000) / 10000.0)),
							Longitude = (float) (-90.1 + (random.Next(0, 3000) / 10000.0)),
							MapPointId = Guid.NewGuid()
						},
					}
				};

				impactedDays.Add(impactDay);
			}

			return impactedDays;
		}

		private static List<MessageCategory> _categories = new List<MessageCategory>() 
		{
			new MessageCategory() 
			{
				Id= Guid.NewGuid(),
				Enabled = false,
				Title = "Inspiration"
			},
			new MessageCategory()
			{
				Id= Guid.NewGuid(),
				Enabled = true,
				Title = "New Testament"
			},
			new MessageCategory()
			{
				Id= Guid.NewGuid(),
				Enabled = false,
				Title = "Old Testament"
			},
			new MessageCategory()
			{
				Id= Guid.NewGuid(),
				Enabled = true,
				Title = "Proverbs"
			}
		};

		private static List<DayOfWeekSettings> _dailySettings = new List<DayOfWeekSettings>()
		{
			new DayOfWeekSettings()
			{
				DayOfWeekSettingsId = Guid.NewGuid(),
				Weekday = 0,
				Enabled = true,
				StartDateTime = DateTime.Now.Date.AddHours(10),
				EndDateTime = DateTime.Now.Date.AddHours(20),
				NumberOfMessages = 3
			},
			new DayOfWeekSettings()
			{
				DayOfWeekSettingsId = Guid.NewGuid(),
				Weekday = 1,
				Enabled = false,
				StartDateTime = DateTime.Now.Date.AddHours(10),
				EndDateTime = DateTime.Now.Date.AddHours(20),
				NumberOfMessages = 3
			},
			new DayOfWeekSettings()
			{
				DayOfWeekSettingsId = Guid.NewGuid(),
				Weekday = 2,
				Enabled = false,
				StartDateTime = DateTime.Now.Date.AddHours(10),
				EndDateTime = DateTime.Now.Date.AddHours(20),
				NumberOfMessages = 3
			},
			new DayOfWeekSettings()
			{
				DayOfWeekSettingsId = Guid.NewGuid(),
				Weekday = 3,
				Enabled = true,
				StartDateTime = DateTime.Now.Date.AddHours(10),
				EndDateTime = DateTime.Now.Date.AddHours(10),
				NumberOfMessages = 3
			},
			new DayOfWeekSettings()
			{
				DayOfWeekSettingsId = Guid.NewGuid(),
				Weekday = 4,
				Enabled = true,
				StartDateTime = DateTime.Now.Date.AddHours(12),
				EndDateTime = DateTime.Now.Date.AddHours(17),
				NumberOfMessages = 3
			},
			new DayOfWeekSettings()
			{
				DayOfWeekSettingsId = Guid.NewGuid(),
				Weekday = 5,
				Enabled = false,
				StartDateTime = DateTime.Now.Date.AddHours(12),
				EndDateTime = DateTime.Now.Date.AddHours(17),
				NumberOfMessages = 3
			},
			new DayOfWeekSettings()
			{
				DayOfWeekSettingsId = Guid.NewGuid(),
				Weekday = 6,
				Enabled = false,
				StartDateTime = DateTime.Now.Date.AddHours(10),
				EndDateTime = DateTime.Now.Date.AddHours(20),
				NumberOfMessages = 3
			},
		};
	}
}
