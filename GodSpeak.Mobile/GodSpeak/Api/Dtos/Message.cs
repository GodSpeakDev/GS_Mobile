using System;
using GodSpeak.Api.Dtos;
namespace GodSpeak
{
    public class Message
    {
        public Guid MessageId {
            get;
            set;
        }

        public DateTime DateTimeToDisplay {
            get;
            set;
        }

        public Verse PreviousVerse {
            get;
            set;
        }

        public Verse Verse {
            get;
            set;
        }

        public Verse NextVerse {
            get;
            set;
        }
    }
}
