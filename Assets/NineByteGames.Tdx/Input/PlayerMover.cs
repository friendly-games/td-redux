using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Assets.NineByteGames.Tdx.Input;
using UnityEngine;

namespace NineByteGames.Tdx.Input
{
  /// <summary> Represents a single-player in the engine. </summary>
  public interface IEnginePlayer
  {
    Vector3 Position { get; set; }
  }

  /// <summary> Represents the time in the engine. </summary>
  public interface IEngineTime
  {
    float DeltaTime { get; }
  }

  /// <summary> A player mover. </summary>
  public class PlayerMover
  {
    private readonly IEnginePlayer _enginePlayer;
    private readonly IEngineTime _engineTime;

    public PlayerMover(IEnginePlayer enginePlayer, IEngineTime engineTime)
    {
      _enginePlayer = enginePlayer;
      _engineTime = engineTime;
    }

    /// <summary> Moves the player forward. </summary>
    public IKeyDownAction MoveForward
      = InputPlaceholders.NullKeyDown;

    /// <summary> Moves the player backward. </summary>
    public IKeyDownAction MoveBackward
      = InputPlaceholders.NullKeyDown;

    /// <summary> Moves the player to the left. </summary>
    public IKeyDownAction MoveLeft
      = InputPlaceholders.NullKeyDown;

    /// <summary> Moves the player to the right. </summary>
    public IKeyDownAction MoveRight
      = InputPlaceholders.NullKeyDown;

    /// <summary> Re-evaluate the position of the player. </summary>
    /// <param name="speed"> The speed at which the player should move. </param>
    public void Update(float speed)
    {
      var desiredVelocity = CheckMovementInput();

      if (desiredVelocity.sqrMagnitude > 0.001f)
      {
        _enginePlayer.Position += desiredVelocity * _engineTime.DeltaTime * speed;
      }
    }

    /// <summary> Find the desired movment for the user. </summary>
    /// <returns> A Vector3. </returns>
    public Vector3 CheckMovementInput()
    {
      var desiredVelocity = new DesiredVelocity();

      if (MoveForward.IsDown)
      {
        desiredVelocity.Forward();
      }

      if (MoveBackward.IsDown)
      {
        desiredVelocity.Backward();
      }

      if (MoveLeft.IsDown)
      {
        desiredVelocity.Left();
      }

      if (MoveRight.IsDown)
      {
        desiredVelocity.Right();
      }

      return desiredVelocity.Velocity;
    }
  }
}