using System;
using System.Threading.Tasks;

namespace GodSpeak
{
	public interface IWebApiService
	{
		Task<BaseResponse<ValidateCodeResponse>> ValidateCode(ValidateCodeRequest request);
		Task<BaseResponse<RequestInviteResponse>> RequestInvite(RequestInviteRequest request);
		Task<BaseResponse<LoginResponse>> Login(LoginRequest request);
		Task<BaseResponse<LogoutResponse>> Logout(LogoutRequest request);
		Task<BaseResponse<GetCategoriesResponse>> GetCategories(GetCategoriesRequest request);
		Task<BaseResponse<GetMessageConfigResponse>> GetMessageConfig(GetMessageConfigRequest request);
		Task<BaseResponse<GetImpactResponse>> GetImpact(GetImpactRequest request);
		Task<BaseResponse<GetInvitesResponse>> GetInvites(GetInvitesRequest request);
		Task<BaseResponse<GetInviteBundlesResponse>> GetInviteBundles(GetInviteBundlesRequest request);
		Task<BaseResponse<PurchaseInviteResponse>> PurchaseInvite(PurchaseInviteRequest request);
		Task<BaseResponse<GetMessagesResponse>> GetMessages(GetMessagesRequest request);
		Task<BaseResponse<GetMessageResponse>> GetMessage(GetMessageRequest request);
		Task<BaseResponse<ForgotPasswordResponse>> ForgotPassword(ForgotPasswordRequest request);
		Task<BaseResponse<RegisterUserResponse>> RegisterUser(RegisterUserRequest request);
		Task<BaseResponse<SaveCategoriesResponse>> SaveCategories(SaveCategoriesRequest request);
		Task<BaseResponse<SetMessagesConfigResponse>> SetMessagesConfigUser(SetMessagesConfigRequest request);
	}
}
