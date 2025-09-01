using dafsem.Components;
using dafsem.Context;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace dafsem.Views.Shared.Components
{
    public class LanguageChangeViewComponent : ViewComponent
    {
        private readonly IServiceManager _serviceManager;

        public LanguageChangeViewComponent(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languages = await _serviceManager.DilService.SoftGetAllDilAsSelectListAsync();
            return View(languages);
        }
    }
}
