using System;
using System.Collections.Generic;

namespace ShadyNagy.Utilities.DesignPatterns.Tests.Models
{


    public class Customer
    {
        public virtual Guid Id { get; set; }
        public virtual Guid? IdNullable { get; set; }
        public virtual string Name { get; set; }
        public virtual int Age { get; set; }
        public virtual FlagsEnum Flags { get; set; } = FlagsEnum.One | FlagsEnum.Two;
        public virtual IEnumerable<Order> Orders { get; set; }
    }
}
