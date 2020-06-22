using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime;
using System.Text;

namespace ShadyNagy.Utilities.Api
{
    public class Garbage
    {
        public static void Clear()
        {
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
        }
    }
}
