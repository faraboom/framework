using System;

namespace Faraboom.Framework.UI.FormBuilder.ModelBinding
{
    public interface IStringEncoder
    {
        Type ReadTypeFromString(string typeString);
        string WriteTypeToString(Type type);
    }
}