using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.Input
{
  /// <summary> Placeholders until an actual implementation can be assigned. </summary>
  public static class InputPlaceholders
  {
    public static readonly IKeyDownAction NullKeyDown
      = new NullInput();

    private class NullInput : IKeyDownAction
    {
      public bool IsDown
      {
        get { return false; }
      }
    }
  }
}