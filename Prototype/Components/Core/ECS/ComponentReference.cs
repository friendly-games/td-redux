using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Core
{
  /// <summary> Identification interface for all component references. </summary>
  public interface IComponentReference
  {
  }

  /// <summary> Represents a specific type of component. </summary>
  /// <typeparam name="T"> Generic type parameter. </typeparam>
  public sealed class ComponentReference<T> : IComponentReference
  {
    public T Value;
  }
}