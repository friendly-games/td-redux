using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Common;
using NineByteGames.Tdx.Input;
using UnityEngine;

namespace NineByteGames.Tdx.Unity
{
  public class UnityPlayerBehavior : MonoBehaviour,
                                     IUpdate,
                                     IStart,
                                     IEngineObject
  {
    private PlayerMover _playerMover;

    [SerializeField]
    [Tooltip("The speed at which the player should move")]
    public float Speed = 1.0f;

    /// <inheritdoc />
    public void Start()
    {
      _playerMover = new PlayerMover(this, UnityEngineTime.Instance)
                     {
                       MoveForward = new UnityKeyDownAction(KeyCode.W),
                       MoveBackward = new UnityKeyDownAction(KeyCode.S),
                       MoveLeft = new UnityKeyDownAction(KeyCode.A),
                       MoveRight = new UnityKeyDownAction(KeyCode.D),
                     };
    }

    /// <inheritdoc />
    public Vector3 Position
    {
      get { return transform.position; }
      set { transform.position = value; }
    }

    /// <inheritdoc />
    public void Update()
    {
      _playerMover.Update(Speed);
    }
  }
}