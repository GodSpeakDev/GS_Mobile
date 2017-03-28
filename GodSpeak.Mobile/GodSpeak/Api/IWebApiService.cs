using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GodSpeak.Api.Dtos;

namespace GodSpeak
{
    public interface IWebApiService
    {
        Task<ApiResponse<List<Country>>> GetCountries ();
        Task<ApiResponse<ValidateCodeResponse>> ValidateCode (ValidateCodeRequest request);
        Task<ApiResponse<RequestInviteResponse>> RequestInvite (RequestInviteRequest request);
        Task<ApiResponse<User>> Login (LoginRequest request);
        Task<ApiResponse<LogoutResponse>> Logout (LogoutRequest request);
        Task<ApiResponse<GetCategoriesResponse>> GetCategories (GetCategoriesRequest request);
        Task<ApiResponse<GetMessageConfigResponse>> GetMessageConfig (GetMessageConfigRequest request);
        Task<ApiResponse<GetImpactResponse>> GetImpact (GetImpactRequest request);
        Task<ApiResponse<GetInvitesResponse>> GetInvites (GetInvitesRequest request);
        Task<ApiResponse<List<InviteBundle>>> GetInviteBundles (GetInviteBundlesRequest request);
        Task<ApiResponse<PurchaseInviteResponse>> PurchaseInvite (PurchaseInviteRequest request);
        Task<ApiResponse<GetMessagesResponse>> GetMessages (GetMessagesRequest request);
        Task<ApiResponse<GetMessageResponse>> GetMessage (GetMessageRequest request);
        Task<ApiResponse<string>> ForgotPassword (ForgotPasswordRequest request);
        Task<ApiResponse<RegisterUserResponse>> RegisterUser (RegisterUserRequest request);
        Task<ApiResponse<SaveCategoriesResponse>> SaveCategories (SaveCategoriesRequest request);
        Task<ApiResponse<SetMessagesConfigResponse>> SetMessagesConfigUser (SetMessagesConfigRequest request);
    }
}
