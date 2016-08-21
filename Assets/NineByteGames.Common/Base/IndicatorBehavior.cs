using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineByteGames.Common.Base
{
  /// <summary>
  ///  Represents a behavior whose sole existence is meant to indicate that the object has some data
  ///  that will be inspected, but then the entire behavior should be removed after inspection is
  ///  complete.
  /// </summary>
  public abstract class IndicatorBehavior : MonoBehaviour,
                                            IDisposable
  {
    /// <summary> Mark the behavior as completed for its purpose. </summary>
    public void Dispose()
    {
      Destroy(this);
    }
  }
}