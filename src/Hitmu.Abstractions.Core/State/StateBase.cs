namespace Hitmu.Abstractions.Core.State
{
    public abstract class StateBase<TIdType> : IState<TIdType> where TIdType : struct
    {
        public abstract TIdType Id { get; }

        public override bool Equals(object obj)
        {
            if (obj is StateBase<TIdType> comparableObject) return Equals(comparableObject);
            return false;
        }

        protected bool Equals(StateBase<TIdType> other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}