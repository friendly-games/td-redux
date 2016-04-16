using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.World
{
  /// <summary> A 2d array of data. </summary>
  /// <typeparam name="T"> The type of element that the array holds. </typeparam>
  public class Array2D<T>
  {
    /// <summary> Constructor. </summary>
    /// <param name="width"> The desired width of the 2d array. </param>
    /// <param name="height"> The desired height of the 2d array. </param>
    public Array2D(int width, int height)
    {
      Width = width;
      Height = height;

      Data = new T[width * height];
    }

    public T this[int x, int y]
    {
      get { return Data[CalculateIndex(x, y)]; }
      set { Data[CalculateIndex(x, y)] = value; }
    }

    public T this[Array2DIndex index]
    {
      get { return this[index.X, index.Y]; }
      set { this[index.X, index.Y] = value; }
    }

    public T[] Data { get; private set; }

    public int Width { get; private set; }

    public int Height { get; private set; }

    /// <summary> Gets the existing value at the specified index and returns the old value.  </summary>
    public T Swap(int x, int y, T newValue)
    {
      var index = CalculateIndex(x, y);

      var existing = Data[index];
      Data[index] = newValue;

      return existing;
    }

    public T Swap(Array2DIndex index, T newValue)
    {
      return Swap(index.X, index.Y, newValue);
    }

    private int CalculateIndex(int x, int y)
    {
      return y * Width + x;
    }
  }

  /// <summary> A composite index for <see cref="Array2D{T}"/>. </summary>
  public struct Array2DIndex
  {
    /// <summary> The X part of the index. </summary>
    public int X;

    /// <summary> The Y part of the index. </summary>
    public int Y;

    /// <summary> Constructor. </summary>
    /// <param name="x"> The X part of the index. </param>
    /// <param name="y"> The Y part of the index. </param>
    public Array2DIndex(int x, int y)
    {
      X = x;
      Y = y;
    }

    /// <inheritdoc />
    public override string ToString()
    {
      return X + "," + Y;
    }
  }
}