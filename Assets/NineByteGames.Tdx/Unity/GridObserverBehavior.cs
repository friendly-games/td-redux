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
                                      IStart
  {
    private ViewableGrid _viewableGrid;
    private TemplatesBehavior _templates;
    private WorldGrid _worldGrid;
    private Camera _itemToTrack;

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
      _viewableGrid = new ViewableGrid(_worldGrid);
      _viewableGrid.GridItemChanged += HandleGridItemChanged;
      _viewableGrid.ViewableChunkChanged += HandleVisibleChunkChanged;
      _viewableGrid.Recenter(Vector2.zero);
    }

    public void Update()
    {
      var position = _itemToTrack.transform.position;
      _viewableGrid.Recenter(position);
    }

    private void HandleVisibleChunkChanged(Chunk oldchunk, Chunk newChunk)
    {
      if (newChunk == null)
        return;

      for (int x = 0; x < Chunk.NumberOfGridItemsWide; x++)
      {
        for (int y = 0; y < Chunk.NumberOfGridItemsHigh; y++)
        {
          var position = new GridCoordinate(newChunk.Position, new InnerChunkGridCoordinate(x, y));
          var item = newChunk[position.InnerChunkGridCoordinate];

          UpdateSprite(newChunk, position, item);
        }
      }
    }

    /// <summary> Callback to invoke when a GridItem changes. </summary>
    private void HandleGridItemChanged(Chunk chunk, GridCoordinate coordinate, GridItem oldvalue, GridItem newvalue)
    {
      UpdateSprite(chunk, coordinate, newvalue);
    }

    /// <summary> Updates the sprite for the given item at the given position. </summary>
    private void UpdateSprite(Chunk chunk, GridCoordinate coordinate, GridItem item)
    {
      var tileTemplate = _templates.Tiles.First(t => t.Name == item.Type);
      var template = tileTemplate.Template;

      // TODO don't create a new object each time.
      // TODO remove old items
      var newObject = template.Clone(coordinate.ToUpperRight(Vector2.zero));
      newObject.SetParent(gameObject);
    }
  }
}