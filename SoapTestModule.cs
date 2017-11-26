using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Nancy.ModelBinding;

namespace MicroserviceTemplate.Service
{
    public class SoapTestModule : NancyModule
    {
        public SoapTestModule()
        {
            Post["/api/service/ExtractData", runAsync: true] = async (ctx, ct) => await DoTheWork();

        }

        private async Task<dynamic> DoTheWork()
        {
            var model = this.Bind<Models.Request.Envelope>();
            var result = await DoTheWorkAsync();
            return result;
        }

        private dynamic DoTheWorkAsync()
        {
            return new
            {
                MicroserviceName = "Test",
                Version = "0.0.1"
            };
        }
    }
}