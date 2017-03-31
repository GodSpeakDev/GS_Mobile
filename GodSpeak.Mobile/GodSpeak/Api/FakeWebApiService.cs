using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GodSpeak.Api.Dtos;

namespace GodSpeak
{
    public class FakeWebApiService : IWebApiService
    {
        private int delay = 1000;

        public async Task<ApiResponse<ValidateCodeResponse>> ValidateCode (ValidateCodeRequest request)
        {
            await Task.Delay (1000);

            if (request.Code == "123456") {
                return new ApiResponse<ValidateCodeResponse> () {
                    StatusCode = System.Net.HttpStatusCode.OK,
                };
            } else {
                return new ApiResponse<ValidateCodeResponse> () {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Sorry, that code is not valid.",
                    Title = "Error Code"
                };
            }
        }

        public async Task<ApiResponse<RequestInviteResponse>> RequestInvite (RequestInviteRequest request)
        {
            await Task.Delay (1000);

            return new ApiResponse<RequestInviteResponse> () {
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }

        public async Task<ApiResponse<User>> Login (LoginRequest request)
        {
            await Task.Delay (1000);

            if (request.Email == "Ben@rendr.io" && request.Password == "J0hn_galt") {
                return new ApiResponse<User> () {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Payload = GetTestPayload ()
                };
            } else {
                return new ApiResponse<User> () {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Title = "Invalid Credentials",
                    Message = "Your login or password doesn't match."
                };
            }
        }

        public async Task<ApiResponse> Logout (LogoutRequest request)
        {
            return null;
        }



        public async Task<ApiResponse<GetInvitesResponse>> GetInvites (GetInvitesRequest request)
        {
            await Task.Delay (delay);
            return new ApiResponse<GetInvitesResponse> () {
                StatusCode = System.Net.HttpStatusCode.OK,
                Payload = new GetInvitesResponse () {
                    Payload = new List<Invite> ()
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

        public async Task<ApiResponse<GetMessagesResponse>> GetMessages (GetMessagesRequest request)
        {
            await Task.Delay (delay);
            var content = new GetMessagesResponse () {
                Messages = _messages
            };

            return new ApiResponse<GetMessagesResponse> () {
                Payload = content,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<ApiResponse<GetMessageResponse>> GetMessage (GetMessageRequest request)
        {
            await Task.Delay (delay);
            var content = new GetMessageResponse () {
                Payload = _messages.First (x => x.MessageId == request.MessageId)
            };

            return new ApiResponse<GetMessageResponse> () {
                Payload = content,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<ApiResponse<string>> ForgotPassword (ForgotPasswordRequest request)
        {
            await Task.Delay (delay);
            return new ApiResponse<string> () {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<ApiResponse<User>> RegisterUser (RegisterUserRequest request)
        {
            await Task.Delay (delay);
            return new ApiResponse<User> () {
                StatusCode = System.Net.HttpStatusCode.OK,
                Payload = GetTestPayload ()

            };
        }

        public async Task<ApiResponse<SaveCategoriesResponse>> SaveCategories (SaveCategoriesRequest request)
        {
            await Task.Delay (delay);

            foreach (var category in _categories) {
                category.Enabled = (request.Payload.FirstOrDefault (x => x.Id == category.Id)?.Enabled).GetValueOrDefault ();
            }

            return new ApiResponse<SaveCategoriesResponse> () {
                StatusCode = System.Net.HttpStatusCode.OK,
                Payload = new SaveCategoriesResponse ()
            };
        }

        public async Task<ApiResponse<SetMessagesConfigResponse>> SetMessagesConfigUser (SetMessagesConfigRequest request)
        {
            await Task.Delay (delay);
            _dailySettings = request.Settings;
            return new ApiResponse<SetMessagesConfigResponse> () {
                StatusCode = System.Net.HttpStatusCode.OK,
                Payload = new SetMessagesConfigResponse () {
                    Payload = _dailySettings
                }
            };
        }

        public async Task<ApiResponse<List<InviteBundle>>> GetInviteBundles (GetInviteBundlesRequest request)
        {
            await Task.Delay (delay);
            return new ApiResponse<List<InviteBundle>> () {
                StatusCode = System.Net.HttpStatusCode.OK,
                Payload = new List<InviteBundle> ()
                    {
                        new InviteBundle()
                        {
                            InviteBundleId=Guid.NewGuid(),
                            Cost = 9.97m,
                            NumberOfInvites=3,
                            AppStoreSku="1",
                            PlayStoreSku="1",
                            CostDescription = "$1.99"
                        },
                        new InviteBundle()
                        {
                            InviteBundleId=Guid.NewGuid(),
                            Cost = 19.90m,
                            NumberOfInvites=10,
                            AppStoreSku="2",
                            PlayStoreSku="2",
                            CostDescription = "$4.99"
                        },
                        new InviteBundle()
                        {
                            InviteBundleId=Guid.NewGuid(),
                            Cost = 24.75m,
                            NumberOfInvites=25,
                            AppStoreSku="3",
                            PlayStoreSku="3",
                            CostDescription = "$24.75"
                        },
                        new InviteBundle()
                        {
                            InviteBundleId=Guid.NewGuid(),
                            Cost = 44.50m,
                            NumberOfInvites=50,
                            AppStoreSku="4",
                            PlayStoreSku="4",
                            CostDescription = "$44.50"
                        },
                    }
            };
        }

        public async Task<ApiResponse<PurchaseInviteResponse>> PurchaseInvite (PurchaseInviteRequest request)
        {
            await Task.Delay (delay);
            return new ApiResponse<PurchaseInviteResponse> () {
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }



        private User GetTestPayload ()
        {
            return new User () {
                Id = Guid.NewGuid (),
                Email = "godspeak@gmail.com",
                PostalCode = "63017",
                CountryCode = "USA",
                InviteBalance = 15,
                FirstName = "GodSpeak",
                LastName = "GodSpeak",
                PhotoUrl = "http://images.clipartpanda.com/happy-man-images-A-Happy-Man.jpg",
                Token = Guid.NewGuid ().ToString (),
                MessageCategorySettings = _categories,
                MessageDayOfWeekSettings = new List<DayOfWeekSettings> () {

                }
            };
        }

        private List<ImpactDay> GetImpactedDays ()
        {
            var random = new Random ();

            var impactedDays = new List<ImpactDay> ();

            for (int i = 0; i < 30; i++) {
                Task.Delay (100);
                var impactDay = new ImpactDay () {
                    Date = DateTime.Now.AddDays (-i),
                    InvitesClaimed = 1,
                    ScripturesDelivered = 1,
                    MapPoints = new List<MapPoint> ()
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

                impactedDays.Add (impactDay);
            }

            return impactedDays;
        }

        public async Task<ApiResponse<List<Country>>> GetCountries ()
        {
            await Task.Delay (delay);
            return new ApiResponse<List<Country>> () {
                StatusCode = System.Net.HttpStatusCode.OK,
                Payload = new List<Country> () {
                    new Country(){ Code = "US", Title = "United States of America"}
                }
            };
        }

        public async Task<ApiResponse<User>> GetProfile (TokenRequest request)
        {
            await Task.Delay (delay);
            return new ApiResponse<User> () { Payload = new User () };
        }

        public async Task<ApiResponse<GetImpactResponse>> GetImpact (GetImpactRequest request)
        {
            await Task.Delay (delay);
            return new ApiResponse<GetImpactResponse> () {
                StatusCode = System.Net.HttpStatusCode.OK,
                Payload = new GetImpactResponse () {
                    Payload = GetImpactedDays ()
                }
            };

        }

        private static List<MessageCategory> _categories = new List<MessageCategory> ()
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

        private static List<DayOfWeekSettings> _dailySettings = new List<DayOfWeekSettings> ()
        {
            new DayOfWeekSettings()
            {
                Id = Guid.NewGuid(),
                Title="Sunday",
                Enabled = true,
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(20),
                NumOfMessages = 3
            },
            new DayOfWeekSettings()
            {
                Id = Guid.NewGuid(),
                Title="Monday",
                Enabled = false,
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(20),
                NumOfMessages = 3
            },
            new DayOfWeekSettings()
            {
                Id = Guid.NewGuid(),
                Title="Tuesday",
                Enabled = false,
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(20),
                NumOfMessages = 3
            },
            new DayOfWeekSettings()
            {
                Id = Guid.NewGuid(),
                Title="Wednesday",
                Enabled = true,
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(10),
                NumOfMessages = 3
            },
            new DayOfWeekSettings()
            {
                Id = Guid.NewGuid(),
                Title="Thursday",
                Enabled = true,
                StartTime = TimeSpan.FromHours(12),
                EndTime = TimeSpan.FromHours(17),
                NumOfMessages = 3
            },
            new DayOfWeekSettings()
            {
                Id = Guid.NewGuid(),
                Title="Friday",
                Enabled = false,
                StartTime = TimeSpan.FromHours(12),
                EndTime = TimeSpan.FromHours(17),
                NumOfMessages = 3
            },
            new DayOfWeekSettings()
            {
                Id = Guid.NewGuid(),
                Title="Saturday",
                Enabled = false,
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(20),
                NumOfMessages = 3
            },
        };

        private static List<Message> _messages = new List<Message> ()
        {
            new Message()
            {
                DateTimeToDisplay = DateTime.Now.AddSeconds(30),
                BeforeVerse="Therefore I say to you, all things for which you pray and ask, believe that you have received them, and they will be granted you.",
                Text = "If you abide in Me, and My words abide in you, ask whatever you wish, and it will be done for you.",
                AfterVerse="Truly I say to you, whoever says to this mountain, 'Be taken up and cast into the sea,' and does not doubt in his heart, but believes that what he says is going to happen, it will be granted him.",
                MessageId = Guid.NewGuid(),
                BeforeAuthor = "John 15:6 NASB",
                Author="John 15:7 NASB",
                AfterAuthor="John 15:8 NASB",
            },
            new Message()
            {
                DateTimeToDisplay = DateTime.Now,
                AfterVerse="Truly I say to you, whoever says to this mountain, 'Be taken up and cast into the sea,' and does not doubt in his heart, but believes that what he says is going to happen, it will be granted him.",
                Text = "If you abide in Me, and My words abide in you, ask whatever you wish, and it will be done for you.",
                BeforeVerse="Therefore I say to you, all things for which you pray and ask, believe that you have received them, and they will be granted you.",
                MessageId = Guid.NewGuid(),
                BeforeAuthor = "John 15:6 NASB",
                Author="John 15:7 NASB",
                AfterAuthor="John 15:8 NASB",
            },
            new Message()
            {
                DateTimeToDisplay = DateTime.Now,
                AfterVerse="Therefore I say to you, all things for which you pray and ask, believe that you have received them, and they will be granted you.",
                Text = "Be anxious for nothing, but in everything by prayer and supplication with thanksgiving let your requests be made known to God.",
                MessageId = Guid.NewGuid(),
                Author="Philippians 4:6 NASB",
                AfterAuthor="Philippians 4:7 NASB",
            },
            new Message()
            {
                DateTimeToDisplay = DateTime.Now.AddDays(-1),
                Text = "Therefore I say to you, all things for which you pray and ask, believe that you have received them, and they will be granted you.",
                BeforeVerse="Truly I say to you, whoever says to this mountain, 'Be taken up and cast into the sea,' and does not doubt in his heart, but believes that what he says is going to happen, it will be granted him.",
                MessageId = Guid.NewGuid(),
                Author="Mark 11:24 NASB",
                BeforeAuthor="Mark 11:23 NASB",
            },
            new Message()
            {
                DateTimeToDisplay = DateTime.Now.AddDays(-1),
                Text = "And when you are praying, do not use meaningless repetition as the Gentiles do, for they suppose that they will be heard for their many words.",
                MessageId = Guid.NewGuid(),
                Author="Mark 11:24 NASB"
            },
            new Message()
            {
                DateTimeToDisplay = DateTime.Now.AddDays(-7),
                Text = "So I say to you, ask, and it will be given to you; seek, and you will find; knock, and it will be opened to you.",
                MessageId = Guid.NewGuid(),
                Author="Mark 11:24 NASB"
            },
        };
    }
}
