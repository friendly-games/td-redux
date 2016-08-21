using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineByteGames.Tdx.Unity
{
  /// <summary> Core object that has a lookup of all Template objects in the unity system. </summary>
  internal class TemplatedObjects
  {
    private readonly Dictionary<string, GameObject> _lookup;

    /// <summary> Constructor. </summary>
    /// <param name="parent"> The object that contains all of the templates/prefabs. </param>
    public TemplatedObjects(GameObject parent)
    {
      _lookup = new Dictionary<string, GameObject>();

      AddChildren(parent);
    }

    /// <summary>
    ///  Adds the child game objects of the given instance to the dictionary lookup.
    /// </summary>
    private void AddChildren(GameObject parent)
    {
      foreach (Transform child in parent.transform)
      {
        _lookup.Add(child.name, child.gameObject);
        AddChildren(child.gameObject);
      }
    }

    /// <summary> Finds the template with the given name. </summary>
    /// <param name="name"> The name of the template to lookup. </param>
    /// <returns> The game object representing the given template. </returns>
    public GameObject FindTemplate(string name)
    {
      return _lookup[name];
    }

    /// <summary> Creates a dictionary lookup that maps a specific enum to another datatype. </summary>
    /// <typeparam name="TEnum"> The type that represents the key into the dictionary. </typeparam>
    /// <typeparam name="TValue"> The type of object that will be the value in the dictionary. </typeparam>
    /// <param name="nameSuffix"> The name to prepend to the string representation of each value of
    ///  TEnum to get the name of the object. </param>
    /// <param name="factory"> The factory used to create instances of TValue for each TEnum value. </param>
    /// <param name="namePrefix"> (Optional) The text to add to the end of the string representation of
    ///  each value of TEnum to get the name of the object. </param>
    /// <returns>
    ///  A dictionary of all the enums represented by TEnum plus the values created by
    ///  <paramref name="factory"/> for each of the enum values.
    /// </returns>
    public Dictionary<TEnum, TValue> CreateLookup<TEnum, TValue>(string nameSuffix,
                                                                 Func<GameObject, TValue> factory,
                                                                 string namePrefix = null)
      where TEnum : struct
    {
      var allOfThem = from TEnum key in Enum.GetValues(typeof(TEnum))
                      select new { key, name = key.ToString(), }
                      into next
                      where next.name != "None"
                      select
                      new
                      {
                        next.key,
                        pool = factory.Invoke(FindTemplate(namePrefix + next.name + nameSuffix))
                      };

      return allOfThem.ToDictionary(it => it.key, it => it.pool);
    }
  }
}