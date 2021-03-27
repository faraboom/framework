using Faraboom.Framework.Core;

using System;

namespace Faraboom.Framework.DataAnnotation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Method)]
    public sealed class DisplayAttribute : Attribute
    {
        public Type ResourceType { get; set; }
        
        public Type EnumType { get; set; }

        /// <summary>
        /// A value that is used for display in the UI for label.
        /// if EnumType & ResourceType are provided: 1- localized EnumName_PropertyName_Description, 2- Static Description
        /// if just ResourceType provided: 1- localized PropertyName_Name, 2- Static Description
        /// else Name
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A value that is used to group fields in the UI.
        /// if EnumType & ResourceType are provided: 1- localized EnumName_PropertyName_GroupName, 2- Static GroupName
        /// if just ResourceType provided: 1- localized PropertyName_GroupName, 2- Static GroupName
        /// else Name
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// A value that is used for display in the UI.
        /// if EnumType & ResourceType are provided: 1- localized EnumName_PropertyName_Name, 2- localized EnumName_PropertyName, 3- Static Name
        /// if just ResourceType provided: 1- localized PropertyName_Name, 2- Static Name
        /// else Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The order weight of the column.
        /// </summary>
        public int Order { get; set; } = Constants.DisplayOrder;

        /// <summary>
        /// A value that will be used to display a watermark in the UI.
        /// if EnumType & ResourceType are provided: 1- localized EnumName_PropertyName_Prompt, 2- Static Prompt
        /// if just ResourceType provided: 1- localized PropertyName_Prompt, 2- Static Prompt
        /// else Name
        /// </summary>
        public string Prompt { get; set; }

        /// <summary>
        /// A value that is for the grid column label.
        /// if EnumType & ResourceType are provided: 1- localized EnumName_PropertyName_ShortName, 2- Static ShortName
        /// if just ResourceType provided: 1- localized PropertyName_ShortName, 2- Static ShortName
        /// else Name
        /// </summary>
        public string ShortName { get; set; }
    }
}