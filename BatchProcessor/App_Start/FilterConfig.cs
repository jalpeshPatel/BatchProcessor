using System.Web;
using System.Web.Mvc;

namespace BatchProcessor
{
    public class UserSessionActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContextORG)
        {
            HttpContext ctx = HttpContext.Current;
            string controller = (filterContextORG.RouteData.Values["controller"] != null) ? filterContextORG.RouteData.Values["controller"].ToString().ToLower().Trim() : string.Empty;
            string action = (filterContextORG.RouteData.Values["action"] != null) ? filterContextORG.RouteData.Values["action"].ToString().ToLower().Trim() : string.Empty;
            if (HttpContext.Current.Session["User"] == null)
            {
                /// this handles session when data is requested through Ajax json
                if (filterContextORG.HttpContext.Request.IsAjaxRequest())
                {
                    JsonResult result = new JsonResult { Data = "Session Timeout!" };
                    filterContextORG.Result = result;
                }
                else
                {
                    if (controller.ToLower() == "home" && action.ToLower() == "index")
                    {
                    }
                    else { 
                        /// If session is expired then redirected to logout page which further redirect to login page. 
                        filterContextORG.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { action = "index", Controller = "home", returnUrl = filterContextORG.HttpContext.Request.RawUrl }));
                        return;
                    }
                }
            }
        }
    }
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UserSessionActionFilter());

        }
    }
}
