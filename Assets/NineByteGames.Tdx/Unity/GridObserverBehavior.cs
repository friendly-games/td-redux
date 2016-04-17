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

    public int Count;
    private ViewableGrid<GameObject> _theGrid;

    /// <unitymethod />
    public void Start()
    {
      _templates = GetComponent<TemplatesBehavior>();

      _pools = _templates.Tiles.ToDictionary(t => t.Name,
                                             t => new UnityObjectPool(t.Template, gameObject, VisibleHeight * VisibleHeight));

      // TODO get the grid from elsewhere
      // TODO don't use the camera
      Initialize(new WorldGrid(), Camera.main);

    }

    /// <summary> Sets up the Observer to watch the given grid. </summary>
    public void Initialize(WorldGrid worldGrid, Camera itemToTrack)
    {
      _itemToTrack = itemToTrack;

      _worldGrid = worldGrid;
      _theGrid = new ViewableGrid<GameObject>(_worldGrid, VisibleWidth, VisibleHeight);
      _theGrid.DataChanged += HandleChanged;
      _theGrid.Initialize(new GridCoordinate(Vector2.zero));
    }

    private void HandleChanged(StoredGridData<GameObject> oldData, ref StoredGridData<GameObject> newData)
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

      newData.Data = instance;
    }

    /// <unitymethod />
    public void Update()
    {
      var position = _itemToTrack.transform.position;
      _theGrid.Recenter(new GridCoordinate(position));
    }
  }
}