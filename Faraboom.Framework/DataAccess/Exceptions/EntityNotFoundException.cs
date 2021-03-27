using System;

namespace Faraboom.Framework.DataAccess.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, int entityKey)
        {
            EntityName = entityName;
            EntityKey = entityKey;
            Message = $"Entity of type '{entityName}' and key {EntityKey} not found in the current context.";
        }

        public string EntityName { get; set; }

        public int EntityKey { get; set; }

        public override string Message { get; }
    }
}