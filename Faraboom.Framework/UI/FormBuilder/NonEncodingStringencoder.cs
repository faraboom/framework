using System;
using Faraboom.Framework.UI.FormBuilder.ModelBinding;

namespace Faraboom.Framework.UI.FormBuilder
{
    public class NonEncodingStringencoder : IStringEncoder
    {
        public Type ReadTypeFromString(string typeString)
        {
            return Type.GetType(typeString);
        }

        public string WriteTypeToString(Type type)
        {
            return type.AssemblyQualifiedName;
        }
    }
}