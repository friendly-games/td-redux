using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.Input
{
  /// <summary> Represents the time in the engine. </summary>
  public interface IEngineTime
  {
    float DeltaTime { get; }
  }
}