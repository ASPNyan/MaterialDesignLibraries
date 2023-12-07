using MaterialDesign.Color.Colorspaces;

namespace MaterialDesign.Color.Temperature;

/// <summary>
/// Design utilities using color temperature theory.
/// </summary>
public struct TemperatureCache(HCTA input)
{
    public HCTA Input { get; } = input;

    /// <summary>
    /// The value representing the cool-warm factor of a color. Values below 0 are considered cool, above values are warm.
    /// </summary>
    public static double RawTemperature(HCTA color)
    {
        LAB lab = LAB.FromRGBA(color.ToRGBA());

        double hue = Colorspaces.Color.SanitizeDegrees(Math.Atan2(lab.B, lab.A) * 180 / double.Pi);
        
        double chroma = Math.Sqrt(Math.Pow(lab.A, 2) + Math.Pow(lab.B, 2));

        return -0.5 * 0.02 * Math.Pow(chroma, 1.07) * Math.Cos(Colorspaces.Color.SanitizeDegrees(hue - 50) * double.Pi / 180);
    }

    public HCTA GetComplement()
    {
        if (_precomputedComplement is not null) return _precomputedComplement;

        double coldestHue = GetColdest().H;
        double coldestTemp = GetTempsByHCTA()[GetColdest()];

        double warmestHue = GetWarmest().H;
        double warmestTemp = GetTempsByHCTA()[GetWarmest()];

        double range = warmestTemp - coldestTemp;

        bool startHueIsColdestToWarmest = IsBetween(Input.H, coldestHue, warmestHue);
        double startHue = startHueIsColdestToWarmest ? warmestHue : coldestHue;
        double endHue = startHueIsColdestToWarmest ? coldestHue : warmestHue;

        const double directionOfRotation = 1;
        double smallestError = 1000;

        HCTA answer = GetHCTAsByHue()[Colorspaces.Color.Round0(Input.H)];

        double complementRelativeTemp = 1 - GetRelativeTemperature(Input);
        for (double hueAddend = 0; hueAddend <= 360; hueAddend++)
        {
            double hue = Colorspaces.Color.SanitizeDegrees(startHue + directionOfRotation * hueAddend);
            if (!IsBetween(hue, startHue, endHue)) continue;

            HCTA possibleAnswer = GetHCTAsByHue()[Colorspaces.Color.Round0(hue)];
            double relativeTemp = (GetTempsByHCTA()[possibleAnswer] - coldestTemp) / range;
            double error = Math.Abs(complementRelativeTemp - relativeTemp);

            if (error < smallestError)
            {
                smallestError = error;
                answer = possibleAnswer;
            }
        }

        _precomputedComplement = answer;
        return _precomputedComplement;
    }

    public List<HCTA> GetAnalogousColors() => GetAnalogousColors(5, 12);

