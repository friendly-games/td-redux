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
    private ViewableGrid _viewableGrid;
    private readonly Dictionary<GridCoordinate, GameObject> _lookup = new Dictionary<GridCoordinate, GameObject>();

    public int VisibleWidth = 40;
    public int VisibleHeight = 40;

    public int Count;

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
      _viewableGrid = new ViewableGrid(_worldGrid, VisibleWidth, VisibleHeight);

      _viewableGrid.GridItemAdded += HandleGridItemAdded;
      _viewableGrid.GridItemRemoved += HandleGridItemRemoved;

      _viewableGrid.Recenter(Vector2.zero);
    }

    private void HandleGridItemRemoved(Chunk chunk, GridCoordinate coordinate, GridItem item)
    {
      var existing = _lookup[coordinate];
      _lookup.Remove(coordinate);
      // TODO pool it
      Destroy(existing);

      Count--;
    }

    private void HandleGridItemAdded(Chunk chunk, GridCoordinate coordinate, GridItem item)
    {
      UpdateSprite(coordinate, item);
    }

    /// <unitymethod />
    public void Update()
    {
      var position = _itemToTrack.transform.position;
      _viewableGrid.Recenter(position);
    }

    /// <summary> Updates the sprite for the given item at the given position. </summary>
    private void UpdateSprite(GridCoordinate coordinate, GridItem item)
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