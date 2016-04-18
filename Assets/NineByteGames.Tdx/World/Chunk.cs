using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NineByteGames.Tdx.World
{
  /// <summary>
  ///  Represents a square portion of the map containing the tiles.
  /// </summary>
  public class Chunk : CoreObject
  {
    /// <summary>
    ///  How many bits to shift a <see cref="GridCoordinate"/> to get the
    ///  <see cref="ChunkCoordinate"/>.
    /// </summary>
    public const int XGridCoordinateToChunkCoordinateBitShift = 6;

    /// <summary>
    ///  How many bits to shift a <see cref="GridCoordinate"/> to get the
    ///  <see cref="ChunkCoordinate"/>.
    /// </summary>
    public const int YGridCoordinateToChunkCoordinateBitShift = XGridCoordinateToChunkCoordinateBitShift;

    /// <summary> The number of GridItems wide in each chunk. </summary>
    public const int NumberOfGridItemsWide = 1 << XGridCoordinateToChunkCoordinateBitShift;

    /// <summary> The number of GridItems high in each chunk. </summary>
    public const int NumberOfGridItemsHigh = 1 << YGridCoordinateToChunkCoordinateBitShift;

    /// <summary>
    ///  The bits of a <see cref="GridCoordinate"/> that represent the
    ///  <see cref="InnerChunkGridCoordinate"/>
    /// </summary>
    public const int GridItemsXCoordinateBitmask = NumberOfGridItemsWide - 1;

    /// <summary>
    ///  The bits of a <see cref="GridCoordinate"/> that represent the
    ///  <see cref="InnerChunkGridCoordinate"/>
    /// </summary>
    public const int GridItemsYCoordinateBitmask = NumberOfGridItemsHigh - 1;

    /// <summary> All of the items that exist in the grid. </summary>
    private readonly GridItem[] _items;

    /// <summary> Constructor. </summary>
    /// <param name="position"> The position of the given chunk. </param>
    public Chunk(ChunkCoordinate position)
    {
      Position = position;
      _items = new GridItem[NumberOfGridItemsWide * NumberOfGridItemsHigh];

      // TODO do this elsewhere

      for (int y = 0; y < NumberOfGridItemsHigh; y++)
      {
        for (int x = 0; x < NumberOfGridItemsWide; x++)
        {
          var gridPosition = new GridCoordinate(position, new InnerChunkGridCoordinate(x, y));
          // TODO load this from somewhere else
          var value = Mathf.PerlinNoise(gridPosition.X / 10f, gridPosition.Y / 10f);

          byte variant = (byte)(Random.value * 4);

          if (value > 0.7f)
          {
            this[new InnerChunkGridCoordinate(x, y)] = new GridItem(TileType.Water, variant);
          }
          else if (value < 0.3f)
          {
            this[new InnerChunkGridCoordinate(x, y)] = new GridItem(TileType.Hill, variant);
          }
          else
          {
            this[new InnerChunkGridCoordinate(x, y)] = new GridItem(TileType.Path, variant);
          }
        }
      }
    }

    /// <summary> The position of the given chunk. </summary>
    public ChunkCoordinate Position { get; set; }

    /// <summary>
    ///  Indexer to get the GridItem at the specified coordinates.
    /// </summary>
    /// <param name="coordinate"> The position at which the item should be set or gotten.. </param>
    /// <returns> The GridItem at the specified position. </returns>
    public GridItem this[InnerChunkGridCoordinate coordinate]
    {
      get { return _items[CalculateIndex(coordinate.X, coordinate.Y)]; }
      set
      {
        var existing = this[coordinate];
        OnGridItemChanged(this, new GridCoordinate(Position, coordinate), existing, value);
        _items[CalculateIndex(coordinate.X, coordinate.Y)] = value;
      }
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

    private int CalculateIndex(int x, int y)
    {
      return x + y * NumberOfGridItemsWide;
    }
  }
}