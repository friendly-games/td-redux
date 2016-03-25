using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Core
{
  public abstract class ComponentId
  {
    protected ComponentId(int id, string name)
    {
      Id = id;
      Name = name;
    }

    public int Id { get; private set; }

    public string Name { get; private set; }
  }
}