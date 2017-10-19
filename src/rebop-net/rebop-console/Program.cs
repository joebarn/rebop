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

            //code
            ram[0x4000] = 0x90; //lda
            ram[0x4001] = 0x8F; 

            ram[0x4002] = 0x11; //add
            ram[0x4003] = 0x80;
            ram[0x4004] = 0x90;

            ram[0x4005] = 0x01; //halt

            //data
            ram[0x8090] = 0x00;


            while (!driver.IsHalted)
            {
                driver.Clock();
            }

            Console.WriteLine(driver.Cpu.Acc);
            Console.WriteLine(driver.Cpu.Status);

        }
    }
}
