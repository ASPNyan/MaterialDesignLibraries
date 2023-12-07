using System.Numerics;
using MaterialDesign.Color.Colorspaces;

namespace MaterialDesign.Color.Quantize;

/// <summary>
/// An image quantizer that divides the image's pixels into clusters by
/// recursively cutting an RGB cube, based on the weight of pixels in each area
/// of the cube.
///
/// The algorithm was described by Xiaolin Wu in Graphic Gems II, published in
/// 1991.
/// </summary>
public class QuantizerWu
{
    private const int IndexBits = 5;
    private const int IndexCount = (1 << IndexBits) + 1;
    private const int TotalSize = IndexCount * IndexCount * IndexCount;

    private long[] Weights { get; set; } = new long[TotalSize];
    private long[] MomentsR { get; set; } = new long[TotalSize];
    private long[] MomentsG { get; set; } = new long[TotalSize];
    private long[] MomentsB { get; set; } = new long[TotalSize];
    private double[] Moments { get; set; } = new double[TotalSize];
    private Box[] Cubes { get; set; } = [];

    private static int GetIndex(int r, int g, int b) => 
        (r << (IndexBits * 2)) + (r << (IndexBits + 1)) + (g << IndexBits) + r + g + b;

    private void ConstructHistogram(IEnumerable<RGBA> pixels)
    {
        foreach (RGBA pixel in pixels)
        {
            const int bitsToRemove = 8 - IndexBits;

            int indexR = (pixel.R >> bitsToRemove) + 1;
            int indexG = (pixel.G >> bitsToRemove) + 1;
            int indexB = (pixel.B >> bitsToRemove) + 1;
        
            int index = GetIndex(indexR, indexG, indexB);

            Weights[index]++;
            MomentsR[index] += pixel.R;
            MomentsG[index] += pixel.G;
            MomentsB[index] += pixel.B;
            Moments[index] += pixel.R * pixel.R + pixel.G * pixel.G + pixel.B * pixel.B;
        }
    }

    private void ComputeMoments()
    {
        for (int r = 1; r < IndexCount; r++)
        {
            long[] area = new long[IndexCount];
            long[] areaR = new long[IndexCount];
            long[] areaG = new long[IndexCount];
            long[] areaB = new long[IndexCount];
            double[] area2 = new double[IndexCount];
    
            for (int g = 1; g < IndexCount; g++)
            {
                long line = 0, lineR = 0, lineG = 0, lineB = 0;
                double line2 = 0.0;
                
                for (int b = 1; b < IndexCount; b++)
                {
                    int index = GetIndex(r, g, b);
                    
                    line += Weights[index];
                    lineR += MomentsR[index];
                    lineG += MomentsG[index];
                    lineB += MomentsB[index];
                    line2 += Moments[index];
                    
                    area[b] += line;
                    areaR[b] += lineR;
                    areaG[b] += lineG;
                    areaB[b] += lineB;
                    area2[b] += line2;
    
                    int previousIndex = GetIndex(r - 1, g, b);
                    Weights[index] = Weights[previousIndex] + area[b];
                    MomentsR[index] = MomentsR[previousIndex] + areaR[b];
                    MomentsG[index] = MomentsG[previousIndex] + areaG[b];
                    MomentsB[index] = MomentsB[previousIndex] + areaB[b];
                    Moments[index] = Moments[previousIndex] + area2[b];
                }
            }
        }
    }

    private static long Top(Box cube, Direction direction, int position, IReadOnlyList<long> moment)
    {
        return direction switch
        {
            Direction.Red => moment[GetIndex(position, cube.G1, cube.B1)] -
                             moment[GetIndex(position, cube.G1, cube.B0)] -
                             moment[GetIndex(position, cube.G0, cube.B1)] +
                             moment[GetIndex(position, cube.G0, cube.B0)],
            Direction.Green => moment[GetIndex(cube.R1, position, cube.B1)] -
                               moment[GetIndex(cube.R1, position, cube.B0)] -
                               moment[GetIndex(cube.R0, position, cube.B1)] +
                               moment[GetIndex(cube.R0, position, cube.B0)],
            _ => moment[GetIndex(cube.R1, cube.G1, position)] -
                moment[GetIndex(cube.R1, cube.G0, position)] -
                moment[GetIndex(cube.R0, cube.G1, position)] +
                moment[GetIndex(cube.R0, cube.G0, position)]
        };
    }

