﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NineByteGames.Common
{
  /// <summary> Extension methods to unity classes. </summary>
  public static class UnityExtensions
  {
    /// <summary> Set the given GameObject to be the parent of the other object. </summary>
    public static void SetParent(this GameObject instance, GameObject parent)
    {
      instance.transform.parent = parent.transform;
    }

    /// <summary> Create a copy of the given object. </summary>
    public static GameObject Clone(this GameObject instance)
    {
      return Object.Instantiate(instance);
    }

    /// <summary> Create a copy of the given object. </summary>
    public static GameObject Clone(this GameObject instance, Vector3 position)
    {
      return (GameObject)Object.Instantiate(instance, position, Quaternion.identity);
    }

    /// <summary> Create a copy of the given object. </summary>
    public static GameObject Clone(this GameObject instance, Vector3 position, Quaternion quaternion)
    {
      return (GameObject)Object.Instantiate(instance, position, quaternion);
    }
  }
}