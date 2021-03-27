using System.Collections.Generic;
using System.Linq;

namespace Faraboom.Framework.Data
{
    public abstract class ValueObjectBase
    {
        protected static bool EqualOperator(ValueObjectBase left, ValueObjectBase right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObjectBase left, ValueObjectBase right)
        {
            return !EqualOperator(left, right);
        }

        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            ValueObjectBase other = obj as ValueObjectBase;
            if (other == null)
                return false;

            IEnumerator<object> thisValues = GetAtomicValues().GetEnumerator();
            IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^
                    ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }

                if (thisValues.Current != null &&
                    !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
             .Select(t => t != null ? t.GetHashCode() : 0)
             .Aggregate((t1, t2) => t1 ^ t2);
        }

        public static bool operator ==(ValueObjectBase left, ValueObjectBase right)
        {
            return EqualOperator(left, right);
        }

        public static bool operator !=(ValueObjectBase left, ValueObjectBase right)
        {
            return NotEqualOperator(left, right);
        }
    }
}
