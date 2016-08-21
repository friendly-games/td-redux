using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.Unity.View
{
  /// <summary> Represents the view for an object showing health. </summary>
  public interface IHealthView
  {
    /// <summary> Update the health on-screen to the value specified. </summary>
    /// <param name="health"> The health of the current object, on a scale from 0-100. </param>
    void SetHealth(float health);
  }
}