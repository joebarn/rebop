using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rebop.Vm.Operations
{
    abstract class Operation
    {
        private AddressingModes _addressingMode;

        public static Dictionary<byte, Operation> Opcodes { get; private set; } = new Dictionary<byte, Operation>();

        static Operation()
        {
            //load opcodes

            List<Type> operationTypes = Assembly.GetAssembly(typeof(Operation)).GetTypes().Where(type => type.IsSubclassOf(typeof(Operation))).ToList<Type>();

            foreach (Type operationType in operationTypes)
            {
                OpcodeAttribute[] oas = (OpcodeAttribute[])Attribute.GetCustomAttributes(operationType, typeof(OpcodeAttribute));

                foreach (OpcodeAttribute oa in oas)
                {
                    Operation operation = (Operation)Activator.CreateInstance(operationType, oa.AddressingMode);
                    Opcodes.Add(oa.Opcode, operation);
                }

            }

        }

        public Operation(AddressingModes addressingMode)
        {
            _addressingMode = addressingMode;
        }

        public ushort Width
        {
            get
            {
                switch (_addressingMode)
                {
                    case AddressingModes.Implied:
                        return 1;

                    case AddressingModes.Immediate:
                        return 2;

                    default:
                        return 3;
                }

            }
        }

        public void Load(Cpu cpu)
        {
            //TODO

            switch (_addressingMode)
            {
                case AddressingModes.Absolute:
                    break;

                default:
                    break;
            }
        }

        public void Execute(Cpu cpu)
        {
            OnExecute(cpu, _addressingMode);
        }


        protected abstract void OnExecute(Cpu cpu, AddressingModes addressingMode);

    }
}
