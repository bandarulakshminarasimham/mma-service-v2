using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Security.Principal;
using MeetingService.Data;
using Autofac.Core;
using System.Web.Mvc;
using System.Threading.Tasks;
using MeetingService.Data.Entities;
using System.Web.Http;
using Autofac.Integration.WebApi;


namespace MeetingService.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BasicAuthenticationFilter : AuthorizationFilterAttribute
    {

        bool Active = true;
        public BasicAuthenticationFilter()
        { }
        public BasicAuthenticationFilter(bool active)
        {
            Active = active;
        }
        //public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        //{
        //    if (Active)
        //    {
        //        var identity = ParseAuthorizationHeader(actionContext);
        //        if (identity == null)
        //        {
        //            Challenge(actionContext);
        //            return;
        //        }

        //        if (!OnAuthorizeUser(identity.Name, identity.Password, actionContext))
        //        {
        //            Challenge(actionContext);
        //            return;
        //        }

        //        var principal = new GenericPrincipal(identity, null);

        //        if (HttpContext.Current != null)
        //        {
        //            HttpContext.Current.User = principal;
        //        }
        //        base.OnAuthorization(actionContext);
        //    }
        //}

        protected virtual bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            // we can write db validations here
            bool status = false;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return status;

            using (MeetingManagementEntities db = new MeetingManagementEntities())
            {
                status = db.Users.Any(t => t.Username == username);
            }
            return status;

        }
        
        protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext)
        {
            string authHeader = null;
            var auth = actionContext.Request.Headers.Authorization;
            if (auth != null && auth.Scheme == "Basic")
                authHeader = auth.Parameter;

            if (string.IsNullOrEmpty(authHeader))
                return null;

            authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));
            var tokens = authHeader.Split(':');

            if (tokens.Length < 2)
                return null;

            return new BasicAuthenticationIdentity(tokens[0], tokens[1]);

        }
        
        private void Challenge(HttpActionContext actionContext)
        {
            var host = actionContext.Request.RequestUri.DnsSafeHost;
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized User");
            actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
        }


        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (Active)
            {
                var identity = ParseAuthorizationHeader(actionContext);
                if (identity == null)
                {
                    Challenge(actionContext);
                    return Task.FromResult<object>(null);
                }

                if (!OnAuthorizeUser(identity.Name, identity.Password, actionContext))
                {
                    Challenge(actionContext);
                    return Task.FromResult<object>(null);
                }

                var principal = new GenericPrincipal(identity, null);

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }
                base.OnAuthorizationAsync(actionContext, cancellationToken);
            }
            return Task.FromResult<object>(null);
        }
    }
}