using System;

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

        public WrapParam(Func<T> getter, Action<T> setter)
        {
            _getter = getter;
            _setter = setter;
        }

        private readonly Func<T> _getter;
        private readonly Action<T> _setter;
        private T _value;

        public T Value
        {
            get => _getter != default ? _getter() : _value;
            set
            {
                if (_setter != default)
                {
                    _setter(value);
                    return;
                }

                _value = value;
            }
        }
    }
}
