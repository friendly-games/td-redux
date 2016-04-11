using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.World
{
  internal static class MathUtils
  {
    /// <summary> Gets the of the value, always ensuring the value is positive. </summary>
    /// <param name="value"> The value to take the remainder of.. </param>
    /// <param name="divisor"> The divisor. </param>
    /// <returns> The positive modulus of the value. </returns>
    public static int PositiveRemainder(int value, int divisor)
    {
      return ((value % divisor) + divisor) % divisor;
    }
  }
}