using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineByteGames.Tdx.World
{
  /// <summary> Represents a position in the world grid. </summary>
  public struct GridCoordinate : IEquatable<GridCoordinate>
  {
    public int X;
    public int Y;

    public GridCoordinate(int x, int y)
    {
      X = x;
      Y = y;
    }

    public GridCoordinate(ChunkCoordinate chunkCoordinate, InnerChunkGridCoordinate innerCoordinate)
    {
      X = (chunkCoordinate.X << Chunk.XGridCoordinateToChunkCoordinateBitShift) + innerCoordinate.X;
      Y = (chunkCoordinate.Y << Chunk.YGridCoordinateToChunkCoordinateBitShift) + innerCoordinate.Y;
    }

    public GridCoordinate(Vector2 position)
    {
      X = (int)Mathf.Floor(position.x);
      Y = (int)Mathf.Floor(position.y);
    }

    public Vector2 ToUpperRight(Vector2 offset)
    {
      return new Vector2(offset.x + X, offset.y + Y);
    }

    /// <summary>
    ///  The coordinate within the chunk identified by
    ///  <see cref="ChunkCoordinate"/> that this GridCoordinate represents.
    /// </summary>
    public InnerChunkGridCoordinate InnerChunkGridCoordinate
    {
      get
      {
        return new InnerChunkGridCoordinate(X & Chunk.GridItemsXCoordinateBitmask,
                                            Y & Chunk.GridItemsYCoordinateBitmask);
      }
    }

    /// <summary> The coordinate of the chunk that this GridCoordinate represents. </summary>
    public ChunkCoordinate ChunkCoordinate
    {
      get
      {
        return new ChunkCoordinate(X >> Chunk.XGridCoordinateToChunkCoordinateBitShift,
                                   Y >> Chunk.YGridCoordinateToChunkCoordinateBitShift);
      }
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

    public static bool operator ==(GridCoordinate left, GridCoordinate right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(GridCoordinate left, GridCoordinate right)
    {
      return !left.Equals(right);
    }

    /// <inheritdoc />
    public override string ToString()
    {
      return X + "," + Y;
    }
  }
}