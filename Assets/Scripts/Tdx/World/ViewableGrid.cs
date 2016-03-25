using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineByteGames.Tdx.World
{
  /// <summary> Callback to be invoked when a grid item changes. </summary>
  public delegate void GridItemChangedCallback(GridPosition position, GridItem oldValue, GridItem newValue);

  /// <summary> Callback to be invoked for each grid item. </summary>
  public delegate void GridItemCallback(GridPosition position, GridItem item);

  /// <summary> Represents a viewable portion of the world. </summary>
  public sealed class ViewableGrid
  {
    /// <summary> All of the items that exist in the grid. </summary>
    private readonly GridItem[,] _items;

    /// <summary> Default constructor. </summary>
    public ViewableGrid()
    {
      _items = new GridItem[10, 10];

      var dirt = new PathGridItem();
      var water = new WaterGridItem();

      for (int i = 0; i < _items.GetLength(0); i++)
      {
        for (int j = 0; j < _items.GetLength(1); j++)
        {
          if (i % 3 == j % 3)
          {
            _items[i, j] = water;
          }
          else
          {
            _items[i, j] = dirt;
          }
        }
      }
    }

    /// <summary> Gets the number of items in the Y direction. </summary>
    public int Height
    {
      get { return _items.GetLength(1); }
    }

    /// <summary> Gets the number of items in the X direction. </summary>
    public int Width
    {
      get { return _items.GetLength(0); }
    }

    /// <summary>
    ///  Indexer to get the GridItem at the specified coordinates.
    /// </summary>
    /// <param name="position"> The position at which the item should be set or gotten.. </param>
    /// <returns> The GridItem at the specified position. </returns>
    public GridItem this[GridPosition position]
    {
      get { return _items[position.X, position.Y]; }
      set
      {
        var existing = this[position];
        OnGridItemChanged(position, existing, value);
        _items[position.X, position.Y] = value;
      }
    }

    /// <summary>
    ///  Event that occurs when a grid item changes.
    /// </summary>
    public event GridItemChangedCallback GridItemChanged;

    private void OnGridItemChanged(GridPosition position, GridItem oldValue, GridItem newValue)
    {
      var handler = GridItemChanged;
      if (handler != null)
        handler(position, oldValue, newValue);
    }
  }

  /// <summary> An item that takes up one position in the world grid. </summary>
  public abstract class GridItem
  {
    protected GridItem(TileType type)
    {
      Type = type;
    }

    /// <summary> The type of tile that the grid item represents. </summary>
    public TileType Type { get; private set; }
  }

  /// <summary> An object that is able to be walked on by the user. </summary>
  public class PathGridItem : GridItem
  {
    public PathGridItem()
      : base(TileType.Path)
    {
    }
  }

  public class WaterGridItem : GridItem
  {
    public WaterGridItem()
      : base(TileType.Water)
    {
    }
  }

  /// <summary> Represents a position in the world grid. </summary>
  public struct GridPosition
  {
    public int X;
    public int Y;

    public GridPosition(int x, int y)
    {
      X = x;
      Y = y;
    }

    public Vector2 ToUpperRight(Vector2 offset)
    {
      return new Vector2(offset.x + X, offset.y + Y);
    }
  }
}