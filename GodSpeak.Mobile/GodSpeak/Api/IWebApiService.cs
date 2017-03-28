using System;
using System.Threading.Tasks;

namespace GodSpeak
{
	public interface IWebApiService
	{
		Task<ApiResponse<ValidateCodeResponse>> ValidateCode(ValidateCodeRequest request);
		Task<ApiResponse<RequestInviteResponse>> RequestInvite(RequestInviteRequest request);
		Task<ApiResponse<LoginResponse>> Login(LoginRequest request);
		Task<ApiResponse<LogoutResponse>> Logout(LogoutRequest request);
		Task<ApiResponse<GetCategoriesResponse>> GetCategories(GetCategoriesRequest request);
		Task<ApiResponse<GetMessageConfigResponse>> GetMessageConfig(GetMessageConfigRequest request);
		Task<ApiResponse<GetImpactResponse>> GetImpact(GetImpactRequest request);
		Task<ApiResponse<GetInvitesResponse>> GetInvites(GetInvitesRequest request);
		Task<ApiResponse<GetInviteBundlesResponse>> GetInviteBundles(GetInviteBundlesRequest request);
		Task<ApiResponse<PurchaseInviteResponse>> PurchaseInvite(PurchaseInviteRequest request);
		Task<ApiResponse<GetMessagesResponse>> GetMessages(GetMessagesRequest request);
		Task<ApiResponse<GetMessageResponse>> GetMessage(GetMessageRequest request);
		Task<ApiResponse<ForgotPasswordResponse>> ForgotPassword(ForgotPasswordRequest request);
		Task<ApiResponse<RegisterUserResponse>> RegisterUser(RegisterUserRequest request);
		Task<ApiResponse<SaveCategoriesResponse>> SaveCategories(SaveCategoriesRequest request);
		Task<ApiResponse<SetMessagesConfigResponse>> SetMessagesConfigUser(SetMessagesConfigRequest request);
	}
}
