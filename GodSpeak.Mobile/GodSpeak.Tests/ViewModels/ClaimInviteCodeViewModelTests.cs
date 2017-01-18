using System;
using FakeItEasy;
using NUnit.Framework;
using Should;
using System.Threading.Tasks;
using MvvmCross.Test.Core;
using Ploeh.AutoFixture;

namespace GodSpeak.Tests.ViewModels
{
    public class ClaimInviteCodeViewModelTests : BaseViewModelTest
    {

        ClaimInviteCodeViewModel ViewModelUT;

        WelcomeViewModel FakeWelcomeVM = A.Fake<WelcomeViewModel> ();

        IDialogService FakeDialogService = A.Fake<IDialogService> ();

        IWebApiService FakeWebApiService = A.Fake<IWebApiService> ();

        [SetUp]
        public void Init ()
        {

            FakeDialogService = A.Fake<IDialogService> ();
            ViewModelUT = new ClaimInviteCodeViewModel (FakeWelcomeVM, FakeDialogService, FakeWebApiService);
        }
        /// <summary>
        /// ClaimInviteCodeCommand Tests
        /// </summary>

        [Test]
        public void if_invite_code_null_or_empty_DialogService_ShowAlert_SHOULD_BE_called ()
        {

            const string expectedErrorTitle = "Error";
            const string expectedErrorMessage = "Sorry, you can't claim a empty invite code.";

            ViewModelUT.InviteCode = string.Empty;
            ViewModelUT.ClaimInviteCodeCommand.Execute ();

            A.CallTo (() => FakeDialogService.ShowAlert (expectedErrorTitle, expectedErrorMessage)).MustHaveHappened ();

            ViewModelUT.InviteCode = null;
            ViewModelUT.ClaimInviteCodeCommand.Execute ();
            A.CallTo (() => FakeDialogService.ShowAlert (expectedErrorTitle, expectedErrorMessage)).MustHaveHappened ();
        }

        [Test]
        public void if_invite_code_null_or_empty_WebApiService_ValidateCode_SHOULD_NOT_be_called ()
        {

            ViewModelUT.InviteCode = string.Empty;
            ViewModelUT.ClaimInviteCodeCommand.Execute ();
            A.CallTo (() => FakeWebApiService.ValidateCode (A<ValidateCodeRequest>.Ignored)).MustNotHaveHappened ();

            ViewModelUT.InviteCode = null;
            ViewModelUT.ClaimInviteCodeCommand.Execute ();
            A.CallTo (() => FakeWebApiService.ValidateCode (A<ValidateCodeRequest>.Ignored)).MustNotHaveHappened ();
        }

        [Test]
        public void if_WebApiService_ValidateCode_returns_success_ShowViewModel_RegisterViewModel_should_be_invoked ()
        {

            //Arrangee
            var expectedCode = DataFixture.Create<string> ();
            A.CallTo (() => FakeWebApiService.ValidateCode (A<ValidateCodeRequest>.That.Matches (req => req.Code == expectedCode))).Returns (Task.FromResult (new BaseResponse<ValidateCodeResponse> () { StatusCode = System.Net.HttpStatusCode.OK }));
            ViewModelUT.InviteCode = expectedCode;

            //Act
            ViewModelUT.ClaimInviteCodeCommand.Execute ();

            //Assert
            ShouldShowVM<RegisterViewModel> ();

        }

        [Test]
        public void if_WebApiService_ValidateCode_returns_bad_request_ShowViewModel_SHOULD_NOT_BE_invoked ()
        {
            var expectedCode = DataFixture.Create<string> ();
            A.CallTo (() => FakeWebApiService.ValidateCode (A<ValidateCodeRequest>.That.Matches (req => req.Code == expectedCode))).Returns (Task.FromResult (new BaseResponse<ValidateCodeResponse> () { StatusCode = System.Net.HttpStatusCode.BadRequest }));
            ViewModelUT.InviteCode = expectedCode;

            //Act
            ViewModelUT.ClaimInviteCodeCommand.Execute ();

            //Assert
            ShouldNotShowVM<RegisterViewModel> ();
        }
    }
}