    private static long Bottom(Box cube, Direction direction, IReadOnlyList<long> moment)
    {
        return direction switch
        {
            Direction.Red => -moment[GetIndex(cube.R0, cube.G1, cube.B1)] +
                             moment[GetIndex(cube.R0, cube.G1, cube.B0)] +
                             moment[GetIndex(cube.R0, cube.G0, cube.B1)] -
                             moment[GetIndex(cube.R0, cube.G0, cube.B0)],
            Direction.Green => -moment[GetIndex(cube.R1, cube.G0, cube.B1)] +
                               moment[GetIndex(cube.R1, cube.G0, cube.B0)] +
                               moment[GetIndex(cube.R0, cube.G0, cube.B1)] -
                               moment[GetIndex(cube.R0, cube.G0, cube.B0)],
            _ => -moment[GetIndex(cube.R1, cube.G1, cube.B0)] +
                moment[GetIndex(cube.R1, cube.G0, cube.B0)] +
                moment[GetIndex(cube.R0, cube.G1, cube.B0)] -
                moment[GetIndex(cube.R0, cube.G0, cube.B0)]
        };
    }

    private static T Volume<T>(Box cube, IReadOnlyList<T> moment)
        where T : INumber<T>
    {
        return moment[GetIndex(cube.R1, cube.G1, cube.B1)] -
               moment[GetIndex(cube.R1, cube.G1, cube.B0)] -
               moment[GetIndex(cube.R1, cube.G0, cube.B1)] +
               moment[GetIndex(cube.R1, cube.G0, cube.B0)] -
               moment[GetIndex(cube.R0, cube.G1, cube.B1)] +
               moment[GetIndex(cube.R0, cube.G1, cube.B0)] +
               moment[GetIndex(cube.R0, cube.G0, cube.B1)] -
               moment[GetIndex(cube.R0, cube.G0, cube.B0)];
    }

    private double Variance(Box cube)
    {
        double dr = Volume(cube, MomentsR);
        double dg = Volume(cube, MomentsG);
        double db = Volume(cube, MomentsB);
        double xx = Volume(cube, Moments);
        double hypotenuse = dr * dr + dg * dg + db * db;
        double volume = Volume(cube, Weights);
        return xx - hypotenuse / volume;
    }

    private MaximizeResult Maximize(Box cube, Direction direction, int first, int last, long wholeW,
            long wholeR, long wholeG, long wholeB)
    {
        long bottomR = Bottom(cube, direction, MomentsR);
        long bottomG = Bottom(cube, direction, MomentsG);
        long bottomB = Bottom(cube, direction, MomentsB);
        long bottomW = Bottom(cube, direction, Weights);
    
        double max = 0.0;
        int cut = -1;
    
        for (int i = first; i < last; i++) 
        {
            long halfR = bottomR + Top(cube, direction, i, MomentsR);
            long halfG = bottomG + Top(cube, direction, i, MomentsG);
            long halfB = bottomB + Top(cube, direction, i, MomentsB);
            long halfW = bottomW + Top(cube, direction, i, Weights);
            
            if (halfW == 0) continue;
    
            double temp = (halfR * halfR + halfG * halfG + halfB * halfB) / (double)halfW;
    
            halfR = wholeR - halfR;
            halfG = wholeG - halfG;
            halfB = wholeB - halfB;
            halfW = wholeW - halfW;
    
            if (halfW == 0) continue;
            
            temp += (halfR * halfR + halfG * halfG + halfB * halfB) / (double)halfW;
    
            if (temp > max) 
            {
                max = temp;
                cut = i;
            }
        }
        
        return new MaximizeResult(cut, max);
    }

