using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using MicroserviceTemplate.Service.Logging;
using MicroserviceTemplate.Service.Models.Request;
using MicroserviceTemplate.Service.Utilities;

namespace MicroserviceTemplate.Service.SoapAdapter
{
    public class SoapAdapter : ISoapAdapter
    {
        private INLogger _logger;
        public SoapAdapter(INLoggerFactory factory)
        {
            _logger = factory.CreateLogger<SoapAdapter>();
        }

        public void SoapRouteRedirect(NancyContext ctx, NancyRequest nancyRequest)
        {
            if (ctx.Request.Headers["SOAPAction"].Any())
            {
                var matchingHeaders = ctx.Request.Headers["SOAPAction"];
                var headerValue = (matchingHeaders == null) ? "" : (matchingHeaders.FirstOrDefault() ?? "");
                if (!string.IsNullOrEmpty(headerValue))
                {
                    // Strip the namespace and double quotes from the soap action
                    int nsSplit = headerValue.LastIndexOf('/');
                    if (nsSplit >= 0)
                        headerValue = headerValue.Substring(nsSplit + 1);
                    headerValue = headerValue.Trim("\"".ToCharArray());

                    // Set the new action
                    if (ctx.Items.ContainsKey(Utils.SoapActionKey))
                    {
                        ctx.Items.Remove(Utils.SoapActionKey);
                    }
                    ctx.Items.Add(new KeyValuePair<string, object>(Utils.SoapActionKey, headerValue));
                }
            }
        }
    }
}