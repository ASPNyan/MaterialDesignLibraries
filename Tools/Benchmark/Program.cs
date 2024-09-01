using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using MaterialDesign.Color.Colorspaces;

/*
 * 1.1.3 (Pre Matrix/Vector optimization)
 * - Total: 56872712.3us (56.8727123s),
 * - Time spent on processing color: 56241086.301291145us (56.24108630129115s),
 * - Avg. time spent on processing color: 3.3522299707705465us (3.3522299707705467E-06s)
 *
 * 1.2.0 v1 (Matrix/Vector optimization)
 * - Total: 30585114.3us (30.5851143s),
 * - Time spent on processing color: 29976378.099269155us (29.976378099269155s),
 * - Avg. time spent on processing color: 1.7867313682597372us (1.7867313682597372E-06s)
 *
 * 1.2.0 v2 (Vector optimizations in calculations)
 * Total: 24379017.1us (24.3790171s),
 * Time spent on processing color: 23625254.100344427us (23.625254100344428s),
 * Avg. time spent on processing color: 1.4081748783793704us (1.4081748783793704E-06s)
 */

namespace Benchmark;

public class Program
{
    private static void Main()
    {
        TimeSpan total = Benchmark();
        ReadOnlySpan<double> internalTimings = InternalTimings;

        Vector<double> timings = Vector<double>.Zero;
        ReadOnlySpan<Vector<double>> vectors = MemoryMarshal.Cast<double, Vector<double>>(internalTimings);

        foreach (ref readonly Vector<double> vector in vectors) timings += vector;
        double internalTotal = Vector.Sum(timings);
        
        int remainder = internalTimings.Length % Vector<double>.Count;

        foreach (ref readonly double ts in internalTimings[^remainder..]) internalTotal += ts;
        
        Console.WriteLine($"""
                           Total: {total.TotalMicroseconds}us ({total.TotalSeconds}s),
                           Time spent on processing color: {internalTotal}us ({internalTotal / 1e+6}s),
                           Avg. time spent on processing color: {internalTotal / internalTimings.Length}us ({internalTotal / internalTimings.Length / 1e+6}s)
                           """);
    }

    private static readonly double[] InternalTimings = new double[16_777_216]; // 256 * 256 * 256 RGB colors

    private static TimeSpan Benchmark()
    {
        Stopwatch main = Stopwatch.StartNew();

        Stopwatch inside = Stopwatch.StartNew();
        for (byte r = 0; ; r++)
        {
            for (byte g = 0; ; g++) 
            {
                for (byte b = 0; ; b++) 
                {
                    uint color = ((uint)((r << 16) | (g << 8) | b) << 8) | 0xFF;
                    RGBA original = new(r, g, b);
                    inside.Restart();
                    RGBA converted = HCTA.FromRGBA(original).ToRGBA();
                    InternalTimings[color >> 8] = inside.Elapsed.TotalMicroseconds;
                    
                    if (NotInRange(original, converted))
                    {
                        main.Stop();
                        string msg = $"Conversion inaccuracy: got {(uint)converted:X} instead of {color:X}";
                        Console.WriteLine(msg);
                        Console.WriteLine("Press Y to ignore, or any other key to exit.");
                        if (Console.ReadKey(true) is not {Key: ConsoleKey.Y}) throw new Exception(msg);
                        main.Start();
                    }
                    
                    if (b is byte.MaxValue) break;
                }
                
                if (g is byte.MaxValue) break;
            }
            
            if (r is byte.MaxValue) break;
        }

        main.Stop();
        
        return main.Elapsed;
        
        bool NotInRange(RGBA a, RGBA b)
        {
            return a.R - b.R is < -2 or > 2
                   && a.G - b.G is < -2 or > 2
                   && a.B - b.B is < -2 or > 2;
        }
    }
}