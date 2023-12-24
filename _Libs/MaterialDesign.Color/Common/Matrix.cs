namespace MaterialDesign.Color.Common;


public class Matrix
{
    // [col][row]
    private readonly double[][] _data;

    private Matrix(double[][] arr)
    {
        _data = arr;
    }
    
    public static Matrix From(double[][] arr) => new(arr);

    public Vector Multiply(Vector vector)
    {
        List<double> result = [];
        result.AddRange(_data.Select(Vector.From).Select(vector.DotProduct));
        return Vector.From(result);
    }
}