using Faraboom.Framework.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Faraboom.Framework.DataAnnotation
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ServiceLifetimeAttribute : Attribute
    {
        public ServiceLifetime ServiceLifetime { get; }
        public IEnumerable<Type> Parameters { get; }

        public ServiceLifetimeAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, string parameters = null)
        {
            ServiceLifetime = serviceLifetime;
            if (!string.IsNullOrWhiteSpace(parameters))
            {
                Parameters = parameters.Split(Constants.Delimiter, StringSplitOptions.RemoveEmptyEntries)?.Select(t => Type.GetType(t));
            }
        }
    }
}