    public List<HCTA> GetAnalogousColors(int count, int divisions)
    {
        int startHue = Colorspaces.Color.Round0(Input.H);
        HCTA startHCTA = GetHCTAsByHue()[startHue];
        double lastTemp = GetRelativeTemperature(startHCTA);

        List<HCTA> allColors = [startHCTA];

        double absoluteTotalTempDelta = 0;
        for (int i = 0; i < 360; i++)
        {
            int hue = Colorspaces.Color.SanitizeDegrees(startHue + i);
            HCTA hcta = GetHCTAsByHue()[hue];

            double temp = GetRelativeTemperature(hcta);
            double tempDelta = Math.Abs(temp - lastTemp);
            lastTemp = temp;
            absoluteTotalTempDelta += tempDelta;
        }

        int hueAddend = 1;
        double tempStep = absoluteTotalTempDelta / divisions;
        double totalTempDelta = 0;
        lastTemp = GetRelativeTemperature(startHCTA);
        while (allColors.Count < divisions)
        {
            int hue = Colorspaces.Color.SanitizeDegrees(startHue + hueAddend);
            HCTA hcta = GetHCTAsByHue()[hue];

            double temp = GetRelativeTemperature(hcta);
            double tempDelta = Math.Abs(temp - lastTemp);
            totalTempDelta += tempDelta;

            double desiredTotalTempDeltaForIndex = allColors.Count * tempStep;
            bool indexSatisfied = totalTempDelta >= desiredTotalTempDeltaForIndex;

            int indexAddend = 1;

            while (indexSatisfied && allColors.Count < divisions)
            {
                allColors.Add(hcta);
                desiredTotalTempDeltaForIndex = (allColors.Count + indexAddend) * tempStep;
                indexSatisfied = totalTempDelta >= desiredTotalTempDeltaForIndex;
                indexAddend++;
            }

            lastTemp = temp;
            hueAddend++;

            if (hueAddend > 360)
            {
                while (allColors.Count < divisions) allColors.Add(hcta);

                break;
            }
        }

        List<HCTA> answers = [Input];

        int ccwCount = (int)(((double)count - 1) / 2);
        for (int i = 1; i < ccwCount + 1; i++)
        {
            int index = 0 - i;

            while (index < 0) index = allColors.Count + index;

            if (index >= allColors.Count) index %= allColors.Count;
            
            answers.Insert(0, allColors[index]);
        }

        int cwCount = count - ccwCount - 1;

        for (int i = 1; i < cwCount + 1; i++)
        {
            int index = i;
            while (index < 0) index += allColors.Count;
            if (index >= allColors.Count) index %= allColors.Count;
            answers.Add(allColors[index]);
        }

        return answers;
    }

    public double GetRelativeTemperature(HCTA hcta)
    {
        double range = GetTempsByHCTA()[GetWarmest()] - GetTempsByHCTA()[GetColdest()];

        if (range is 0) return 0;

        double differenceFromColdest = GetTempsByHCTA()[hcta] - GetTempsByHCTA()[GetColdest()];

        return differenceFromColdest / range;
    }

    /// <summary>
    /// The coldest color with the same chroma and tone as input.
    /// </summary>
    public HCTA GetColdest() => GetHCTAsByTemp().First();

    /// <summary>
    /// The coldest color with the same chroma and tone as input.
    /// </summary>
    public HCTA GetWarmest() => GetHCTAsByTemp().Last();

    private static bool IsBetween(double angle, double a, double b) => a < b 
        ? a <= angle && angle <= b 
        : a <= angle || angle <= b;

    private List<HCTA> GetHCTAsByHue()
    {
        if (_precomputedHCTAsByHue is not null) return _precomputedHCTAsByHue;

        List<HCTA> hcta = [];

        for (double hue = 0; hue <= 360; hue++)
        {
            HCTA colorAtHue = new(hue, Input.C, Input.T);
            hcta.Add(colorAtHue);
        }

        _precomputedHCTAsByHue = hcta;

        return _precomputedHCTAsByHue;
    }

    private List<HCTA> GetHCTAsByTemp()
    {
        if (_precomputedHCTAsByTemp is not null) return _precomputedHCTAsByTemp;

        List<HCTA> hcta = GetHCTAsByHue();
        hcta.Add(Input);

        Dictionary<HCTA, double> tempsByHCTA = GetTempsByHCTA();

        hcta.Sort((a, b) =>  tempsByHCTA[a].CompareTo(tempsByHCTA[b]));

        _precomputedHCTAsByTemp = hcta;

        return _precomputedHCTAsByTemp;
    }

    private Dictionary<HCTA, double> GetTempsByHCTA()
    {
        if (_precomputedTempsByHCTA is not null) return _precomputedTempsByHCTA;

        List<HCTA> all = GetHCTAsByHue();
        all.Add(Input);

        Dictionary<HCTA, double> temperaturesByHCTA = new();
        foreach (HCTA key in all)
        {
            double value = RawTemperature(key);
            temperaturesByHCTA.TryAdd(key, value);
        }
            

        _precomputedTempsByHCTA = temperaturesByHCTA;

        return _precomputedTempsByHCTA;
    }

    private HCTA? _precomputedComplement;
    private List<HCTA>? _precomputedHCTAsByTemp;
    private List<HCTA>? _precomputedHCTAsByHue;
    private Dictionary<HCTA, double>? _precomputedTempsByHCTA;
}