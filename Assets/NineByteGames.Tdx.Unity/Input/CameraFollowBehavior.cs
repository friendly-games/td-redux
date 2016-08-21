using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Common;
using UnityEngine;

namespace NineByteGames.Tdx.Unity.Input
{
  /// <summary> Has the camera follow a specific object. </summary>
  public class CameraFollowBehavior : MonoBehaviour,
                                      IUpdate
  {
    [Tooltip("The object that the camera should follow")]
    public GameObject Target;

    public void Update()
    {
      if (Target == null)
        return;

      // TODO give a little "leeway"
      var estimatedPosition = Target.transform.position;
      // never change Z
      var cameraTransform = gameObject.transform;
      estimatedPosition.z = cameraTransform.position.z;
      cameraTransform.position = estimatedPosition;
    }
  }
}