using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Registers
{
    class Register<T>:IRegister<T> where T : struct, IFormattable
    {
        protected T _value;
        protected bool _dirty;

        public T Value
        {
            set
            {
                _dirty = true;
                _value = value;
            }
            get
            {
                return _value;
            }

        }

        public bool Dirty
        {
            get
            {
                return _dirty;
            }
        }

        public void Clean()
        {
            _dirty = false;
        }

        public override string ToString()
        {
            string format;

            if (typeof(T)==typeof(byte))
            {
                format = "X2";
            }
            else if (typeof(T) == typeof(ushort))
            {
                format = "X4";
            }
            else
            {
                format = "";
            }

            return $"{GetType().Name} : {_value.ToString(format, System.Globalization.CultureInfo.CurrentCulture)}";
        }

    }
}
