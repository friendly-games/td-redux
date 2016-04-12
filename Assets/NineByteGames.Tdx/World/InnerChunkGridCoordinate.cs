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
  }
}