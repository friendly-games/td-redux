using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Core
{
  /// <summary>
  ///  A bitwise mask which marks which systems an entity is part of.
  /// </summary>
  public struct IncludedSystemMask
  {
    private long _mask;

    public void Include(byte id)
    {
      _mask |= (1L << id);
    }

    public void Exclude(byte id)
    {
      _mask &= ~(1L << id);
    }

    #region Equality

    public bool Equals(IncludedSystemMask other)
    {
      return _mask == other._mask;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      return obj is IncludedSystemMask && Equals((IncludedSystemMask)obj);
    }

    public override int GetHashCode()
    {
      // ReSharper disable NonReadonlyMemberInGetHashCode
      return ((int)_mask >> 32) | (int)_mask;
      // ReSharper restore NonReadonlyMemberInGetHashCode
    }

    public static bool operator ==(IncludedSystemMask left, IncludedSystemMask right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(IncludedSystemMask left, IncludedSystemMask right)
    {
      return !left.Equals(right);
    }

    #endregion
  }
}