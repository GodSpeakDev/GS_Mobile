using System;
using System.Collections.Generic;

namespace GodSpeak
{
    public class User
    {
        public User ()
        {
            MessageCategorySettings = new List<MessageCategory> ();
            MessageDayOfWeekSettings = new List<DayOfWeekSettings> ();
        }

        public string Token {
            get;
            set;
        }

        public Guid Id {
            get;
            set;
        }

        public int InviteBalance {
            get;
            set;
        }

        public string InviteCode {
            get;
            set;
        }

        public string FirstName {
            get;
            set;
        }

        public string LastName {
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

        public string Email {
            get;
            set;
        }

        public string PhotoUrl {
            get;
            set;
        }

        public List<MessageCategory> MessageCategorySettings {
            get;
            set;
        }

        public List<DayOfWeekSettings> MessageDayOfWeekSettings {
            get;
            set;
        }
        public string CurrentPassword { get; internal set; }
        public string NewPassword { get; internal set; }
        public string PasswordConfirm { get; internal set; }
    }
}
