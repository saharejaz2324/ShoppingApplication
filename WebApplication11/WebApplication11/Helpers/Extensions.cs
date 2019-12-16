using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApplication.API.Helpers
{
    // we don't need to create a new instance of extensions when we want to use 
    // one of its methods & the extension method will add or say public static
    // voids because we don't want to return anything from this particular method
    public static class Extensions
    {
        public static void AddApplcationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
