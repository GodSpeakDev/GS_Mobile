using System;
using FakeItEasy;
using NUnit.Framework;
using Should;

namespace GodSpeak.Tests.ViewModles
{
    public class ClaimInviteCodeViewModelTests
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

        //ClaimInviteCodeCommand Tests
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
    }
}
