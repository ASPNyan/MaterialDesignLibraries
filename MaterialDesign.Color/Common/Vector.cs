namespace MaterialDesign.Color.Common;

public class Vector
{
    private readonly double[] _data;

    private Vector(IEnumerable<double> col)
    {
        _data = col.ToArray();
    }
    
    public static Vector From(ICollection<double> col) => new(col);

    public double this[int i]
    {
        get => _data[i];
        set => _data[i] = value;
    }

    public double DotProduct(Vector vector)
    {
        if (Count != vector.Count)
            throw new ArithmeticException("Vectors must be of equal length to calculate a dot product.");

        double result = 0;
        for (int i = 0; i < Count; i++) result += this[i] * vector[i];
        
        return result;
    }
    
    public int Count => _data.Length;
}