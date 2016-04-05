using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Assets.NineByteGames.Tdx.Input;
using UnityEngine;

namespace NineByteGames.Tdx.Input
{
  /// <summary> IsDown for when a keyboard key is pressed. </summary>
  public class UnityKeyDownAction : IKeyDownAction
  {
    private readonly KeyCode _keyCode;

    public UnityKeyDownAction(KeyCode keyCode)
    {
      _keyCode = keyCode;
    }

    /// <inheritdoc />
    public virtual bool IsDown
    {
      get { return UnityEngine.Input.GetKey(_keyCode); }
    }
  }
}