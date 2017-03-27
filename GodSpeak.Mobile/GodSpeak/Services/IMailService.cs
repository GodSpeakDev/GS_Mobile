using System;
using System.Threading.Tasks;

namespace GodSpeak
{
	public interface IMailService
	{
		void SendMail(string to, string[] cc = null, string[] bcc = null, string subject = null, string body = null);
	}
}
