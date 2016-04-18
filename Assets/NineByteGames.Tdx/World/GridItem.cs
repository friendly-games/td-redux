using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.World
{
  /// <summary> An item that takes up one position in the world grid. </summary>
  public struct GridItem
  {
    /// <summary> Constructor. </summary>
    public GridItem(TileType type, byte variant = 0)
    {
      Type = type;
      Variant = variant;
    }

    /// <summary> The type of tile that the grid item represents. </summary>
    public TileType Type;

    /// <summary> The way that the tile is displayed. </summary>
    public byte Variant;
  }
}