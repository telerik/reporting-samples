using Telerik.Blazor.Resources;
using Telerik.Blazor.Services;

namespace NativeBlazorRvLocalizationWebAssembly.Services
{
    public class TelerikBlazorUiStringLocalizer : ITelerikStringLocalizer
    {
        public string this[string key]
        {
            get
            {
                return Messages.ResourceManager.GetString(key, Messages.Culture) ?? key;
            }
        }
    }
}
