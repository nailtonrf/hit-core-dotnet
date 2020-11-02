using System;
using System.Data;

namespace Hitmu.Abstractions.Store
{
    public sealed class TransactionalContextAttribute : Attribute
    {
        public TransactionalContextAttribute(Type contextType, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            ContextType = contextType ?? throw new ArgumentNullException(nameof(contextType));
            IsolationLevel = isolationLevel;
        }

        public Type ContextType { get; }
        public IsolationLevel IsolationLevel { get; }

        public override string ToString() => ContextType.FullName;
    }
}