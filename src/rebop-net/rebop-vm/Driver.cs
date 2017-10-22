using Rebop.Vm.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm
{
    public class Driver
    {
        private Cpu _cpu = new Cpu();

        public ICpu Cpu => _cpu;

        //timer
        public uint Speed { get; set; } = 0;


        public void Start()
        {

        }

        public void Run()
        {


        }

        public void Clock()
        {


            //state = fetch, decode, load, execute
            _cpu.Fetch();
            _cpu.Decode();
            _cpu.Load();
            _cpu.Execute();

            //free running vs stepping

            //monitor/debug events

        }

        public void Reset()
        {
            _cpu.Reset();
        }

        public void Halt()
        {


        }

        public bool IsHalted
        {
            get
            {
                return _cpu.Halted;
            }
        }


        public void Shutdown()
        {

        }



        public static Instruction[] GetInstructions()
        {
            List<Instruction> instructions = new List<Instruction>();

            List<Type> operationTypes = Assembly.GetAssembly(typeof(Operation)).GetTypes().Where(type => type.IsSubclassOf(typeof(Operation))).ToList<Type>();

            foreach (Type operationType in operationTypes)
            {
                OpcodeAttribute[] oas = (OpcodeAttribute[])Attribute.GetCustomAttributes(operationType, typeof(OpcodeAttribute));

                foreach (OpcodeAttribute oa in oas)
                {
                    instructions.Add(new Instruction {OpCode=oa.Opcode, Mnemonic=operationType.Name, AddressingMode=oa.AddressingMode, Width=Operation.GetWidth(oa.AddressingMode) });
                }

            }

            return instructions.ToArray();
        }




    }
}
