using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Models.Base
{
    public class CqrsResponse
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
