using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Registers
{
    class Register<T>:IRegister<T>
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


    }
}
