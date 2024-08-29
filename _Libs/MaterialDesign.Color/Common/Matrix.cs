using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace MaterialDesign.Color.Common;

// ReSharper disable InconsistentNaming
public struct Matrix3x3 : IEquatable<Matrix3x3>
{
    public bool Equals(Matrix3x3 other)
    {
        return M11.Equals(other.M11) && M12.Equals(other.M12) && M13.Equals(other.M13) 
               && M21.Equals(other.M21) && M22.Equals(other.M22) && M23.Equals(other.M23) 
               && M31.Equals(other.M31) && M32.Equals(other.M32) && M33.Equals(other.M33);
    }

    public override bool Equals(object? obj)
    {
        return obj is Matrix3x3 other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(M11);
        hashCode.Add(M12);
        hashCode.Add(M13);
        hashCode.Add(M21);
        hashCode.Add(M22);
        hashCode.Add(M23);
        hashCode.Add(M31);
        hashCode.Add(M32);
        hashCode.Add(M33);
        return hashCode.ToHashCode();
    }

    public float M11, M12, M13;
    public float M21, M22, M23;
    public float M31, M32, M33;
    
    public Matrix3x3(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
    {
        Unsafe.SkipInit(out this);
        AsImpl().Init(m11, m12, m13, m21, m22, m23, m31, m32, m33);
    }
    
    public float this[int row, int column]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)] get => AsROImpl()[row, column];
        [MethodImpl(MethodImplOptions.AggressiveInlining)] set => AsImpl()[row, column] = value;
    }

    [UnscopedRef, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref Impl AsImpl() => ref Unsafe.As<Matrix3x3, Impl>(ref this);

    [UnscopedRef, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref readonly Impl AsROImpl()
    {
        return ref Unsafe.As<Matrix3x3, Impl>(ref Unsafe.AsRef(ref this));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref readonly Impl s_AsROImpl(ref Impl self)
    {
        return ref Unsafe.As<Impl, Impl>(ref Unsafe.AsRef(ref self));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator +(Matrix3x3 value1, Matrix3x3 value2)
    {
        return (value1.AsImpl() + value2.AsImpl()).AsM3x3();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Matrix3x3 value1, Matrix3x3 value2)
    {
        return value1.AsImpl() == value2.AsImpl();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Matrix3x3 value1, Matrix3x3 value2)
    {
        return value1.AsImpl() != value2.AsImpl();
    }
    
    public static Matrix3x3 operator *(Matrix3x3 value1, Matrix3x3 value2)
    {
        return (value1.AsImpl() * value2.AsImpl()).AsM3x3();
    }

    public static Vector3 operator *(Matrix3x3 value1, Vector3 value2)
    {
        return value1.AsImpl() * value2;
    }

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator *(Matrix3x3 value1, float value2)
    {
        return (value1.AsImpl() * value2).AsM3x3();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator -(Matrix3x3 value1, Matrix3x3 value2)
    {
        return (value1.AsImpl() - value2.AsImpl()).AsM3x3();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator -(Matrix3x3 value) => (-value.AsImpl()).AsM3x3();
    
    

    internal struct Impl
    {
        public Vector3 X, Y, Z;

        [UnscopedRef, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Matrix3x3 AsM3x3() => ref Unsafe.As<Impl, Matrix3x3>(ref this);

        public void Init(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
        {
            X = new Vector3(m11, m12, m13);
            Y = new Vector3(m21, m22, m23);
            Z = new Vector3(m31, m32, m33);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x3 Lerp(Matrix3x3 matrix1, Matrix3x3 matrix2, float amount)
        {
            return Lerp(in matrix1.AsImpl(), in matrix2.AsImpl(), amount).AsM3x3();
        }
        
        [UnscopedRef, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return s_AsROImpl(ref this).GetHashCode();
        }

        public static Impl Identity
        {
            get
            {
                Impl identity = default;
                identity.X = Vector3.UnitX;
                identity.Y = Vector3.UnitY;
                identity.Z = Vector3.UnitZ;
                return identity;
            }
        }

        public float this[int row, int col]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)row >= 3U) throw new ArgumentOutOfRangeException(nameof(row));
                return Unsafe.Add(ref Unsafe.AsRef(ref X), row)[col];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)] 
            set
            {
                if ((uint)row >= 3U) throw new ArgumentOutOfRangeException(nameof(row));
                Unsafe.Add(ref X, row)[col] = value;
            }
        }
        
        public readonly bool IsIdentity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => 
                X == Vector3.UnitX && Y == Vector3.UnitY && Z == Vector3.Zero;
        }
        
        public Vector3 Translation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] readonly get => Z;
            [MethodImpl(MethodImplOptions.AggressiveInlining)] set => Z = value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl operator +(in Impl left, in Impl right)
        {
            Impl impl;
            impl.X = left.X + right.X;
            impl.Y = left.Y + right.Y;
            impl.Z = left.Z + right.Z;
            return impl;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Impl left, in Impl right)
        {
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Impl left, in Impl right)
        {
            return left.X != right.X || left.Y != right.Y || left.Z != right.Z;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl operator *(in Impl left, in Impl right)
        {
            Impl impl;
            impl.X = right.X * left.X.X + right.Y * left.X.Y + right.Z * left.X.Z;
            impl.Y = right.X * left.Y.X + right.Y * left.Y.Y + right.Z * left.Y.Z;
            impl.Z = right.X * left.Z.X + right.Y * left.Z.Y + right.Z * left.Z.Z;
            return impl;
        }

        public static Vector3 operator *(in Impl left, in Vector3 right)
        {
            Vector3 vector;
            vector.X = left.X.X * right.X + left.X.Y * right.Y + left.X.Z * right.Z;
            vector.Y = left.Y.X * right.X + left.Y.Y * right.Y + left.Y.Z * right.Z;
            vector.Z = left.Z.X * right.X + left.Z.Y * right.Y + left.Z.Z * right.Z;
            return vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl operator *(in Impl left, float right)
        {
            Impl impl;
            impl.X = left.X * right;
            impl.Y = left.Y * right;
            impl.Z = left.Z * right;
            return impl;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl operator -(in Impl left, in Impl right)
        {
            Impl impl;
            impl.X = left.X - right.X;
            impl.Y = left.Y - right.Y;
            impl.Z = left.Z - right.Z;
            return impl;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl operator -(in Impl value)
        {
            Impl impl;
            impl.X = -value.X;
            impl.Y = -value.Y;
            impl.Z = -value.Z;
            return impl;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl Lerp(in Impl left, in Impl right, float amount)
        {
            Impl impl;
            impl.X = Vector3.Lerp(left.X, right.X, amount);
            impl.Y = Vector3.Lerp(left.Y, right.Y, amount);
            impl.Z = Vector3.Lerp(left.Z, right.Z, amount);
            return impl;
        }
    }
}