namespace MaterialDesign.Color.Common;


[Obsolete("Please, don't use this. For the sake of your code and performance, don't use this." +
          "Use a dedicated math library, or some of the inbuilt matrices in .NET, but please just don't use this.")]
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