    private bool Cut(ref Box box1, ref Box box2)
    {
        long wholeR = Volume(box1, MomentsR);
        long wholeG = Volume(box1, MomentsG);
        long wholeB = Volume(box1, MomentsB);
        long wholeW = Volume(box1, Weights);
    
        MaximizeResult maxR = Maximize(box1, Direction.Red, box1.R0 + 1, box1.R1, wholeW, wholeR, wholeG, wholeB);
        MaximizeResult maxG = Maximize(box1, Direction.Green, box1.G0 + 1, box1.G1, wholeW, wholeR, wholeG, wholeB);
        MaximizeResult maxB = Maximize(box1, Direction.Blue, box1.B0 + 1, box1.B1, wholeW, wholeR, wholeG, wholeB);
    
        Direction direction;
        int cutPosition;
    
        if (maxR.Maximum >= maxG.Maximum && maxR.Maximum >= maxB.Maximum)
        {
            direction = Direction.Red;
            cutPosition = maxR.CutLocation;
            if (cutPosition < 0) return false;
        }
        else if (maxG.Maximum >= maxR.Maximum && maxG.Maximum >= maxB.Maximum)
        {
            direction = Direction.Green;
            cutPosition = maxG.CutLocation;
        }
        else
        {
            direction = Direction.Blue;
            cutPosition = maxB.CutLocation;
        }
    
        box2 = box2 with { R1 = box1.R1, G1 = box1.G1, B1 = box1.B1 };
    
        switch (direction)
        {
            case Direction.Red:
                box2 = box2 with { R0 = cutPosition, G0 = box1.G0, B0 = box1.B0 };
                box1 = box1 with { R1 = cutPosition };
                break;
    
            case Direction.Green:
                box2 = box2 with { G0 = cutPosition, R0 = box1.R0, B0 = box1.B0 };
                box1 = box1 with { G1 = cutPosition };
                break;
    
            case Direction.Blue:
                box2 = box2 with { B0 = cutPosition, R0 = box1.R0, G0 = box1.G0 };
                box1 = box1 with { B1 = cutPosition };
                break;
        }
    
        box1 = box1 with { Vol = (box1.R1 - box1.R0) * (box1.G1 - box1.G0) * (box1.B1 - box1.B0) };
        box2 = box2 with { Vol = (box2.R1 - box2.R0) * (box2.G1 - box2.G0) * (box2.B1 - box2.B0) };
    
        return true;
    }
    
    /// <param name="pixels">Colors in <see cref="RGBA"/> format</param>
    /// <param name="maxColors">The number of colors to divide the image into. A lower
    /// number of colors may be returned.</param>
    /// <returns>Quantized colors in <see cref="RGBA"/> format</returns>
    public RGBA[] Quantize(RGBA[] pixels, int maxColors)
    {
        if (maxColors is <= 0 or > 256 || pixels.Length is 0) return [];
        
        Weights = new long[TotalSize];
        MomentsR = new long[TotalSize];
        MomentsG = new long[TotalSize];
        MomentsB = new long[TotalSize];
        Moments = new double[TotalSize];
        
        ConstructHistogram(pixels);
        ComputeMoments();
    
        Cubes = new Box[maxColors];
        Cubes[0] = new Box { R0 = 0, G0 = 0, B0 = 0, R1 = IndexCount - 1, G1 = IndexCount - 1, B1 = IndexCount - 1 };
    
        double[] volumeVariance = new double[maxColors];
        int next = 0;
    
        for (int i = 1; i < maxColors; ++i) {
            if (Cut(ref Cubes[next], ref Cubes[i])) 
            {
                volumeVariance[next] = Cubes[next].Vol > 1 ? Variance(Cubes[next]) : 0.0;
                volumeVariance[i] = Cubes[i].Vol > 1 ? Variance(Cubes[i]) : 0.0;
            } else 
            {
                volumeVariance[next] = 0.0;
                i--;
            }
          
            next = 0;
            double temp = volumeVariance[0];
            for (int j = 1; j <= i; j++) 
            {
                if (volumeVariance[j] > temp) 
                {
                    temp = volumeVariance[j];
                    next = j;
                }
            }
            
            if (temp <= 0.0) 
            {
                maxColors = i + 1;
                break;
            }
        }
    
        List<RGBA> outColors = [];
    
        for (int i = 0; i < maxColors; ++i) {
            long weight = Volume(Cubes[i], Weights);
            if (weight > 0) {
                byte r = (byte)(Volume(Cubes[i], MomentsR) / weight);
                byte g = (byte)(Volume(Cubes[i], MomentsG) / weight);
                byte b = (byte)(Volume(Cubes[i], MomentsB) / weight);
    
                outColors.Add(new RGBA(r, g, b));
            }
        }
        return outColors.ToArray();
    }
    
    private enum Direction
    {
        Red,
        Green,
        Blue
    }
}