using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Memory
{
    class Ram : IRam
    {
        protected const int SIZE= 0xFFFF + 1; //65,536
        protected byte[] _buffer=new byte[SIZE]; 
        protected ushort? _lastRead;
        protected ushort? _lastWrite;

        public Ram()
        {
            Reset();
        }

        public void Reset()
        {
            Array.Clear(_buffer, 0, SIZE);
        }

        public void Clean()
        {
            _lastRead = null;
            _lastWrite = null;
        }

        public void Load(ushort address, byte[] buffer)
        {
            foreach (byte b in buffer)
            {
                this[address++]=b;
            }
        }

        public byte this[ushort address]
        {
            set
            {
                _lastWrite = address;
                UpdateRegions(address);
                _buffer[address] = value;
            }
            get
            {
                _lastRead = address;
                UpdateRegions(address);
                return _buffer[address];
            }
        }

        public ushort? LastRead
        {
            get
            {
                return _lastRead;
            }
        }

        public ushort? LastWrite
        {
            get
            {
                return _lastWrite;
            }
        }

        protected void UpdateRegions(ushort address)
        {
            //contiguous regions
        }

        public IList<IRegion> Regions
        {
            get
            {
                throw new NotImplementedException();
            }
        }

    }
}
