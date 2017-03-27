using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace GodSpeak.Api
{
    public class AzureWebApiService : IWebApiService
    {

        protected HttpClient client = new HttpClient ();

        public AzureWebApiService ()
        {
        }

        public Task<BaseResponse<ForgotPasswordResponse>> ForgotPassword (ForgotPasswordRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<GetCategoriesResponse>> GetCategories (GetCategoriesRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<GetImpactResponse>> GetImpact (GetImpactRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<GetInviteBundlesResponse>> GetInviteBundles (GetInviteBundlesRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<GetInvitesResponse>> GetInvites (GetInvitesRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<GetMessageResponse>> GetMessage (GetMessageRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<GetMessageConfigResponse>> GetMessageConfig (GetMessageConfigRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<GetMessagesResponse>> GetMessages (GetMessagesRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<LoginResponse>> Login (LoginRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<LogoutResponse>> Logout (LogoutRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<PurchaseInviteResponse>> PurchaseInvite (PurchaseInviteRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<RegisterUserResponse>> RegisterUser (RegisterUserRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<RequestInviteResponse>> RequestInvite (RequestInviteRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<SaveCategoriesResponse>> SaveCategories (SaveCategoriesRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<SetMessagesConfigResponse>> SetMessagesConfigUser (SetMessagesConfigRequest request)
        {
            throw new NotImplementedException ();
        }

        public Task<BaseResponse<ValidateCodeResponse>> ValidateCode (ValidateCodeRequest request)
        {
            throw new NotImplementedException ();
        }
    }
}
