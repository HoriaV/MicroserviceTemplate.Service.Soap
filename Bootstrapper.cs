﻿using Autofac;
using MicroserviceTemplate.Service.Logging;
using MicroserviceTemplate.Service.Models.Request;
using MicroserviceTemplate.Service.SoapAdapter;
using MicroserviceTemplate.Service.Utilities;
using MicroserviceTemplate.Service.Utilities.Configuration;
using MicroserviceTemplate.Service.Utilities.Pipelines;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;

namespace MicroserviceTemplate.Service
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during application startup.

            Nancy.Json.JsonSettings.RetainCasing = true;
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(config => config.RouteResolver = typeof(SoapRouteResolver));
            }
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            // Perform registration that should have an application lifetime

            container.Update(builder =>
            {
                builder.RegisterType<ConfigurationManager>()
                    .As<IConfigurationManager>()
                    .SingleInstance();

                builder.RegisterType<PipelineHelper>()
                    .As<IPipelineHelper>()
                    .UsingConstructor(typeof(IConfigurationManager), typeof(INLoggerFactory), typeof(ICorrelationId));

                builder.RegisterType<NLogger>().As<INLogger>();

                builder.RegisterType<SoapAdapter.SoapAdapter>().As<ISoapAdapter>();

                builder.RegisterType<NLoggerFactory>()
                    .As<INLoggerFactory>();
            });
        }

        protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
        {
            // Perform registrations that should have a request lifetime

            container.Update(builder =>
            {
                builder.RegisterType<CorrelationId>()
                    .As<ICorrelationId>()
                    .InstancePerLifetimeScope();
            });
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during request startup.

            var _configurationManager = container.Resolve<IConfigurationManager>();
            var _correlationId = container.Resolve<ICorrelationId>();
            var _pipelineHelper = container.Resolve<IPipelineHelper>();
            var _soapAdapter = container.Resolve<ISoapAdapter>();

            pipelines.BeforeRequest += (ctx) =>
            {
                if (ctx.Request.Path.StartsWith(_configurationManager.Instance.ApiPrefix))
                {
                    var requestObject = new NancyRequest(ctx.Request.Method, ctx.Request.Url, ctx.Request.Query, Utils.BodyXmlToJObject(ctx.Request.Body));
                    _correlationId.CurrentValue = _pipelineHelper.GetCorrelationId(requestObject);
                    _pipelineHelper.LogRequest(requestObject);
                    _soapAdapter.SoapRouteRedirect(ctx, requestObject);
                    ctx.Request.Body.Position = 0;
                }
                return null;
            };

            pipelines.AfterRequest += (ctx) =>
            {
                if (ctx.Request.Path.StartsWith(_configurationManager.Instance.ApiPrefix) && ctx.Response.ContentType.Contains("json"))
                {
                    _pipelineHelper.LogAndFormatResponse(ctx.Response);
                }
            };

            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                _pipelineHelper.LogError(ex);
                var error = _pipelineHelper.BuildErrorResponse(ex.Message);
                error.StatusCode = HttpStatusCode.BadRequest;
                return error;
            });

            base.RequestStartup(container, pipelines, context);
        }
    }
}