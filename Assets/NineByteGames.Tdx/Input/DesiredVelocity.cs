using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineByteGames.Assets.NineByteGames.Tdx.Input
{
  /// <summary> Holds the velocity that the user would like to move in. </summary>
  internal struct DesiredVelocity
  {
    private Vector3 _desiredVelocity;

    /// <summary> Increment the forward velocity by 1. </summary>
    public void Forward()
    {
      _desiredVelocity.y += 1;
    }

    /// <summary> Decrement the forward velocity by 1. </summary>
    public void Backward()
    {
      _desiredVelocity.y -= 1;
    }

    /// <summary> Decrement the side velocity by 1 (in right direction). </summary>
    public void Left()
    {
      _desiredVelocity.x -= 1;
    }

    /// <summary> Increment the side velocity by 1 (in right direction). </summary>
    public void Right()
    {
      _desiredVelocity.x += 1;
    }

    public Vector3 Velocity
    {
      get { return _desiredVelocity; }
    }
  }
}