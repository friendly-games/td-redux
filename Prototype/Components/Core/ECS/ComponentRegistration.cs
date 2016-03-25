using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NineByteGames.Core
{
  /// <summary> Provides methods to register the components in the system. </summary>
  public static class ComponentRegistration
  {
    private static int _uniqueId;

    public static void Reset()
    {
      _uniqueId = 0;
    }

    public static void Initialize<T>(out ComponentId<T> componentId, string name)
    {
      int uniqueId = _uniqueId;
      componentId = new ComponentId<T>(uniqueId, name);
      _uniqueId++;
    }

    public static void Initialize<T>(out ComponentId<T> componentId, Expression<Func<ComponentId<T>>> propName)
    {
      Initialize(out componentId, propName.ToString());
    }

    public static void Initialize<T>(Expression<Func<ComponentId<T>>> propName)
    {
      var fieldInfo = (FieldInfo)((MemberExpression)propName.Body).Member;
      var name = fieldInfo.Name;

      int uniqueId = _uniqueId;
      var componentId = new ComponentId<T>(uniqueId, name);
      _uniqueId++;

      // assign it
      fieldInfo.SetValue(null, componentId);
    }
  }
}