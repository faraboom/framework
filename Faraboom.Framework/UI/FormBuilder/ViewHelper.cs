using System.Text;

namespace Faraboom.Framework.UI.FormBuilder
{
    public static class ViewHelper
    {
        //public static string Raw(this bool value, string output)
        //{
        //    return (value ? output : "");
        //}
        //public static string Raw(this string value)
        //{
        //    return (value);
        //}

        //public static string InputAtts(this PropertyViewModel vm)
        //{
        //    return string.Join("", string.Join(" ", new string[] { vm.Disabled(), vm.Readonly(), vm.DataAtts() }));
        //}
        //public static string Disabled(this PropertyViewModel vm)
        //{
        //    return Attr(vm.Disabled, "disabled", null);
        //}
        //public static string Readonly(this PropertyViewModel vm)
        //{
        //    return Attr(vm.Readonly, "readonly", null);
        //}
        public static string DataAtts(this PropertyViewModel vm)
        {
            var sb = new StringBuilder();
            foreach (var att in vm.DataAttributes)
            {
                sb.Append("data-" + att.Key + "='" + att.Value + "' ");
            }
            return sb.ToString();
        }

        //public static string Attr(this bool value, string att, string attValue = null)
        //{
        //    return value.Raw(att + "=\"" + (attValue ?? att) + "\"");
        //}
        //public static string Attr(this string value, string att)
        //{
        //    if (value == null) return ("");
        //    return Raw(att + "=\"" + (value ?? att) + "\"");
        //}

        //public static string Placeholder(PropertyViewModel pi)
        //{
        //    var placeHolderText = pi.GetCustomAttributes().OfType<PlaceholderAttribute>().Select(a => a.Text).FirstOrDefault();
        //    return Attr((!string.IsNullOrWhiteSpace(placeHolderText)), "placeholder", placeHolderText);
        //}
    }



}
