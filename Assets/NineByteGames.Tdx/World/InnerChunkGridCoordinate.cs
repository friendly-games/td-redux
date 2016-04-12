using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.World
{
  /// <summary> Represents a position of a grid item within a chunk. </summary>
  public struct InnerChunkGridCoordinate
  {
    public int X;
    public int Y;

    public InnerChunkGridCoordinate(int x, int y)
    {
      X = x;
      Y = y;
    }

    public override string ToString()
    {
      return X + "," + Y;
    }

    public bool Equals(GridCoordinate other)
    {
      return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      return obj is GridCoordinate && Equals((GridCoordinate)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (X * 397) ^ Y;
      }
    }

    public static bool operator ==(InnerChunkGridCoordinate left, InnerChunkGridCoordinate right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(InnerChunkGridCoordinate left, InnerChunkGridCoordinate right)
    {
      return !left.Equals(right);
    }
  }
}