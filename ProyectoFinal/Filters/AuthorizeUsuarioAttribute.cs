using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Filters
{
    public class AuthorizeUsuarioAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                ITempDataProvider provider= context.HttpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;
                var TempData = provider.LoadTempData(context.HttpContext);
                String action = context.RouteData.Values["action"].ToString();
                string controller = context.RouteData.Values["controller"].ToString();
                TempData["action"] = action;
                TempData["controller"] = controller;
                provider.SaveTempData(context.HttpContext, TempData);
                context.Result = this.GetRedirectToRoute("Usuario", "Login");
            }
        }

        private RedirectToRouteResult GetRedirectToRoute(String controller,String action)
        {
            RouteValueDictionary ruta = new RouteValueDictionary(new
            {
                controller = controller,
                action = action
            });
            RedirectToRouteResult redirect = new RedirectToRouteResult(ruta);
            return redirect;
        }
    }
}
