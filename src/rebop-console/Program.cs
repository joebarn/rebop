using System;
using Rebop.Vm;
using Rebop.Vm.Memory;

namespace rebop_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Driver driver = new Driver();
            ICpu cpu = driver.Cpu;
            IRam ram = cpu.Ram;

            ram[0x4000] = 0x90; //lda
            ram[0x4001] = 0x02;

            ram[0x4002] = 0x10; //add
            ram[0x4003] = 0x02;

            ram[0x4004] = 0x01; //halt


            while (!driver.Halted)
            {
                driver.Clock();
            }

            Console.WriteLine(driver.Cpu.Acc.Value);

        }
    }
}
