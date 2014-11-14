using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Spring.Context.Support;
using Spring.Context;
using Spring.Objects.Factory.Xml;
using MvcContrib.Services;
using Spring.Objects.Factory;
using Spring.Core.IO;
using MvcContrib.ControllerFactories;
using System.Configuration;
using MvcContrib.Spring;
using Compelligence.BusinessLogic.Interface;
using System.Web.Configuration;
using Compelligence.Web.Controllers;

namespace Compelligence.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             "Root",
             "",
             new { controller = "Home", action = "Index", id = "" }
           );

            //routes.MapRoute(
            //    "DefaultIndex",
            //    "{controller}.aspx/{id}",
            //    new { action = "Index", id = "" }
            //  );

            routes.MapRoute(
                "Default",
                "{controller}.aspx/{action}/{id}",
                new { action = "Index", id = "" }
              );
        }

        //public static void RegisterRoutes(RouteCollection routes)
        //{
        //    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        //    routes.MapRoute(
        //        "Default",                                              // Route name
        //        "{controller}/{action}/{id}",                           // URL with parameters
        //        new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
        //    );

        //}

        protected void Application_Start()
        {
            ConfigureIoC();

            RegisterRoutes(RouteTable.Routes);

            LoadDataInCache();
        }

        private void ConfigureIoC()
        {
            WebApplicationContext webApplicationContext = ContextRegistry.GetContext() as WebApplicationContext;
            DependencyResolver.InitializeWith(new SpringDependencyResolver(webApplicationContext.ObjectFactory));
            ControllerBuilder.Current.SetControllerFactory(typeof(IoCControllerFactory));
        }

        private void LoadDataInCache()
        {
            WebApplicationContext webApplicationContext = ContextRegistry.GetContext() as WebApplicationContext;

            IApplicationConfigurationService applicationConfigurationService = webApplicationContext.GetObject("ApplicationConfigurationService") as IApplicationConfigurationService;
            IClientCompanyConfigurationService clientCompanyConfigurationService = webApplicationContext.GetObject("ClientCompanyConfigurationService") as IClientCompanyConfigurationService;
            IResourceService resourceService = webApplicationContext.GetObject("ResourceService") as IResourceService;

            applicationConfigurationService.LoadAllConfigurationInCache();

            clientCompanyConfigurationService.LoadAllConfigurationInCache();

            resourceService.LoadAllInCache();
        }

        //private void Application_Error(object sender, EventArgs e)
        //{
        //    if (GlobalHelper.IsMaxRequestExceededEexception(this.Server.GetLastError()))
        //    {
        //        this.Server.ClearError();
        //        this.Server.Transfer("~/error/UploadTooLarge.aspx");
        //    }
        //}

        void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            string errorN = ex.Message;

            if (errorN.Equals("Se produjo una excepción de tipo 'System.Web.HttpUnhandledException'.") || errorN.Equals("Could not open Hibernate Session for transaction") || errorN.Equals("Could not roll back Hibernate transaction"))
            {
                if (errorN.Equals("Se produjo una excepción de tipo 'System.Web.HttpUnhandledException'."))
                {
                    ConfigureIoC();
                    RegisterRoutes(RouteTable.Routes);
                    LoadDataInCache();
                }
                var httpContext = ((MvcApplication)sender).Context;
                var currentController = " ";
                var currentAction = " ";
                var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

                if (currentRouteData != null)
                {
                    if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                    {
                        currentController = currentRouteData.Values["controller"].ToString();
                    }

                    if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                    {
                        currentAction = currentRouteData.Values["action"].ToString();
                    }
                }


                var controller = new CustomErrorController();
                var routeData = new RouteData();
                var action = "Index";

                httpContext.ClearError();
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
                httpContext.Response.TrySkipIisCustomErrors = true;

                routeData.Values["controller"] = "CustomError";
                routeData.Values["action"] = action;
                routeData.Values["exception"] = ex;
                routeData.Values["currentController"] = currentController;
                routeData.Values["currentAction"] =currentAction;

                controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
                ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            }
            /*
            Exception exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Errors";
            routeData.Values["action"] = "General";
            routeData.Values["exception"] = exception;
            Response.StatusCode = 500;
            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode)
                {
                    case 403:
                        routeData.Values["action"] = "Http403";
                        break;
                    case 404:
                        routeData.Values["action"] = "Http404";
                        break;
                }
            }
            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;
            IController errorsController = new FileController();
            HttpContextWrapper wrapper = new HttpContextWrapper(Context);
            var rc = new RequestContext(wrapper, routeData);
            errorsController.Execute(rc);*/
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {

            switch (Request.Url.Scheme)
            {
                case "https":
                    Response.AddHeader("Strict-Transport-Security", "max-age=300");
                    break;
                case "http":
                    if (!Request.Url.AbsoluteUri.Contains("localhost"))
                    {
                        var path = "https://" + Request.Url.Host + Request.Url.PathAndQuery;
                        Response.Status = "301 Moved Permanently";
                        Response.AddHeader("Location", path);
                    }
                    break;
            }
            //Burp Issue::Frameable response (potential Clickjacking)
            //Response.AddHeader("X-FRAME-OPTIONS", "DENY"); //Disable because this option disable call from SFDC
            //Burp Issue::Cacheable HTTPS response
            Response.AddHeader("Cache-control", "no-store");
            Response.AddHeader("Pragma", "no-cache");

            HttpRuntimeSection runTime = (HttpRuntimeSection)WebConfigurationManager.GetSection("system.web/httpRuntime");

            //Approx 100 Kb(for page content) size has been deducted because the maxRequestLength proprty is the page size, not only the file upload size
            int maxRequestLength = (runTime.MaxRequestLength - 100) * 1024;
            //This code is used to check the request length of the page and if the request length is greater than
            //MaxRequestLength then retrun to the same page with extra query string value action=exception

            HttpContext context = ((HttpApplication)sender).Context;
            if (context.Request.ContentLength > maxRequestLength)
            {

                IServiceProvider provider = (IServiceProvider)context;
                HttpWorkerRequest workerRequest = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));
                // Check if body contains data

                if (workerRequest.HasEntityBody())
                {

                    // get the total body length
                    int requestLength = workerRequest.GetTotalEntityBodyLength();

                    // Get the initial bytes loaded
                    int initialBytes = 0;

                    if (workerRequest.GetPreloadedEntityBody() != null)
                        initialBytes = workerRequest.GetPreloadedEntityBody().Length;

                    if (!workerRequest.IsEntireEntityBodyIsPreloaded())
                    {
                        byte[] buffer = new byte[512000];

                        // Set the received bytes to initial bytes before start reading
                        int receivedBytes = initialBytes;

                        while (requestLength - receivedBytes >= initialBytes)
                        {
                            // Read another set of bytes
                            initialBytes = workerRequest.ReadEntityBody(buffer, buffer.Length);
                            // Update the received bytes
                            receivedBytes += initialBytes;
                        }
                        initialBytes = workerRequest.ReadEntityBody(buffer, requestLength - receivedBytes);
                    }

                }

            }

        }
 

    }
}