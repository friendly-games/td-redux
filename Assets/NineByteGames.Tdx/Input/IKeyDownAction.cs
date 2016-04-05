using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.Input
{
  /// <summary> Represents a key that is pressed. </summary>
  public interface IKeyDownAction
  {
    /// <summary>
    ///  True if the key is down, false otherwise.
    /// </summary>
    bool IsDown { get; }
  }
}