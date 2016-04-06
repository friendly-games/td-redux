using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.World
{
  /// <summary> A coordinate to a chunk. </summary>
  public struct ChunkCoordinate : IEquatable<ChunkCoordinate>
  {
    public int X;
    public int Y;

    public ChunkCoordinate(int x, int y)
    {
      X = x;
      Y = y;
    }

    public bool Equals(ChunkCoordinate other)
    {
      return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      return obj is ChunkCoordinate && Equals((ChunkCoordinate)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (X * 397) ^ Y;
      }
    }

    public static bool operator ==(ChunkCoordinate left, ChunkCoordinate right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(ChunkCoordinate left, ChunkCoordinate right)
    {
      return !left.Equals(right);
    }
  }
}