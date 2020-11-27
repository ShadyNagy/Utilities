using System;

namespace ShadyNagy.Utilities.DesignPatterns.Tests.Models
{
    [Flags]
    public enum FlagsEnum
    {
        One = 1 << 0,
        Two = 1 << 1
    }
}
