using System;

namespace GodSpeak.Services
{
    public interface IProgressHudService
    {
        void Show (string message = null);
        void Hide ();
    }
}
