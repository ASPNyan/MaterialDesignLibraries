namespace MaterialDesign.Color.Common;

/* This and the Vector class are internal as they are not fully developed to be used, only containing the methods
 * required by this library.
 */
internal class Matrix
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