using System;
using Rebop.Vm.Registers;
using Rebop.Vm.Memory;
using Rebop.Vm.Operations;

namespace Rebop.Vm
{
    class Cpu : ICpu, ICycle
    {


        protected Acc _acc = new Acc();
        protected Ir _ir = new Ir();
        protected Iv _iv = new Iv();
        protected Pc _pc = new Pc();
        protected X _x = new X();
        protected Sp _sp = new Sp();

        protected Status _status = new Status();

        protected Temp _tempA = new Temp();
        protected Temp _tempB = new Temp();

        protected Ram _ram = new Ram();

        protected bool _halted;

        Operation _operation;

        public IRegister<byte> Acc => _acc;
        public IRegister<byte> Ir => _ir;
        public IRegister<ushort> Iv => _iv;
        public IRegister<ushort> Pc => _pc;
        public IRegister<ushort> X => _x;
        public IRegister<ushort> Sp => _sp;
        public IRegister<ushort> TempA => _tempA;
        public IRegister<ushort> TempB => _tempB;
        public IStatus Status => _status;
        public IRam Ram => _ram;

        

        public Cpu()
        {
            Reset();
        }

        public void Reset()
        {
            _acc.Value = 0;
            _ir.Value = 0;
            _iv.Value = 0;
            _pc.Value = 0x4000;
            _x.Value = 0;
            _sp.Value = 0;
            _status.Value = 0;
            _tempA.Value = 0;
            _tempB.Value = 0;
            _ram.Reset();
            _halted = false;
        }

        protected void Clean()
        {
            _acc.Clean();
            _ir.Clean();
            _iv.Clean();
            _pc.Clean();
            _x.Clean();
            _sp.Clean();
            _status.Clean();
            _tempA.Clean();
            _tempB.Clean();
            _ram.Clean();
        }

        public void Halt()
        {
            _halted = true;
        }

        public bool Halted => _halted;

        public void Fetch()
        {
            if (!_halted)
            {

                _ir.Value = _ram[_pc.Value];

                //TODO post fetch event
            }
        }

        public void Decode()
        {
            if (!_halted)
            {
                _operation = Operation.Opcodes[_ir.Value];

                //TODO post decode event
            }
        }

        public void Load()
        {
            if (!_halted)
            {
                _operation.Load(this);

                //TODO post load event
            }
        }

        public void Execute()
        {
            if (!_halted)
            {
                _operation.Execute(this);
                _pc.Value=(ushort)(_pc.Value + _operation.Width);

                //TODO post execute event
            }
        }
    }
}
