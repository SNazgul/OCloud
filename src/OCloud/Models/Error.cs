using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace OCloud.Models
{
    public class HttpError
    {
        public HttpError(HttpStatusCode code, params string[] descriptions)
        {
            Code = code;
            Descriptions = ImmutableList.Create<string>(descriptions);
        }

        HttpStatusCode Code { get; }

        IImmutableList<string> Descriptions { get; }
    }
}
