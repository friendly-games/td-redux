using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Common.Utils;
using UnityEngine;

namespace NineByteGames.Tdx.World
{
  /// <summary> Callback to be invoked when a grid item changes. </summary>
  public delegate void GridItemChangedCallback(
    Chunk chunk,
    GridCoordinate coordinate,
    GridItem oldValue,
    GridItem newValue);

  /// <summary> Callback to be invoked for each grid item. </summary>
  public delegate void GridItemCallback(Chunk chunk, GridCoordinate coordinate, GridItem item);

  public delegate void ViewableChunkChanged(Chunk oldChunk, Chunk newChunk);

  /// <summary>
  ///  A view into the WorldGrid that "watches" a specific section of the world
  ///  grid in order to allow callers to only be interested in the events that
  ///  occur to that section of the world.  Useful for the UI that is only
  ///  interested in changes that are currently visible to the user.
  /// </summary>
  public sealed class ViewableChunks
  {
    private readonly WorldGrid _world;

    public readonly Array2D<Chunk> VisibleChunks;
    private ChunkCoordinate? _centeredChunkCoordinate;

    private readonly int _xRadius;
    private readonly int _yRadius;

    /// <summary> Default constructor. </summary>
    public ViewableChunks(WorldGrid worldGrid, int xRadius, int yRadius)
    {
      _world = worldGrid;

      _xRadius = xRadius;
      _yRadius = yRadius;

      NumberOfChunksWide = _xRadius * 2 + 1;
      NumberOfChunksHigh = _yRadius * 2 + 1;

      VisibleChunks = new Array2D<Chunk>(NumberOfChunksWide, NumberOfChunksHigh);
    }

    /// <summary> Gets the number of items in the Y direction. </summary>
    public int NumberOfChunksHigh { get; private set; }

    /// <summary> Gets the number of items in the X direction. </summary>
    public int NumberOfChunksWide { get; private set; }

    public void Recenter(Vector2 position, bool shouldForce = false)
    {
      // TODO what about out of bounds?
      var gridCoordinate = new GridCoordinate(position);

      var centeredChunkCoordinate = gridCoordinate.ChunkCoordinate;

      if (!shouldForce && _centeredChunkCoordinate == centeredChunkCoordinate)
        return;

      for (int yOffset = -_yRadius; yOffset <= _yRadius; yOffset++)
      {
        for (int xOffset = -_xRadius; xOffset <= _xRadius; xOffset++)
        {
          var chunkCoordinate = centeredChunkCoordinate;
          chunkCoordinate.X += xOffset;
          chunkCoordinate.Y += yOffset;

          UpdateChunkAt(chunkCoordinate);
        }
      }

      _centeredChunkCoordinate = centeredChunkCoordinate;
    }

    /// <summary> Loads the chunk from the world for the given coordinate, if it needs to be updated. </summary>
    /// <param name="chunkCoordinate"> The coordinate of the chunk to load, if it is different. </param>
    private void UpdateChunkAt(ChunkCoordinate chunkCoordinate)
    {
      Chunk newChunk;

      if (chunkCoordinate.X < 0 || chunkCoordinate.Y < 0)
      {
        newChunk = null;
      }
      else if (chunkCoordinate.X >= WorldGrid.NumberOfChunksWide
               || chunkCoordinate.Y >= WorldGrid.NumberOfChunksHigh)
      {
        newChunk = null;
      }
      else
      {
        newChunk = _world[chunkCoordinate];
      }

      var viewableXOffset = MathUtils.PositiveRemainder(chunkCoordinate.X, NumberOfChunksWide);
      var viewableYOffset = MathUtils.PositiveRemainder(chunkCoordinate.Y, NumberOfChunksHigh);
      var oldChunk = VisibleChunks[viewableXOffset, viewableYOffset];

      // they're the same so we don't have to do anything
      if (oldChunk == newChunk)
        return;

      VisibleChunks[viewableXOffset, viewableYOffset] = newChunk;

      // we've detected a change, so unsubscribe + subscribe from all of the stuff
      if (oldChunk != null)
      {
        oldChunk.GridItemChanged -= OnGridItemChanged;
      }

      if (newChunk != null)
      {
        newChunk.GridItemChanged += OnGridItemChanged;
      }

      OnViewableChunkChanged(oldChunk, newChunk);
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

    /// <summary>
    ///  Invoked when one chunk is swapped out for another.
    /// </summary>
    public event ViewableChunkChanged ViewableChunkChanged;

    private void OnViewableChunkChanged(Chunk oldchunk, Chunk newchunk)
    {
      var handler = ViewableChunkChanged;
      if (handler != null)
        handler(oldchunk, newchunk);
    }
  }
}