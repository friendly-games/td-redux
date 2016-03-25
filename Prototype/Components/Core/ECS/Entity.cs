using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Core
{
  /// <summary>  </summary>
  public class Entity : IEnumerable
  {
    private readonly ComponentList _components;

    public Entity()
    {
      // TODO pool these
      // TODO figure out better storage for the components
      _components = new ComponentList();
    }

    public Entity Clone()
    {
      throw new NotImplementedException();
    }

    /// <summary> Adds a component to the entity. </summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    public void Add<T>(ComponentId<T> id, T value)
    {
      _components.AddComponent(id, value);
    }

    /// <summary> Gets a reference to the component for the current entity. </summary>
    public ComponentReference<T> GetComponentReference<T>(ComponentId<T> id)
    {
      return _components.GetComponentReference(id);
    }

    /// <summary> Fake implementation to allow collection initializer. </summary>
    public IEnumerator GetEnumerator()
    {
      throw new NotImplementedException();
    }
  }
}