using Microsoft.AspNetCore.Mvc;

namespace Faraboom.Framework.ViewComponents
{
    [ViewComponent(Name = "frb-bool")]
    public class Boolean : ViewComponent
    {
        public IViewComponentResult Invoke(bool? model)
        {
            return View(model);
        }
    }
}
