using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rebop.Vm;
using Rebop.Vm.Memory;

public class RunRebop : MonoBehaviour
{
    protected Driver _driver = new Driver();

    private void Start()
    {
        ICpu cpu = _driver.Cpu;
        IRam ram = cpu.Ram;

        ram[0x4000] = 0x90; //lda
        ram[0x4001] = 0x02;

        ram[0x4002] = 0x10; //add
        ram[0x4003] = 0x01;

        ram[0x4007] = 0x01; //halt


        while (!_driver.Halted)
        {
            _driver.Clock();
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), $"{_driver.Cpu.Acc} : {_driver.Cpu.Status}");
    }

}
