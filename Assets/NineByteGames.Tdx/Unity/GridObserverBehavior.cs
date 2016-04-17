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
    //private ViewableGrid _viewableGrid;
    private readonly Dictionary<GridCoordinate, GameObject> _lookup = new Dictionary<GridCoordinate, GameObject>();

    public int VisibleWidth = 40;
    public int VisibleHeight = 40;

    public int Count;
    private ViewableGrid _theGrid;

    /// <unitymethod />
    public void Start()
    {
      _templates = GetComponent<TemplatesBehavior>();

      // TODO get the grid from elsewhere
      // TODO don't use the camera
      Initialize(new WorldGrid(), Camera.main);
    }

    /// <summary> Sets up the Observer to watch the given grid. </summary>
    public void Initialize(WorldGrid worldGrid, Camera itemToTrack)
    {
      _itemToTrack = itemToTrack;

      _worldGrid = worldGrid;
      _theGrid = new ViewableGrid(_worldGrid, VisibleWidth, VisibleHeight);
      _theGrid.DataChanged += HandleChanged;
      _theGrid.Initialize(new GridCoordinate(Vector2.zero));
    }

    private void HandleChanged(ViewableGrid.StoredGridData oldData, ViewableGrid.StoredGridData newData)
    {
      GameObject existing;
      if (_lookup.TryGetValue(oldData.Position, out existing))
      {
        // TODO check if it's actually valid data

        // TODO pool it
        _lookup.Remove(oldData.Position);
        Destroy(existing);
      }

      Count--;

      Add(newData.Position, newData.Data);
    }

    /// <unitymethod />
    public void Update()
    {
      var position = _itemToTrack.transform.position;
      //_viewableGrid.Recenter(position);
      _theGrid.Recenter(new GridCoordinate(position));
    }

    /// <summary> Updates the sprite for the given item at the given position. </summary>
    private void Add(GridCoordinate coordinate, GridItem item)
    {
      var tileTemplate = _templates.Tiles.First(t => t.Name == item.Type);
      var template = tileTemplate.Template;

      // TODO don't create a new object each time.
      var newObject = template.Clone(coordinate.ToUpperRight(Vector2.zero));
      newObject.SetParent(gameObject);

      _lookup.Add(coordinate, newObject);
      Count++;
    }
  }
}