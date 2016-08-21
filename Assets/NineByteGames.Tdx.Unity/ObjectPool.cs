using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.Unity
{
  /// <summary> Pools objects to reduce memory pressure during run time. </summary>
  /// <typeparam name="T"> The type of object that is pooled. </typeparam>
  public abstract class ObjectPool<T>
  {
    private readonly int _limit;
    private readonly Queue<T> _instances;

    /// <summary> Specialised constructor for use only by derived class. </summary>
    /// <param name="limit"> The maximum number of instances that should be cached. </param>
    protected ObjectPool(int limit)
    {
      _limit = limit;
      _instances = new Queue<T>();
    }

    /// <summary> Creates a new instance of the given type. </summary>
    /// <returns> The new instance. </returns>
    protected abstract T CreateInstance();

    /// <summary>
    ///  Retrieves an instance from the pool.  If the pool already has an instance waiting to be used,
    ///  it will be returned.  If the pool does not have any spare instances a new one will be created.
    /// </summary>
    /// <returns> The instance to use. </returns>
    public virtual T Get()
    {
      if (_instances.Count > 0)
      {
        return _instances.Dequeue();
      }

      return CreateInstance();
    }

    /// <summary>
    ///  Restores an instance to the pool, allowing the next invocation of <see cref="Get"/> to return
    ///  the instance.
    /// </summary>
    /// <param name="instance"> The instance to restore. </param>
    public virtual void Restore(T instance)
    {
      if (_instances.Count < _limit)
      {
        _instances.Enqueue(instance);
      }
    }
  }
}