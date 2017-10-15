using System;
using Rebop.Vm.Registers;
using Rebop.Vm.Memory;

namespace Rebop.Vm
{
    public interface ICpu
    {
        IRegister<byte> Acc { get; }
        IRegister<byte> Ir { get; }
        IRegister<ushort> Iv { get; }
        IRegister<ushort> Pc { get; }
        IRegister<ushort> X { get; }
        IRegister<ushort> Sp { get; }
        IRegister<ushort> TempA { get; }
        IRegister<ushort> TempB { get; }
        IRegister<ushort> Mar { get; }
        IStatus Status { get; }
        IRam Ram { get; }
    }
}
