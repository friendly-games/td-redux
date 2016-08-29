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

    private Dictionary<TileType, UnityObjectPool> _tilePools;
    private Dictionary<BuildingType, UnityObjectPool> _buildingPools;

    public int VisibleWidth = 40;
    public int VisibleHeight = 40;

    public GameObject TilesParent;
    public GameObject BuildingsParent;

    private WorldGridSlice<ViewGridItemData> _theGridSlice;

    /// <unitymethod />
    public void Start()
    {
      _templates = GetComponent<TemplatesBehavior>();

      _tilePools = _templates.TemplatesLookup
                             .CreateLookup<TileType, UnityObjectPool>(
                               "TileTemplate",
                               it =>
                                 new UnityObjectPool(it,
                                                     TilesParent,
                                                     Chunk.NumberOfGridItemsHigh * Chunk.NumberOfGridItemsWide));

      _buildingPools = _templates.TemplatesLookup
                                 .CreateLookup<BuildingType, UnityObjectPool>(
                                   "Template",
                                   it =>
                                     new UnityObjectPool(it,
                                                         BuildingsParent,
                                                         Chunk.NumberOfGridItemsWide));

      // TODO get the grid from elsewhere
      // TODO don't use the camera
      Initialize(new WorldGrid(), Camera.main);
    }

    /// <summary> Sets up the Observer to watch the given grid. </summary>
    public void Initialize(WorldGrid worldGrid, Camera itemToTrack)
    {
      _itemToTrack = itemToTrack;

      _worldGrid = worldGrid;
      _theGridSlice = new WorldGridSlice<ViewGridItemData>(_worldGrid, VisibleWidth, VisibleHeight);
      _theGridSlice.DataChanged += HandleChanged;
      _theGridSlice.Initialize(new GridCoordinate(Vector2.zero));
    }

    private void HandleChanged(SliceUnitData<ViewGridItemData> oldData, ref SliceUnitData<ViewGridItemData> newData)
    {
      if (oldData.Data.IsValid)
      {
        UnityObjectPool tilePool = _tilePools[oldData.GridItem.Type];
        tilePool.Restore(oldData.Data.Tile);

        if (oldData.GridItem.BuildingType != BuildingType.None)
        {
          UnityObjectPool buildingPool = _buildingPools[oldData.GridItem.BuildingType];
          buildingPool.Restore(oldData.Data.Building);
        }
      }

      GridItem item = newData.GridItem;
      UnityObjectPool tileTemplatePool = _tilePools[item.Type];

      GameObject tileInstance = tileTemplatePool.Get();
      tileInstance.transform.position = newData.Position.ToUpperRight(Vector2.zero);
      tileInstance.transform.rotation = Quaternion.Euler(0, 0, item.Variant * 90);

      var newInstance = new ViewGridItemData();
      newInstance.Tile = tileInstance;

      if (newData.GridItem.BuildingType != BuildingType.None)
      {
        UnityObjectPool buildingPool = _buildingPools[newData.GridItem.BuildingType];
        var buildingInstance = buildingPool.Get();
        buildingInstance.transform.position = newData.Position.ToUpperRight(Vector2.zero);
        newInstance.Building = buildingInstance;
      }

      newData.Data = newInstance;
    }

    /// <unitymethod />
    public void Update()
    {
      var position = _itemToTrack.transform.position;
      _theGridSlice.Recenter(new GridCoordinate(position));
    }

    private struct ViewGridItemData
    {
      public GameObject Tile;
      public GameObject Building;

      public bool IsValid
      {
        get { return Tile != null; }
      }
    }
  }
}