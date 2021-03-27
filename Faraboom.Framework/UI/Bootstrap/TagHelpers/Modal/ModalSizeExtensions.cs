using System;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Modal
{
    public static class ModalSizeExtensions
    {
        public static string ToClassName(this ModalSize size)
        {
            switch (size)
            {
                case ModalSize.Small:
                    return "modal-sm";
                case ModalSize.Large:
                    return "modal-lg";
                case ModalSize.ExtraLarge:
                    return "modal-xl";
                case ModalSize.Default:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException($"Unknown {nameof(ModalSize)}: {size}");
            }
        }
    }
}