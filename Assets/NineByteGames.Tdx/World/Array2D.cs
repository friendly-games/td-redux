﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.World
{
  /// <summary> A 2d array of data. </summary>
  /// <typeparam name="T"> The type of element that the array holds. </typeparam>
  public class Array2D<T>
  {
    public T this[int x, int y]
    {
      get { return Data[CalculateIndex(x, y)]; }
      set { Data[CalculateIndex(x, y)] = value; }
    }

    public T[] Data { get; private set; }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public Array2D(int width, int height)
    {
      Width = width;
      Height = height;

      Data = new T[width * height];
    }

    private int _elementsWide;

    private int CalculateIndex(int x, int y)
    {
      return y * _elementsWide + x;
    }
  }
}