using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http.Filters;
using UserManagement.Domain.Core.Exceptions;
using UserManagement.WebApi.Extensions;

namespace UserManagement.WebApi.Handlers
{
    public class ModuleActionExceptionHandler
    {
        public bool ExceptionHandled { get; set; }

        public static void HandleException(HttpContext filterContext)
        {
            var exceptionHandlerPathFeature =
                filterContext.Features.Get<IExceptionHandlerPathFeature>();

            var exception = exceptionHandlerPathFeature.Error;
            if (exception == null) return;
            try
            {
                //log the exception
            }
            catch (Exception)
            {
                //log exception to file if smth's wrong
            }

            ServiceResult result;
            if (exception is UserManagementException userManagementException)
            {
                result = new ServiceResult
                {
                    State = ServiceResultStates.ERROR,
                    Message = userManagementException.BusinessMessage + ":" + exception.Message
                };
                filterContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                filterContext.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
                filterContext.Response.WriteAsync(JsonConvert.SerializeObject(result));
            }
            else
            {
                result = new ServiceResult
                {
                    State = ServiceResultStates.ERROR,
                    Message = "An error occured during process" + ":" + exception.Message
                };

                filterContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                filterContext.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
                filterContext.Response.WriteAsync(JsonConvert.SerializeObject(result));

            }
        }
    }
}
