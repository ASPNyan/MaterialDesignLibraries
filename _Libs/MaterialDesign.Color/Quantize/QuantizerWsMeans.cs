namespace MaterialDesign.Color.Quantize;

/// <summary>
/// An image quantizer that improves on the speed of a standard K-Means algorithm
/// by implementing several optimizations, including deduplicating identical pixels
/// and a triangle inequality rule that reduces the number of comparisons needed
/// to identify which cluster a point should be moved to.
///
/// WsMeans stands for Weighted Square Means.
///
/// This algorithm was designed by M. Emre Celebi, and was found in their 2011
/// paper, Improving the Performance of K-Means for Color Quantization.
/// https://arxiv.org/abs/1101.0395
/// </summary>
public static class QuantizerWsMeans
{
    private const int MaxIterations = 10;
    private const double MinMovementDistance = 3;
    
    /// <param name="inputPixels">Colors in <see cref="RGBA"/> format</param>
    /// <param name="startingClusters">Defines the initial state of the quantizer. Passing
    /// an empty array is fine, the implementation will create its own initial
    /// state that leads to reproducible results for the same inputs.
    /// Passing an array that is the result of Wu quantization leads to higher
    /// quality results.</param>
    /// <param name="maxColors">The number of colors to divide the image into. A lower
    /// number of colors may be returned.</param>
    /// <returns>Quantized colors in <see cref="RGBA"/> format</returns>
    public static FrequencyMap<RGBA> Quantize(in RGBA[] inputPixels, in RGBA[] startingClusters, int maxColors = 128)
    {
        if (maxColors <= 0 || startingClusters.Length is 0) return [];
        if (maxColors > 256) maxColors = 256;

        FrequencyMap<RGBA> pixelToCount = [];
        List<LAB> points = [];
        List<RGBA> pixels = [];

        foreach (RGBA inputPixel in inputPixels)
        {
            int frequency = pixelToCount.Add(inputPixel);
            
            if (frequency is 1)
            {
                LAB point = LAB.FromRGBA(inputPixel);
                points.Add(point);
                pixels.Add(inputPixel);
            }
        }

        int clusterCount = Math.Min(maxColors, pixelToCount.Count);
        if (startingClusters.Length > 0) clusterCount = Math.Min(clusterCount, startingClusters.Length);

        List<LAB> clusters = [..startingClusters.Select(LABPointProvider.FromRGBA)];

        int additionalClustersNeeded = clusterCount - clusters.Count;
        
        Random rng = new();

        if (startingClusters.Length is 0 && additionalClustersNeeded > 0)
        {
            for (int i = 0; i < additionalClustersNeeded; i++)
            {
                double l = rng.NextDouble() * 100;
                double a = rng.NextDouble() * 200 - 100; // the original math for this is (100.0 - (-100.0) + 1) + -100. what the fuck.
                double b = rng.NextDouble() * 200 - 100;
                
                clusters.Add(new LAB(l, a, b));
            }
        }

        List<int> clusterIndices = [];
        for (int i = 0; i < points.Count; i++) clusterIndices.Add(rng.Next(clusterCount));

        int[][] indexMatrix = new int[clusterCount][];
        DistanceAndIndex[][] distanceToIndexMatrix = new DistanceAndIndex[clusterCount][];
        for (int i = 0; i < clusterCount; i++)
        {
            indexMatrix[i] = new int[clusterCount];
            distanceToIndexMatrix[i] = new DistanceAndIndex[clusterCount];
            for (int j = 0; j < clusterCount; j++) distanceToIndexMatrix[i][j] = new DistanceAndIndex();
        }

        List<int> pixelCountSums = [];
        for (int i = 0; i < clusterCount; i++) pixelCountSums.Add(0);

        for (int iteration = 0; iteration < MaxIterations; iteration++)
        {
            for (int i = 0; i < clusterCount; i++)
            {
                for (int j = 0; j < clusterCount; j++)
                {
                    double distance = LABPointProvider.Distance(clusters[i], clusters[j]);
                    
                    distanceToIndexMatrix[i][j] = new DistanceAndIndex(distance, j);
                    distanceToIndexMatrix[j][i] = new DistanceAndIndex(distance, i);
                }
                
                Array.Sort(distanceToIndexMatrix[i]);
                for (int j = 0; j < clusterCount; j++)
                {
                    indexMatrix[i][j] = distanceToIndexMatrix[i][j].Index;
                }
            }


            bool colorMoved = false;
            for (int i = 0; i < points.Count; i++)
            {
                LAB point = points[i];
                int previousClusterIndex = clusterIndices[i];
                LAB previousCluster = clusters[previousClusterIndex];

                double previousDistance = LABPointProvider.Distance(point, previousCluster);

                double minimumDistance = previousDistance;
                int newClusterIndex = -1;

                for (int j = 0; j < clusterCount; j++)
                {
                    if (distanceToIndexMatrix[previousClusterIndex][j].Distance >= 4 * previousDistance) continue;
                    double distance = LABPointProvider.Distance(point, clusters[j]);
                    if (distance < minimumDistance)
                    {
                        minimumDistance = distance;
                        newClusterIndex = j;
                    }
                }

                if (newClusterIndex is not -1)
                {
                    double distanceChange = Math.Abs(Math.Sqrt(minimumDistance) - Math.Sqrt(previousDistance));
                    if (distanceChange > MinMovementDistance)
                    {
                        colorMoved = true;
                        clusterIndices[i] = newClusterIndex;
                    }
                }
            }

            if (!colorMoved && iteration is not 0) break;   

            double[] componentASums = new double[clusterCount];
            double[] componentBSums = new double[clusterCount];
            double[] componentCSums = new double[clusterCount];
            pixelCountSums = [..new int[clusterCount]]; // creates list with "clusterCount" amount of default-initialised values

            for (int i = 0; i < points.Count; i++)
            {
                int clusterIndex = clusterIndices[i];
                LAB point = points[i];
                int count = pixelToCount.GetFrequency(pixels[i]);

                pixelCountSums[clusterIndex] += count;
                componentASums[clusterIndex] += point.L * count;
                componentBSums[clusterIndex] += point.A * count;
                componentCSums[clusterIndex] += point.B * count;
            }

            for (int i = 0; i < clusterCount; i++)
            {
                int count = pixelCountSums[i];

                if (count is 0)
                {
                    clusters[i] = new LAB(0, 0, 0);
                    continue;
                }

                double a = componentASums[i] / count;
                double b = componentBSums[i] / count;
                double c = componentCSums[i] / count;
                clusters[i] = new LAB(a, b, c);
            }
        }
        
        Dictionary<RGBA, int> rgbaToPopulation = new();
        for (int i = 0; i < clusterCount; i++)
        {
            int count = pixelCountSums[i];
            if (count is 0) continue;

            RGBA possibleNewCluster = LABPointProvider.FromLAB(clusters[i]);
            rgbaToPopulation.TryAdd(possibleNewCluster, count);
        }

        return FrequencyMap<RGBA>.From(rgbaToPopulation);
    }
}

file struct DistanceAndIndex(double distance, int index) : IComparable<DistanceAndIndex>, IComparable
{
    public double Distance { get; set; } = distance;
    public int Index { get; set; } = index;

    public int CompareTo(DistanceAndIndex other)
    {
        return Distance.CompareTo(other.Distance);
    }

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        return obj is DistanceAndIndex other
            ? CompareTo(other)
            : throw new ArgumentException($"Object must be of type {nameof(DistanceAndIndex)}");
    }
}