using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Core
{
  /// <summary>
  ///  A unique id for a specific component.
  /// </summary>
  /// <typeparam name="T"> The type of data that the component holds. </typeparam>
  public sealed class ComponentId<T> : ComponentId
  {
    public ComponentId(int id, string name)
      : base(id, name)
    {
    }
  }
}