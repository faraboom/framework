using System;

namespace Faraboom.Framework.DataAnnotation.Schema
{
    public sealed class DatabaseGeneratedAttribute : System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedAttribute
    {
        public string SequenceName { get; }

        public new DatabaseGeneratedOption DatabaseGeneratedOption { get; }

        public DatabaseGeneratedAttribute(DatabaseGeneratedOption databaseGeneratedOption, string sequenceName = null)
            : base(Convert(databaseGeneratedOption))
        {
            SequenceName = sequenceName;
            DatabaseGeneratedOption = databaseGeneratedOption;
        }

        private static System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption Convert(DatabaseGeneratedOption databaseGeneratedOption)
        {
            switch (databaseGeneratedOption)
            {
                case Schema.DatabaseGeneratedOption.None:
                    return System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None;
                case Schema.DatabaseGeneratedOption.Identity:
                    return System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity;
                case Schema.DatabaseGeneratedOption.Computed:
                    return System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed;
                default:
                    throw new ArgumentException(databaseGeneratedOption.ToString());
            }
        }
    }
}