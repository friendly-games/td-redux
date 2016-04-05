using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Common
{
  /// <summary> MonoBehavior with a Start() method. </summary>
  public interface IStart
  {
    /// <summary> Invoked when the object is initialized. </summary>
    void Start();
  }

  /// <summary> Represents a MonoBehavior that is updated per tick. </summary>
  public interface IUpdate
  {
    void Update();
  }
}