using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineByteGames.Tdx.World
{
  /// <summary> Contains all aspects that a tile can take. </summary>
  [Serializable]
  public class TileDescriptor
  {
    public TileType Name;
    public GameObject Template;
  }
}