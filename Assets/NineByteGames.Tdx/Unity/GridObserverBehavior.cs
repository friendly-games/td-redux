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
    private readonly Queue<EnqueuedChunk> _chunksToProcess = new Queue<EnqueuedChunk>();

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

      if (_chunksToProcess.Count > 0)
      {
        bool didComplete;
        var item = _chunksToProcess.Peek();

        if (item.IsRemoving)
        {
          didComplete = ContinueRemoving(item);
        }
        else
        {
          didComplete = ContinueAdding(item);
        }

        if (didComplete)
        {
          _chunksToProcess.Dequeue();
        }
      }
    }

    private void HandleVisibleChunkChanged(Chunk oldchunk, Chunk newChunk)
    {
      if (oldchunk != null)
      {
        var oldVisual = oldchunk.GetVisual<ChunkVisuals>();
        // TODO remove the actual tiles
        oldVisual.IsRemoved = true;

        _chunksToProcess.Enqueue(new EnqueuedChunk(oldchunk, true));
      }

      if (newChunk != null)
      {
        var visuals = new ChunkVisuals();
        newChunk.SetVisual(visuals);

        _chunksToProcess.Enqueue(new EnqueuedChunk(newChunk, false));
      }
    }

    /// <summary> Callback to invoke when a GridItem changes. </summary>
    private void HandleGridItemChanged(Chunk chunk, GridCoordinate coordinate, GridItem oldvalue, GridItem newvalue)
    {
      UpdateSprite(coordinate, newvalue, chunk.GetVisual<ChunkVisuals>());
    }

    /// <summary> Updates the sprite for the given item at the given position. </summary>
    private void UpdateSprite(GridCoordinate coordinate, GridItem item, ChunkVisuals chunkVisuals)
    {
      var innerCoordinates = coordinate.InnerChunkGridCoordinate;

      var tileTemplate = _templates.Tiles.First(t => t.Name == item.Type);
      var template = tileTemplate.Template;

      // TODO don't create a new object each time.
      var newObject = template.Clone(coordinate.ToUpperRight(Vector2.zero));
      newObject.SetParent(gameObject);

      var existingValue = chunkVisuals.Tiles.Swap(innerCoordinates.X, innerCoordinates.Y, newObject);

      if (!existingValue.IsNull())
      {
        // TODO recycle the old tile
        Destroy(existingValue);
      }
    }

    private bool ContinueAdding(EnqueuedChunk chunkDataToProcess)
    {
      var chunk = chunkDataToProcess.TheChunk;

      int y = chunkDataToProcess.Y;

      for (int x = 0; x < Chunk.NumberOfGridItemsWide; x++)
      {
        var position = new GridCoordinate(chunk.Position, new InnerChunkGridCoordinate(x, y));
        var item = chunk[position.InnerChunkGridCoordinate];

        UpdateSprite(position, item, chunkDataToProcess.ChunkVisuals);
      }

      chunkDataToProcess.Y++;

      return chunkDataToProcess.Y >= Chunk.NumberOfGridItemsHigh;
    }

    private bool ContinueRemoving(EnqueuedChunk chunkDataToProcess)
    {
      var chunk = chunkDataToProcess.TheChunk;
      var visuals = chunkDataToProcess.ChunkVisuals;

      int y = chunkDataToProcess.Y;

      for (int x = 0; x < Chunk.NumberOfGridItemsWide; x++)
      {
        var position = new GridCoordinate(chunk.Position, new InnerChunkGridCoordinate(x, y));

        var tile = visuals.Tiles[position.InnerChunkGridCoordinate.X, position.InnerChunkGridCoordinate.Y];

        // TODO recycle
        if (tile != null)
        {
          Destroy(tile);
        }
      }

      chunkDataToProcess.Y++;

      return chunkDataToProcess.Y >= Chunk.NumberOfGridItemsHigh;
    }

    private class ChunkVisuals
    {
      // TODO pool this
      public readonly Array2D<GameObject> Tiles = new Array2D<GameObject>(Chunk.NumberOfGridItemsWide,
                                                                          Chunk.NumberOfGridItemsHigh);

      public bool IsRemoved;
    }

    private class EnqueuedChunk
    {
      public EnqueuedChunk(Chunk theChunk, bool isRemoving)
      {
        if (theChunk == null)
          throw new ArgumentNullException("theChunk");

        TheChunk = theChunk;
        ChunkVisuals = TheChunk.GetVisual<ChunkVisuals>();
        IsRemoving = isRemoving;
        Y = 0;
      }

      public readonly bool IsRemoving;
      public readonly Chunk TheChunk;
      public readonly ChunkVisuals ChunkVisuals;

      public int Y;
    }
  }
}