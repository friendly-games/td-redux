using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Core;

namespace NineByteGames.Tdx.Messages
{
  /// <summary> Processes a specific type of <see cref="IMessage"/> for an entity. </summary>
  public abstract class MessagePipeline<TMessage>
    where TMessage : IMessage
  {
    /// <summary> Processes the given entity with the given data. </summary>
    /// <param name="entity"> The entity for which the message is valid. </param>
    /// <param name="message"> The message to process. </param>
    public abstract void Process(Entity entity, TMessage message);
  }

  /// <summary> Indicates that an entity has lost of it's health. </summary>
  public class DeathMessage : IMessage
  {
    /// <summary> The cause of death. </summary>
    public string Cause;
  }
}