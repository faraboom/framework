using System;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Modal
{
    [Flags]
    public enum ModalButtons
    {
        None = 0,
        Save = 1,
        Cancel = 2,
        Close = 4
    }
}