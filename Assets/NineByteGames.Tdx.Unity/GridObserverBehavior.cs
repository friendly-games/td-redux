using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Common;
using NineByteGames.Tdx.World;
using UnityEngine;

namespace NineByteGames.Tdx.Unity
{
  /// <summary>
  ///  Observes the grid for changes and adds or removes tiles as necessary.
  /// </summary>
  public class GridObserverBehavior : MonoBehaviour,
                                      IStart,
                                      IUpdate
  {
    private TemplatesBehavior _templates;
    private WorldGrid _worldGrid;
    private Camera _itemToTrack;

    private Dictionary<TileType, UnityObjectPool> _pools;

    public int VisibleWidth = 40;
    public int VisibleHeight = 40;

    private WorldGridSlice<GameObject> _theGridSlice;

    /// <unitymethod />
    public void Start()
    {
      _templates = GetComponent<TemplatesBehavior>();

      _pools = _templates.TemplatesLookup
                         .CreateLookup<TileType, UnityObjectPool>(
                           "TileTemplate",
                           it => new UnityObjectPool(it, gameObject, 50));

      // TODO get the grid from elsewhere
      // TODO don't use the camera
      Initialize(new WorldGrid(), Camera.main);
    }

    /// <summary> Sets up the Observer to watch the given grid. </summary>
    public void Initialize(WorldGrid worldGrid, Camera itemToTrack)
    {
      _itemToTrack = itemToTrack;

      _worldGrid = worldGrid;
      _theGridSlice = new WorldGridSlice<GameObject>(_worldGrid, VisibleWidth, VisibleHeight);
      _theGridSlice.DataChanged += HandleChanged;
      _theGridSlice.Initialize(new GridCoordinate(Vector2.zero));
    }

    private void HandleChanged(SliceUnitData<GameObject> oldData, ref SliceUnitData<GameObject> newData)
    {
      if (oldData.Data != null)
      {
        UnityObjectPool pool = _pools[oldData.GridItem.Type];
        pool.Restore(oldData.Data);
      }

      GridItem item = newData.GridItem;
      var tileTemplatePool = _pools[item.Type];

      var instance = tileTemplatePool.Get();
      instance.transform.position = newData.Position.ToUpperRight(Vector2.zero);

      instance.transform.rotation = Quaternion.Euler(0, 0, item.Variant * 90);

      newData.Data = instance;
    }

    /// <unitymethod />
    public void Update()
    {
      var position = _itemToTrack.transform.position;
      _theGridSlice.Recenter(new GridCoordinate(position));
    }
  }
}