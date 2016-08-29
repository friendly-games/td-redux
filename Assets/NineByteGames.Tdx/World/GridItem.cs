using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.World
{
  /// <summary> An item that takes up one position in the world grid. </summary>
  public struct GridItem
  {
    /// <summary> Constructor. </summary>
    public GridItem(TileType type, byte variant)
    {
      Type = type;
      Variant = variant;

      BuildingType = 0;
      BuildingInstance = 0;
    }

    /// <summary> The type of tile that the grid item represents. </summary>
    public TileType Type;

    /// <summary> The way that the tile is displayed. </summary>
    public byte Variant;

    /// <summary> The index of the building that exists at the specific coordinate. </summary>
    public BuildingType BuildingType;

    /// <summary> The building instance of the building that exists at the specific coordinate. </summary>
    public short BuildingInstance;
  }
}