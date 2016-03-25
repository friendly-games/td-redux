using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Core
{
  /// <summary> Stores a list of components that are present for a given entity. </summary>
  public class ComponentList
  {
    private readonly Dictionary<ComponentId, IComponentReference> _components;
    private ComponentMask _componentMask;

    public ComponentList()
    {
      _components = new Dictionary<ComponentId, IComponentReference>();
      _componentMask = new ComponentMask();
    }

    /// <summary> Adds a component to the entity. </summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    public void AddComponent<T>(ComponentId<T> id, T value)
    {
      _components.Add(id, new ComponentReference<T>());
      _componentMask.Include(id);
    }

    /// <summary> Gets a reference to the component for the current entity. </summary>
    public ComponentReference<T> GetComponentReference<T>(ComponentId<T> id)
    {
      IComponentReference untypedComponent;
      if (_components.TryGetValue(id, out untypedComponent))
      {
        return (ComponentReference<T>)untypedComponent;
      }

      return null;
    }
  }
}