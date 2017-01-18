using System;
using FakeItEasy;
using NUnit.Framework;
using Should;
using System.Threading.Tasks;
using MvvmCross.Test.Core;

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
        public void if_submitted_invite_code_and_WebApiService_ValidateCode_returns_success_ShowViewModel_RegisterViewModel_should_be_invoked ()
        {

            //Arrangee
            var expectedCode = "1234adf";
            A.CallTo (() => FakeWebApiService.ValidateCode (A<ValidateCodeRequest>.That.Matches (req => req.Code == expectedCode))).Returns (Task.FromResult (new BaseResponse<ValidateCodeResponse> () { StatusCode = System.Net.HttpStatusCode.OK }));
            ViewModelUT.InviteCode = expectedCode;

            //Act
            ViewModelUT.ClaimInviteCodeCommand.Execute ();

            //Assert
            ShouldShowVM<RegisterViewModel> ();

        }
    }
}
