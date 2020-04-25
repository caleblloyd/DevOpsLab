namespace DevOpsLab.Client.Components
{
    public class WrapParam<T>
    {
        public WrapParam()
        {
        }

        public WrapParam(T initial)
        {
            Value = initial;
        }

        public T Value { get; set; }
    }
}
