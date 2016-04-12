using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace NineByteGames.Tdx
{
  /// <summary> An object in the game that has a corresponding Visual object. </summary>
  public abstract class CoreObject
  {
    private object _visual;

    /// <summary> Gets the visual typed to the correct data-type. </summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    /// <returns> The visual. </returns>
    [Pure]
    public T GetVisual<T>()
      where T : class
    {
      return (T)_visual;
    }

    /// <summary> Sets the visual data associated with the object. </summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    /// <param name="value"> The visual value of the object. </param>
    public void SetVisual<T>(T value)
      where T : class
    {
      _visual = value;
    }
  }
}