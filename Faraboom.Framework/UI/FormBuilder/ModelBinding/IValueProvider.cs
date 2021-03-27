namespace Faraboom.Framework.UI.FormBuilder.ModelBinding
{
    public interface IValueProvider
    {
        IValueProviderResult GetValue(string key);
    }
}