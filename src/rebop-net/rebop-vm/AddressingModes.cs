using System;

namespace Rebop.Vm
{
    [Flags]
    public enum AddressingModes
    {
        Implied = 0,                 //imp
        Immediate = 1,               //imm(2)   $ff
        BigImmediate = 2,            //imm(3)   $ffff
        Absolute = 4,                //abs      [$ffff] -> byte
        BigAbsolute = 8,              //abs      [$ffff] -> ushort
        AbsoluteIndexed = 16,        //abs-x    [$ffff,X]
        Indirect = 32,               //ind      [[$ffff]]
        IndirectPreIndexed = 64,     //x-ind    [[$ffff,X]]
        IndirectPostIndexed = 128    //ind-x    [[$ffff],X]
    }
}
