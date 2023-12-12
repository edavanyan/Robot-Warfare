namespace StateMachine
{
    public class Optional<T>
    {
        private readonly T value;

        public Optional(T value)
        {
            this.value = value;
        }

        public T Get() => value;

        public static readonly Optional<T> Empty = new (default);
    }
}
