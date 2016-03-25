using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.Components
{
  /// <summary>
  ///  Represents the current and maximum health that a unit can have.
  /// </summary>
  public struct Health
  {
    /// <summary> The number of hitpoints that the entity currently has. </summary>
    public int Hp;

    /// <summary> The maximum HP that the entity can have. </summary>
    public int MaxHp;
  }
}