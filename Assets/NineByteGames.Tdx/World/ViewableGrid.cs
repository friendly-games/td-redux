using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineByteGames.Tdx.World
{
  /// <summary> Callback to be invoked when a grid item changes. </summary>
  public delegate void GridItemChangedCallback(GridCoordinate coordinate, GridItem oldValue, GridItem newValue);

  /// <summary> Callback to be invoked for each grid item. </summary>
  public delegate void GridItemCallback(GridCoordinate coordinate, GridItem item);

  public delegate void ViewableChunkChanged(Chunk oldChunk, Chunk newChunk);

  /// <summary>
  ///  A view into the WorldGrid that "watches" a specific section of the world
  ///  grid in order to allow callers to only be interested in the events that
  ///  occur to that section of the world.  Useful for the UI that is only
  ///  interested in changes that are currently visible to the user.
  /// </summary>
  public sealed class ViewableGrid
  {
    private readonly WorldGrid _world;

    private readonly Array2D<Chunk> _visibleChunks;
    private ChunkCoordinate? _centeredChunkCoordinate;

    /// <summary> Default constructor. </summary>
    public ViewableGrid(WorldGrid worldGrid)
    {
      _world = worldGrid;

      _visibleChunks = new Array2D<Chunk>(3, 3);
    }

    /// <summary> Gets the number of items in the Y direction. </summary>
    public int NumberOfGridItemsHigh
    {
      get { return Chunk.NumberOfGridItemsHigh; }
    }

    /// <summary> Gets the number of items in the X direction. </summary>
    public int NumberOfGridItemsWide
    {
      get { return Chunk.NumberOfGridItemsWide; }
    }

    public void Recenter(Vector2 position, bool shouldForce = false)
    {
      // TODO what about out of bounds?
      var gridCoordinate = new GridCoordinate(position);

      var centeredChunkCoordinate = gridCoordinate.ChunkCoordinate;

      if (!shouldForce && _centeredChunkCoordinate == centeredChunkCoordinate)
        return;

      for (int yOffset = -1; yOffset < 2; yOffset++)
      {
        for (int xOffset = -1; xOffset < 2; xOffset++)
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
      else
      {
        newChunk = _world[chunkCoordinate];
      }

      var viewableXOffset = PositiveRemainder(chunkCoordinate.X, 3);
      var viewableYOffset = PositiveRemainder(chunkCoordinate.Y, 3);
      var oldChunk = _visibleChunks[viewableXOffset, viewableYOffset];

      if (oldChunk == newChunk)
        return;

      _visibleChunks[viewableXOffset, viewableYOffset] = newChunk;

      OnViewableChunkChanged(oldChunk, newChunk);
    }

    private static int PositiveRemainder(int value, int divisor)
    {
      return ((value % divisor) + divisor) % divisor;
    }

    /// <summary>
    ///  Event that occurs when a grid item changes.
    /// </summary>
    public event GridItemChangedCallback GridItemChanged;

    private void OnGridItemChanged(GridCoordinate coordinate, GridItem oldValue, GridItem newValue)
    {
      var handler = GridItemChanged;
      if (handler != null)
        handler(coordinate, oldValue, newValue);
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

  /// <summary> An item that takes up one position in the world grid. </summary>
  public struct GridItem
  {
    public GridItem(TileType type)
    {
      Type = type;
      ViewData = null;
    }

    /// <summary> The type of tile that the grid item represents. </summary>
    public TileType Type;

    public object ViewData;
  }
}