using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Rebop.Vm.Registers;
using Rebop.Vm.Memory;
using Rebop.Vm.Operations;

namespace Rebop.Vm
{
    class Cpu : ICpu, ICycle
    {


        internal Acc _acc = new Acc();
        internal Ir _ir = new Ir();
        internal Iv _iv = new Iv();
        internal Pc _pc = new Pc();
        internal X _x = new X();
        internal Sp _sp = new Sp();

        internal Status _status = new Status();

        internal Temp _tempA = new Temp();
        internal Temp _tempB = new Temp();
        internal Temp _mar = new Temp();

        internal Ram _ram = new Ram();

        protected bool _halted;

        protected Operation _operation;

        public IRegister<byte> Acc => _acc;
        public IRegister<byte> Ir => _ir;
        public IRegister<ushort> Iv => _iv;
        public IRegister<ushort> Pc => _pc;
        public IRegister<ushort> X => _x;
        public IRegister<ushort> Sp => _sp;
        public IRegister<ushort> TempA => _tempA;
        public IRegister<ushort> TempB => _tempB;
        public IRegister<ushort> Mar => _mar;
        public IStatus Status => _status;
        public IRam Ram => _ram;

        public Dictionary<byte, Operation> Opcodes { get; private set; } = new Dictionary<byte, Operation>();


        public Cpu()
        {
            Reset();
            LoadOperations();
        }

        private void LoadOperations()
        {
            //load opcodes

            List<Type> operationTypes = Assembly.GetAssembly(typeof(Operation)).GetTypes().Where(type => type.IsSubclassOf(typeof(Operation))).ToList<Type>();

            foreach (Type operationType in operationTypes)
            {
                OpcodeAttribute[] oas = (OpcodeAttribute[])Attribute.GetCustomAttributes(operationType, typeof(OpcodeAttribute));

                foreach (OpcodeAttribute oa in oas)
                {
                    Operation operation = (Operation)Activator.CreateInstance(operationType, this, oa.AddressingMode);
                    Opcodes.Add(oa.Opcode, operation);
                }

            }

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
            _mar.Value = 0;
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
            //Pc is set to next instruction


            if (!_halted)
            {
                _tempA.Value = 0;
                _tempB.Value = 0;
                _mar.Value = 0;
                Clean();

                _ir.Value = _ram[_pc.Value];

                //TODO post fetch event
            }
            else
            {
                //TODO throw exeception
            }
        }

        public void Decode()
        {
            if (!_halted)
            {
                if (Opcodes.ContainsKey(_ir.Value))
                {
                    _operation = Opcodes[_ir.Value];
                }
                else
                {
                    throw new InvalidOperationException($"Opcode {_ir.Value} isn't valid");
                }

                //TODO post decode event
            }
        }

        public void Load()
        {

            if (!_halted)
            {
                _operation.Load();

                //TODO post load event
            }
        }

        public void Execute()
        {
            if (!_halted)
            {
                _operation.Execute();
                _pc.Value=(ushort)(_pc.Value + _operation.Width);

                //TODO post execute event
            }
        }
    }
}
