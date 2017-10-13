using System;

namespace Rebop.Vm
{
    [Flags]
    public enum AddressingModes
    {
        Implied = 0,                 //imp
        Immediate = 1,               //imm(2)   $ff
        BigImmediate = 2,            //imm(3)   $ffff
        Absolute = 4,                //abs      [$ffff]
        Indexed = 8,                 //abs-x    [$ffff,X]
        Indirect = 16,               //ind      [[$ffff]]
        IndirectPreIndexed = 32,     //x-ind    [[$ffff,X]]
        IndirectPostIndexed = 64     //ind-x    [[$ffff],X]
    }
}
