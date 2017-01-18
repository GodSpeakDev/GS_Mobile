using System;
using System.Collections.Generic;
using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;
using Should;
using System.Linq;
using MvvmCross.Platform;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace GodSpeak.Tests.ViewModels
{
    public class BaseViewModelTest : MvxIoCSupportingTest
    {
        protected MockDispatcher MockDispatcher;

        protected Fixture DataFixture = new Fixture ();

        [SetUp]
        public void InitMvxSupport ()
        {
            base.Setup ();//MUST CALL THIS TO SETUP MVX TEST SUPPORT
        }

        protected override void AdditionalSetup ()
        {
            MockDispatcher = new MockDispatcher ();
            Mvx.RegisterSingleton<IMvxViewDispatcher> (MockDispatcher);
            Mvx.RegisterSingleton<IMvxMainThreadDispatcher> (MockDispatcher);

            // for navigation parsing
            Mvx.RegisterSingleton<IMvxStringToTypeParser> (new MvxStringToTypeParser ());

        }

        protected void ShouldShowVM<T> () where T : MvxViewModel
        {
            MockDispatcher.Requests.Any (req => req.ViewModelType == typeof (T)).ShouldBeTrue ();
        }

        protected void ShouldNotShowVM<T> () where T : MvxViewModel
        {
            MockDispatcher.Requests.Any (req => req.ViewModelType == typeof (T)).ShouldBeFalse ();
        }
    }

    public class MockDispatcher
        : MvxMainThreadDispatcher
    , IMvxViewDispatcher
    {
        public readonly List<MvxViewModelRequest> Requests = new List<MvxViewModelRequest> ();
        public readonly List<MvxPresentationHint> Hints = new List<MvxPresentationHint> ();

        public bool RequestMainThreadAction (Action action)
        {
            action ();
            return true;
        }

        public bool ShowViewModel (MvxViewModelRequest request)
        {
            Requests.Add (request);
            return true;
        }

        public bool ChangePresentation (MvxPresentationHint hint)
        {
            Hints.Add (hint);
            return true;
        }
    }
}