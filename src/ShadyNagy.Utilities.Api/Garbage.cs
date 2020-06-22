using System;
using System.Runtime;

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
