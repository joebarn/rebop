using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm
{
    public class Driver
    {
        private Cpu _cpu = new Cpu();

        //timer

        public void Start() { }

        public uint Speed { get; set; } = 0;

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

        public void Stop() { }

        public bool Halted
        {
            get
            {
                return _cpu.Halted;
            }
        }

        public ICpu Cpu => _cpu;

    }
}
