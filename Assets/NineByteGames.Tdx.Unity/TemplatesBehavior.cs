﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineByteGames.Tdx.Unity
{
  /// <summary> Contains all of the templates in the world. </summary>
  public class TemplatesBehavior : MonoBehaviour
  {
    [Tooltip("The GameObject that contains all of the prefabs as children")]
    public GameObject PrefabsTarget;

    public void Start()
    {
      TemplatesLookup = new TemplatedObjects(PrefabsTarget);
    }

    internal TemplatedObjects TemplatesLookup { get; set; }
  }
}