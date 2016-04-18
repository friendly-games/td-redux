using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.World
{
  /// <summary> The type of tile that is present in the world. </summary>
  public enum TileType
  {
    /// <summary> Special cased to indicate that no tile is present. </summary>
    None,
    Water,
    Path,
    Hill,
  }
}