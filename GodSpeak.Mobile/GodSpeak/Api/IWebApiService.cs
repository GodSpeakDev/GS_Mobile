﻿using System;
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
        Task<ApiResponse> Logout (LogoutRequest request);
        Task<ApiResponse<User>> GetProfile (TokenRequest request);
        Task<ApiResponse<User>> SaveProfile (User user);

        Task<ApiResponse<GetImpactResponse>> GetImpact (GetImpactRequest request);
        Task<ApiResponse<GetInvitesResponse>> GetInvites (GetInvitesRequest request);
        Task<ApiResponse<List<InviteBundle>>> GetInviteBundles (GetInviteBundlesRequest request);
        Task<ApiResponse<string>> PurchaseInvite (PurchaseInviteRequest request);
        Task<ApiResponse<List<Message>>> GetMessages (TokenRequest request);
        Task<ApiResponse<GetMessageResponse>> GetMessage (GetMessageRequest request);
        Task<ApiResponse<string>> ForgotPassword (ForgotPasswordRequest request);
        Task<ApiResponse<User>> RegisterUser (RegisterUserRequest request);

        Task<ApiResponse<List<AcceptedInvite>>> GetAcceptedInvites (TokenRequest request);
        Task<ApiResponse<string>> DonateInvite (TokenRequest request);
        Task<ApiResponse<string>> GetDidYouKnow (TokenRequest request);

        Task<ApiResponse<User>> UploadPhoto (UploadPhotoRequest request);
    }
}
