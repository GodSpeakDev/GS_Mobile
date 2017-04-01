using System;
using GodSpeak.Resources;

namespace GodSpeak
{
    public class InviteBundle
    {
        public Guid InviteBundleId {
            get;
            set;
        }

        public int NumberOfInvites {
            get;
            set;
        }

        public string AppStoreSku {
            get;
            set;
        }

        public string PlayStoreSku {
            get;
            set;
        }

        public decimal Cost {
            get;
            set;
        }

		public string CostDescription
		{
			get
			{
				return string.Format("$ {0}", Cost);
			}
		}

        public string Description {
            get {
                return string.Format (Text.InviteBundleDescription, NumberOfInvites);
            }
        }
    }
}
