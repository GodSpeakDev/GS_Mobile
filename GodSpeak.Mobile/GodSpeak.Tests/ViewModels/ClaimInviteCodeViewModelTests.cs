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
        const string GetACode = "Get a Code";
        const string TryAgain = "Try Again";

        ClaimInviteCodeViewModel ViewModelUT;

        WelcomeViewModel FakeWelcomeVM;

        IDialogService FakeDialogService;
        IWebApiService FakeWebApiService;

        [SetUp]
        public void Init ()
        {

            FakeWelcomeVM = A.Fake<WelcomeViewModel> ();
            FakeDialogService = A.Fake<IDialogService> ();
            FakeWebApiService = A.Fake<IWebApiService> ();

            ViewModelUT = new ClaimInviteCodeViewModel (FakeWelcomeVM, FakeDialogService, FakeWebApiService);
        }


        [Test]
        public void bindable_props_SHOULD_dispatch_change_events ()
        {
            PropShouldDispatchChange (ViewModelUT, "InviteCode", () => ViewModelUT.InviteCode = DataFixture.Create<string> ());
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
            WebApiValidateWillReturn (new BaseResponse<ValidateCodeResponse> () { StatusCode = System.Net.HttpStatusCode.OK });
            //Act
            ViewModelUT.ClaimInviteCodeCommand.Execute ();

            //Assert
            ShouldShowVM<RegisterViewModel> ();

        }

        [Test]
        public void if_WebApiService_ValidateCode_returns_bad_request_ShowViewModel_SHOULD_NOT_BE_invoked ()
        {
            //Arrange
            WebApiValidateWillReturn (new BaseResponse<ValidateCodeResponse> () { StatusCode = System.Net.HttpStatusCode.BadRequest });

            //Act
            ViewModelUT.ClaimInviteCodeCommand.Execute ();

            //Assert
            ShouldNotShowVM<RegisterViewModel> ();
        }

        [Test]
        public void if_WebApiService_ValidateCode_returns_bad_request_DialogService_ShowConfirmation_SHOULD_BE_invoked ()
        {
            //Arrange
            var badResponse = new BaseResponse<ValidateCodeResponse> () { StatusCode = System.Net.HttpStatusCode.BadRequest, ErrorTitle = DataFixture.Create<string> (), ErrorMessage = DataFixture.Create<string> () };
            WebApiValidateWillReturn (badResponse);

            //Act
            ViewModelUT.ClaimInviteCodeCommand.Execute ();

            //Assert
            A.CallTo (() => FakeDialogService.ShowConfirmation (badResponse.ErrorTitle, badResponse.ErrorMessage, GetACode, TryAgain));
        }


        [Test]
        public void if_ShowConfirmation_returns_True_WelcomeVM_SelectPage_SHOULD_BE_invoked ()
        {
            //Arrange
            var badResponse = new BaseResponse<ValidateCodeResponse> () { StatusCode = System.Net.HttpStatusCode.BadRequest, ErrorTitle = DataFixture.Create<string> (), ErrorMessage = DataFixture.Create<string> () };
            A.CallTo (() => FakeDialogService.ShowConfirmation (badResponse.ErrorTitle, badResponse.ErrorMessage, GetACode, TryAgain)).Returns (Task.FromResult (true));

            WebApiValidateWillReturn (badResponse);

            //Act
            ViewModelUT.ClaimInviteCodeCommand.Execute ();


            //Assert
            A.CallTo (() => FakeWelcomeVM.SelectPage<RequestInviteCodeViewModel> ()).MustHaveHappened ();
        }


        [Test]
        public void if_ShowConfirmation_returns_False_WelcomeVM_SelectPage_SHOULD_NOT_BE_invoked ()
        {
            //Arrange
            var badResponse = new BaseResponse<ValidateCodeResponse> () { StatusCode = System.Net.HttpStatusCode.BadRequest, ErrorTitle = DataFixture.Create<string> (), ErrorMessage = DataFixture.Create<string> () };
            A.CallTo (() => FakeDialogService.ShowConfirmation (badResponse.ErrorTitle, badResponse.ErrorMessage, GetACode, TryAgain)).Returns (Task.FromResult (false));

            WebApiValidateWillReturn (badResponse);

            //Act
            ViewModelUT.ClaimInviteCodeCommand.Execute ();


            //Assert
            A.CallTo (() => FakeWelcomeVM.SelectPage<RequestInviteCodeViewModel> ()).MustNotHaveHappened ();
        }

        [Test]
        public void DontHaveCodeCommand_SHOULD_invoke_SelectPage_on_WelcomeVM ()
        {
            //Act
            ViewModelUT.DontHaveCodeCommand.Execute ();

            //Assert
            A.CallTo (() => FakeWelcomeVM.SelectPage<RequestInviteCodeViewModel> ()).MustHaveHappened ();
        }

        [Test]
        public void AlreadyRegisteredCommand_SHOULD_ShowViewModel_LoginViewModel ()
        {
            //Act
            ViewModelUT.AlreadyRegisteredCommand.Execute ();

            //Assert
            ShouldShowVM<LoginViewModel> ();


        }

        /// <summary>
        /// Helper Method for setting up Fake Web Api
        /// </summary>
        /// <param name="response">Response.</param>
        void WebApiValidateWillReturn (BaseResponse<ValidateCodeResponse> response)
        {
            var expectedCode = DataFixture.Create<string> ();

            A.CallTo (() => FakeWebApiService.ValidateCode (A<ValidateCodeRequest>.That.Matches (req => req.Code == expectedCode))).Returns (Task.FromResult (response));
            ViewModelUT.InviteCode = expectedCode;


        }
    }
}
