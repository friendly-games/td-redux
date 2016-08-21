using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineByteGames.Tdx.Input
{
  /// <summary> Represents a single-object that has a position in the engine. </summary>
  public interface IEngineObject
  {
    Vector3 Position { get; set; }
  }
}