using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Tdx.World;
using UnityEngine;

namespace NineByteGames.Tdx.Unity
{
  /// <summary> Contains all of the templates in the world. </summary>
  public class TemplatesBehavior : MonoBehaviour
  {
    [SerializeField]
    public TileDescriptor[] Tiles;
  }
}