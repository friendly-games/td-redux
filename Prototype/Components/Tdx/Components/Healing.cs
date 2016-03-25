using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.Components
{
  /// <summary> Represents an entity which continually heals. </summary>
  public struct Healing
  {
    /// <summary> The number of HP restored per tick. </summary>
    public int HitPointsPerTick;
  }
}