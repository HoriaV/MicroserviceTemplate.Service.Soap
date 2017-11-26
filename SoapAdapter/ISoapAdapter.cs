using MicroserviceTemplate.Service.Models.Request;
using Nancy;

namespace MicroserviceTemplate.Service.SoapAdapter
{
    public interface ISoapAdapter
    {
        void SoapRouteRedirect(NancyContext ctx, NancyRequest nancyRequest);
    }
}
