using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Core;
using NineByteGames.Tdx.Components;

namespace NineByteGames.Tdx
{
  /// <summary>
  ///  Checks if the entity has healing applied, and if so, restores health at a
  ///  given rate.
  /// </summary>
  public class HealingSystemProcessor : SystemProcessor
  {
    public HealingSystemProcessor()
    {
      Initialize(Update, AllComponents.Health, AllComponents.Healing);
    }

    /// <inheritdoc />
    private void Update(Entity entity,
                        ref Health health,
                        ref Healing healing)
    {
      // make sure we heal the given amount
      if (health.Hp < health.MaxHp)
      {
        int maxAmount = Math.Max(health.MaxHp - health.Hp, healing.HitPointsPerTick);
        health.Hp += maxAmount;
      }
    }
  }
}