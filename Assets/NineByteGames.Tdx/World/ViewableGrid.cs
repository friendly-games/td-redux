using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineByteGames.Tdx.World
{
  /// <summary>
  ///  A view into the WorldGrid that "watches" a specific section of the world
  ///  grid in order to allow callers to only be interested in the events that
  ///  occur to that section of the world.  Useful for the UI that is only
  ///  interested in changes that are currently visible to the user.
  /// </summary>
  public sealed class ViewableGrid
  {
    private readonly int _gridItemsWide;
    private readonly int _gridItemsHigh;
    private readonly Array2D<DataStruct> _visibleGridItems;
    private readonly ViewableChunks _viewableChunks;

    private readonly int _gridItemsHighHalf;
    private readonly int _gridItemsWideHalf;

    private GridCoordinate _currentPosition;
    private GridCoordinate _lastPosition;

    public ViewableGrid(WorldGrid worldGrid, int gridItemsWide, int gridItemsHigh)
    {
      _gridItemsWide = gridItemsWide;
      _gridItemsHigh = gridItemsHigh;

      _gridItemsWideHalf = gridItemsWide / 2;
      _gridItemsHighHalf = gridItemsHigh / 2;

      var numChunksWide = (int)Mathf.Ceil(Mathf.Ceil((float)gridItemsWide / 2.0f) / (float)Chunk.NumberOfGridItemsWide);
      var numChunksHigh = (int)Mathf.Ceil(Mathf.Ceil((float)gridItemsHigh / 2.0f) / (float)Chunk.NumberOfGridItemsHigh);

      _visibleGridItems = new Array2D<DataStruct>(gridItemsWide, gridItemsHigh);

      _viewableChunks = new ViewableChunks(worldGrid, numChunksWide, numChunksHigh);
      _viewableChunks.GridItemChanged += HandleChunksItemChanged;
      _viewableChunks.ViewableChunkChanged += HandleVisibleChunkChanged;
    }

    /// <summary>
    ///  Event that occurs when a grid item changes.
    /// </summary>
    public event GridItemChangedCallback GridItemChanged;

    private void OnGridItemChanged(Chunk chunk, GridCoordinate coordinate, GridItem oldValue, GridItem newValue)
    {
      var handler = GridItemChanged;
      if (handler != null)
        handler(chunk, coordinate, oldValue, newValue);
    }

    private void HandleChunksItemChanged(Chunk chunk, GridCoordinate coordinate, GridItem oldvalue, GridItem newvalue)
    {
      if (IsWithinViewableArea(coordinate))
      {
        OnGridItemChanged(chunk, coordinate, oldvalue, newvalue);
      }
    }

    private bool IsWithinViewableArea(GridCoordinate coordinate)
    {
      int xDistance = coordinate.X - _currentPosition.X;
      int yDistance = coordinate.Y - _currentPosition.Y;

      return -_gridItemsWideHalf < xDistance && xDistance < _gridItemsWideHalf
             && -_gridItemsHighHalf < yDistance && yDistance < _gridItemsHighHalf;
    }

    private void HandleVisibleChunkChanged(Chunk oldChunk, Chunk newChunk)
    {
      if (oldChunk != null)
      {
        // TODO remove the items that are now gone
        for (int y = 0; y < Chunk.NumberOfGridItemsHigh; y++)
        {
          for (int x = 0; x < Chunk.NumberOfGridItemsWide; x++)
          {
            var gridCoordinate = new GridCoordinate(oldChunk.Position, new InnerChunkGridCoordinate(x, y));
            if (IsWithinViewableArea(gridCoordinate))
            {
              RemoveItem(oldChunk, gridCoordinate);
            }
          }
        }
      }

      if (newChunk != null)
      {
        // TODO optimize this only for the new items
        for (int y = 0; y < Chunk.NumberOfGridItemsHigh; y++)
        {
          for (int x = 0; x < Chunk.NumberOfGridItemsWide; x++)
          {
            var gridCoordinate = new GridCoordinate(newChunk.Position, new InnerChunkGridCoordinate(x, y));
            if (IsWithinViewableArea(gridCoordinate))
            {
              PutItem(newChunk, gridCoordinate);
            }
          }
        }
      }
    }

    private void RemoveItem(Chunk oldChunk, GridCoordinate gridCoordinate)
    {
      int correctedX = MathUtils.PositiveRemainder(gridCoordinate.X, _gridItemsWide);
      int correctedY = MathUtils.PositiveRemainder(gridCoordinate.Y, _gridItemsHigh);

      var existingDataItem = _visibleGridItems.Swap(correctedX, correctedY, new DataStruct());

      if (existingDataItem.ChunkOwner != null)
      {
        OnGridItemRemoved(existingDataItem.ChunkOwner,
                          new GridCoordinate(existingDataItem.ChunkOwner.Position, existingDataItem.InnerCoordinate),
                          oldChunk[gridCoordinate.InnerChunkGridCoordinate]);
      }
    }

    private void PutItem(Chunk chunk, GridCoordinate gridCoordinate)
    {
      int correctedX = MathUtils.PositiveRemainder(gridCoordinate.X, _gridItemsWide);
      int correctedY = MathUtils.PositiveRemainder(gridCoordinate.Y, _gridItemsHigh);

      var newDataItem = new DataStruct()
                        {
                          ChunkOwner = chunk,
                          InnerCoordinate = gridCoordinate.InnerChunkGridCoordinate
                        };

      var existingDataItem = _visibleGridItems.Swap(correctedX, correctedY, newDataItem);

      if (existingDataItem.ChunkOwner == newDataItem.ChunkOwner
          && existingDataItem.InnerCoordinate == newDataItem.InnerCoordinate)
      {
        return;
      }

      // if the previous item was currently populated, let the listener know that the data is going away
      if (existingDataItem.ChunkOwner != null)
      {
        var existingGridItem = existingDataItem.ChunkOwner[existingDataItem.InnerCoordinate];
        OnGridItemRemoved(existingDataItem.ChunkOwner,
                          new GridCoordinate(existingDataItem.ChunkOwner.Position, existingDataItem.InnerCoordinate),
                          existingGridItem);
      }

      // and then let them know about the data that replaced it
      var item = chunk[gridCoordinate.InnerChunkGridCoordinate];
      OnGridItemAdded(chunk, gridCoordinate, item);
    }

    public event GridItemCallback GridItemAdded;

    private void OnGridItemAdded(Chunk chunk, GridCoordinate coordinate, GridItem item)
    {
      var handler = GridItemAdded;
      if (handler != null)
        handler(chunk, coordinate, item);
    }

    public event GridItemCallback GridItemRemoved;

    private void OnGridItemRemoved(Chunk chunk, GridCoordinate coordinate, GridItem item)
    {
      var handler = GridItemRemoved;
      if (handler != null)
        handler(chunk, coordinate, item);
    }

    public void Recenter(Vector2 position, bool shouldForce = false)
    {
      _lastPosition = _currentPosition;
      _currentPosition = new GridCoordinate(position);

      // this is when chunk-visible callbacks will be invoked.
      _viewableChunks.Recenter(position);

      foreach (var chunk in _viewableChunks.VisibleChunks.Data)
      {
        if (chunk == null)
          continue;

        for (int y = 0; y < Chunk.NumberOfGridItemsHigh; y++)
        {
          for (int x = 0; x < Chunk.NumberOfGridItemsWide; x++)
          {
            var gridCoordinate = new GridCoordinate(chunk.Position, new InnerChunkGridCoordinate(x, y));
            if (IsWithinViewableArea(gridCoordinate))
            {
              PutItem(chunk, gridCoordinate);
            }
          }
        }
      }
    }

    private struct DataStruct
    {
      public Chunk ChunkOwner;
      public InnerChunkGridCoordinate InnerCoordinate;
    }
  }
}