using System;
namespace GodSpeak
{
    public class RegisterUserRequest
    {

        public string FirstName {
            get;
            set;
        }

        public string LastName {
            get;
            set;
        }

        public string EmailAddress {
            get;
            set;
        }

        public string CountryCode {
            get;
            set;
        }

        public string PostalCode {
            get;
            set;
        }

        public string Password {
            get;
            set;
        }

        public string PasswordConfirm { get; set; }

        public string InviteCode {
            get;
            set;
        }

        public byte [] ProfilePhoto {
            get;
            set;
        }
    }
}
