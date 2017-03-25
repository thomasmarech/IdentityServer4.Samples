using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer
{
    public class UrlReferrerFilter : IActionFilter
    {
        private ILogger<UrlReferrerFilter> _logger;

        public UrlReferrerFilter()
        {
            //_logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string referer = context.HttpContext.Request.Headers["Referer"];
        }
    }
}
