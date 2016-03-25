using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Core;
using NineByteGames.Tdx.Messages;

namespace NineByteGames.Tdx
{
  /// <summary> Manages Death messages for a given entity. </summary>
  public class DeathMessagePipeline : MessagePipeline<DeathMessage>
  {
    public override void Process(Entity entity, DeathMessage message)
    {
    }
  }
}