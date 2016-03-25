using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Core
{
  /// <summary> Fast/efficient way to check which components are available. </summary>
  public struct ComponentMask : IEquatable<ComponentMask>
  {
    private long _mask;

    public void Include(ComponentId componentId)
    {
      _mask |= (1L << componentId.Id);
    }

    public void Exclude(ComponentId componentId)
    {
      _mask &= ~(1L << componentId.Id);
    }

    #region Equality

    public bool Equals(ComponentMask other)
    {
      return _mask == other._mask;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      return obj is ComponentMask && Equals((ComponentMask)obj);
    }

    public override int GetHashCode()
    {
      // ReSharper disable NonReadonlyMemberInGetHashCode
      return ((int)_mask >> 32) | (int)_mask;
      // ReSharper restore NonReadonlyMemberInGetHashCode
    }

    public static bool operator ==(ComponentMask left, ComponentMask right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(ComponentMask left, ComponentMask right)
    {
      return !left.Equals(right);
    }

    #endregion
  }
}