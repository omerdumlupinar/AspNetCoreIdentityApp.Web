using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreIdentityApp.Web.Extenisons
{
    public static class ModelStateExtensions
    {
        public static void AddModelErrorList(this ModelStateDictionary modelStateExtensions, List<string> error)
        {
            error.ForEach(x =>
            {
                modelStateExtensions.AddModelError(string.Empty, x);
            });
        }
    }
}
