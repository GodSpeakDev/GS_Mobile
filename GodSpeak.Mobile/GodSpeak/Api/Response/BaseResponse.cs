using System;
using System.Net;

namespace GodSpeak
{
	public class BaseResponse<TEntity>
	{
		private HttpStatusCode statusCode;
		public HttpStatusCode StatusCode
		{
			get { return statusCode; }
			set
			{
				statusCode = value;
			}
		}

		public bool IsSuccess
		{
			get
			{
				return StatusCode == HttpStatusCode.OK;
			}
		}

		public string ErrorMessage
		{
			get;
			set;
		}

		public string ErrorTitle
		{
			get;
			set;
		}

		public TEntity Content
		{
			get;
			set;
		}
	}
}
