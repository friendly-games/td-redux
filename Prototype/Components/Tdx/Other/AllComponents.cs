using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Core;
using NineByteGames.Tdx.Components;

namespace NineByteGames.Tdx
{
  public static class AllComponents
  {
    // ReSharper disable UnassignedReadonlyField
    public static readonly ComponentId<Health> Health;
    public static readonly ComponentId<Healing> Healing;
    public static readonly ComponentId<string> Name;
    // ReSharper restore UnassignedReadonlyField

    static AllComponents()
    {
      ComponentRegistration.Reset();

      ComponentRegistration.Initialize(() => Health);
      ComponentRegistration.Initialize(() => Name);
      ComponentRegistration.Initialize(() => Healing);
    }
  }
}