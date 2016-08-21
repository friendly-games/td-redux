using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Common;
using UnityEngine;

namespace NineByteGames.Tdx.World
{
  /// <summary> Contains all aspects that a tile can take. </summary>
  [Serializable]
  public class TileDescriptor : IUnityExposed
  {
    public TileType Name;
    public GameObject Template;
  }
}