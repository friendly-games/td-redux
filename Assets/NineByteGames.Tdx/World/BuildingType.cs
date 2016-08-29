using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.World
{
  /// <summary> The type of building that is present in the world. </summary>
  public enum BuildingType : short
  {
    /// <summary> Special cased to indicate that no building is present. </summary>
    None,
    TreeStump,
  }
}