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
    private ViewableChunks _viewableChunks;
    private TemplatesBehavior _templates;
    private WorldGrid _worldGrid;
    private Camera _itemToTrack;
    private readonly Queue<ChunkDataOperation> _chunksToProcess = new Queue<ChunkDataOperation>();

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
      _viewableChunks = new ViewableChunks(_worldGrid, 3, 3);
      _viewableChunks.GridItemChanged += HandleChunksItemChanged;
      _viewableChunks.ViewableChunkChanged += HandleVisibleChunkChanged;
      _viewableChunks.Recenter(Vector2.zero);
    }

    /// <unitymethod />
    public void Update()
    {
      var position = _itemToTrack.transform.position;
      _viewableChunks.Recenter(position);

      if (_chunksToProcess.Count > 0)
      {
        var operation = _chunksToProcess.Peek();

        // NOTE we're adding/removing two rows at once TODO tweak this. 
        bool didComplete;
        if (operation.ShouldRemove)
        {
          didComplete = operation.Data.RemoveSingleRow();
        }
        else
        {
          didComplete = operation.Data.AddSingleRow(this);
        }

        if (didComplete)
        {
          _chunksToProcess.Dequeue();
        }
      }
    }

    /// <summary> Callback to be invoked when a new chunk has come into view. </summary>
    /// <param name="oldchunk"> The oldchunk. </param>
    /// <param name="newChunk"> The new chunk. </param>
    private void HandleVisibleChunkChanged(Chunk oldchunk, Chunk newChunk)
    {
      // TODO handle a recently removed queue being re-added
      if (oldchunk != null)
      {
        var chunkData = oldchunk.GetVisual<ChunkData>();
        _chunksToProcess.Enqueue(new ChunkDataOperation(chunkData, shouldRemove: true));
      }

      if (newChunk != null)
      {
        // TODO check if we have a visual and if so, reset it to start adding items back
        var chunkData = new ChunkData(newChunk);
        newChunk.SetVisual(chunkData);

        _chunksToProcess.Enqueue(new ChunkDataOperation(chunkData, shouldRemove: false));
      }
    }

    /// <summary> Callback to invoke when a GridItem changes. </summary>
    private void HandleChunksItemChanged(Chunk chunk, GridCoordinate coordinate, GridItem oldvalue, GridItem newvalue)
    {
      UpdateSprite(coordinate, newvalue, chunk.GetVisual<ChunkData>());
    }

    /// <summary> Updates the sprite for the given item at the given position. </summary>
    private void UpdateSprite(GridCoordinate coordinate, GridItem item, ChunkData chunkVisuals)
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

    /// <summary> Represents an operation that needs to be performed on a chunk. </summary>
    private struct ChunkDataOperation
    {
      /// <summary> The chunk data that should be processed. </summary>
      public readonly ChunkData Data;

      /// <summary> True if the chunk should be removed, false otherwise. </summary>
      public readonly bool ShouldRemove;

      public ChunkDataOperation(ChunkData data, bool shouldRemove)
      {
        Data = data;
        ShouldRemove = shouldRemove;

        data.PrepareForOperation();
      }
    }

    /// <summary>
    ///  Contains the GameObjects that are in the chunk (representing the tiles)
    ///  and also contains the algorithms for updating the tile information.
    /// </summary>
    private class ChunkData
    {
      // TODO pool this
      public readonly Array2D<GameObject> Tiles = new Array2D<GameObject>(Chunk.NumberOfGridItemsWide,
                                                                          Chunk.NumberOfGridItemsHigh);

      // /state\
      private readonly Chunk _owner;
      private int _currentRow;
      // \state/

      /// <summary> Constructor. </summary>
      /// <exception cref="ArgumentNullException"> Thrown when one or more required
      ///  arguments are null. </exception>
      /// <param name="chunk"> The owner of the data. </param>
      public ChunkData(Chunk chunk)
      {
        if (chunk == null)
          throw new ArgumentNullException("chunk");

        _owner = chunk;
        _currentRow = 0;
      }

      /// <summary> Prepares the data to begin adding or removing rows. </summary>
      public void PrepareForOperation()
      {
        _currentRow = 0;
      }

      /// <summary> Adds a single row of the chunk to the row. </summary>
      /// <returns> True if it completed adding all rows in the chunk, false otherwise. </returns>
      public bool AddSingleRow(GridObserverBehavior gridObserverBehavior)
      {
        var chunk = _owner;

        int y = _currentRow;

        for (int x = 0; x < Chunk.NumberOfGridItemsWide; x++)
        {
          var position = new GridCoordinate(chunk.Position, new InnerChunkGridCoordinate(x, y));
          var item = chunk[position.InnerChunkGridCoordinate];

          gridObserverBehavior.UpdateSprite(position, item, this);
        }

        _currentRow++;

        return _currentRow >= Chunk.NumberOfGridItemsHigh;
      }

      /// <summary> Removes a single row of the chunk </summary>
      /// <returns> True if it completed removing all rows in the chunk, false otherwise. </returns>
      public bool RemoveSingleRow()
      {
        var chunk = _owner;

        int y = _currentRow;

        for (int x = 0; x < Chunk.NumberOfGridItemsWide; x++)
        {
          var position = new GridCoordinate(chunk.Position, new InnerChunkGridCoordinate(x, y));

          var tile = Tiles.Swap(position.InnerChunkGridCoordinate.X, position.InnerChunkGridCoordinate.Y, null);

          // TODO recycle
          if (tile != null)
          {
            Destroy(tile);
          }
        }

        _currentRow++;

        return _currentRow >= Chunk.NumberOfGridItemsHigh;
      }
    }
  }
}