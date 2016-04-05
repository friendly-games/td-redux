using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Tdx.Input;
using UnityEngine;

namespace NineByteGames.Tdx.Unity
{
  internal class UnityEngineTime : IEngineTime
  {
    public static readonly UnityEngineTime Instance
      = new UnityEngineTime();

    public float DeltaTime
    {
      get { return Time.deltaTime; }
    }
  }
}