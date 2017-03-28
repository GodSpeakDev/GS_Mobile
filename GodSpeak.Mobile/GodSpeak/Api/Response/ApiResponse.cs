using System;
using System.Net;

namespace GodSpeak
{
    public class ApiResponse
    {
        private HttpStatusCode statusCode;
        public HttpStatusCode StatusCode {
            get { return statusCode; }
            set {
                statusCode = value;
            }
        }

        public bool IsSuccess {
            get {
                return StatusCode == HttpStatusCode.OK;
            }
        }

        public string Message {
            get;
            set;
        }

        public string Title {
            get;
            set;
        }
    }

    public class ApiResponse<TEntity>
    {
        private HttpStatusCode statusCode;
        public HttpStatusCode StatusCode {
            get { return statusCode; }
            set {
                statusCode = value;
            }
        }

        public bool IsSuccess {
            get {
                return StatusCode == HttpStatusCode.OK;
            }
        }

        public string Message {
            get;
            set;
        }

        public string Title {
            get;
            set;
        }

        public TEntity Payload {
            get;
            set;
        }
    }
}
