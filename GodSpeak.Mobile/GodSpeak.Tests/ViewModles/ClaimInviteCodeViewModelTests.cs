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
        public void if_invite_code_null_or_empty_DialogService_ShowAlert_called ()
        {
            ViewModelUT.InviteCode = string.Empty;
            ViewModelUT.ClaimInviteCodeCommand.Execute ();


            A.CallTo (() => FakeDialogService.ShowAlert ("Error", "Please, you can't claim a empty invite code.")).MustHaveHappened ();
        }
    }
}
