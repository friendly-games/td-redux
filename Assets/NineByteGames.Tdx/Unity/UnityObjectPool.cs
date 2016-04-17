using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Common;
using UnityEngine;

namespace NineByteGames.Tdx.Unity
{
  /// <summary>
  ///  An object pool that keeps GameObjects and automatically activates/deactivates the instances
  ///  as needed.
  /// </summary>
  internal class UnityObjectPool : ObjectPool<GameObject>
  {
    private readonly GameObject _templateGameObject;
    private readonly GameObject _parent;

    /// <summary> Constructor. </summary>
    /// <param name="templateGameObject"> The object to be cloned when a new instance is needed. </param>
    /// <param name="parent"> The GameObject who should be the parent of all created instances. </param>
    /// <param name="limit"> The max-number of instances. </param>
    public UnityObjectPool(GameObject templateGameObject, GameObject parent, int limit = 1024)
      : base(limit)
    {
      _templateGameObject = templateGameObject;
      _parent = parent;
    }

    /// <inheritdoc />
    protected override GameObject CreateInstance()
    {
      var instance = _templateGameObject.Clone();
      instance.SetParent(_parent);
      return instance;
    }

    /// <inheritdoc />
    public override GameObject Get()
    {
      var instance = base.Get();
      instance.SetActive(true);
      return instance;
    }

    /// <inheritdoc />
    public override void Restore(GameObject instance)
    {
      instance.SetActive(false);
      base.Restore(instance);
    }
  }
}