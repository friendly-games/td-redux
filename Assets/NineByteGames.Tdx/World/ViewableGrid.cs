using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Debug = UnityEngine.Debug;

namespace NineByteGames.Tdx.World
{
  internal delegate void ViewableGridItemChangedCallback(Chunk chunk, GridCoordinate coordinate, GridItem item);

  /// <summary>
  ///  Takes a portion of the WorldGrid and watches a movable rectangle of units in the world for
  ///  changes.  Useful for watching the change around the player as he moves.
  /// </summary>
  public sealed class ViewableGrid
  {
    private readonly WorldGrid _worldGrid;
    private readonly int _numUnitsWideHalfThreshold;
    private readonly int _numUnitsHighHalfThreshold;

    private readonly Array2D<StoredGridData> _visibleGridItems;
    private GridCoordinate _currentPosition;
    private Array2DIndex _viewCurrentPosition;

    /// <summary> Constructor. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="worldGrid"> The world grid which should provide the grid data to be watched. </param>
    /// <param name="numUnitsWide"> How many units wide the area that should be watched. </param>
    /// <param name="numUnitsHigh"> How many units high the area that should be watched. </param>
    public ViewableGrid(WorldGrid worldGrid, int numUnitsWide, int numUnitsHigh)
    {
      if (worldGrid == null)
        throw new ArgumentNullException("worldGrid");

      // we always need to be odd isn't that a statement of life)
      if (numUnitsWide % 2 != 1)
      {
        numUnitsWide = numUnitsWide + 1;
      }

      if (numUnitsHigh % 2 != 1)
      {
        numUnitsHigh = numUnitsHigh + 1;
      }

      // let's make sure we always have valid values
      numUnitsWide = Math.Max(numUnitsWide, 5);
      numUnitsHigh = Math.Max(numUnitsHigh, 5);

      _worldGrid = worldGrid;
      _visibleGridItems = new Array2D<StoredGridData>(numUnitsWide, numUnitsHigh);

      // these are used for converting an array index back into a world coordinate
      _numUnitsWideHalfThreshold = (_visibleGridItems.Width - 1) / 2;
      _numUnitsHighHalfThreshold = (_visibleGridItems.Height - 1) / 2;
    }

    /// <summary>
    ///  Reads data from the world grid.  Not done in the constructor to allow callers to subscribe to
    ///  the events that will be fired.
    /// </summary>
    /// <param name="position"> The position. </param>
    public void Initialize(GridCoordinate position)
    {
      _currentPosition = position;
      _viewCurrentPosition = ConvertToGridItemIndex(position);

      InitializeUnits();
    }

    /// <summary> Initialize each grid unit. </summary>
    private void InitializeUnits()
    {
      for (var y = 0; y < _visibleGridItems.Height; y++)
      {
        for (var x = 0; x < _visibleGridItems.Width; x++)
        {
          var index = new Array2DIndex(x, y);
          var coordinate = ConvertToGridCoordinate(index);

          var chunkCoordinate = coordinate.ChunkCoordinate;
          if (_worldGrid.IsValid(chunkCoordinate))
          {
            var chunk = _worldGrid[chunkCoordinate];
            var data = chunk[coordinate.InnerChunkGridCoordinate];

            UpdateData(index, data, coordinate);
          }
        }
      }
    }

    /// <summary> Changes the position being observed. </summary>
    /// <param name="gridCoordinate"> The new center that should be observed for changes. </param>
    public void Recenter(GridCoordinate gridCoordinate)
    {
      var center = gridCoordinate;
      _currentPosition = center;
      _viewCurrentPosition = ConvertToGridItemIndex(_currentPosition);

      UpdateUnits();
    }

    private void UpdateUnits()
    {
      // TODO use diffing to make this more efficient

      for (var y = 0; y < _visibleGridItems.Height; y++)
      {
        for (var x = 0; x < _visibleGridItems.Width; x++)
        {
          var index = new Array2DIndex(x, y);
          var coordinate = ConvertToGridCoordinate(index);

          var chunkCoordinate = coordinate.ChunkCoordinate;
          if (_worldGrid.IsValid(chunkCoordinate))
          {
            var existingData = _visibleGridItems[index];

            if (existingData.Position != coordinate)
            {
              var chunk = _worldGrid[chunkCoordinate];
              var data = chunk[coordinate.InnerChunkGridCoordinate];
              UpdateData(index, data, coordinate);
            }
          }
        }
      }
    }

    /// <summary> Updates the data at the specified index inside the grid. </summary>
    /// <param name="index"> Zero-based index of the. </param>
    /// <param name="data"> The grid data to store. </param>
    /// <param name="position"> The coordinate of the original GridItem where the data was retrieved
    ///  from. </param>
    private void UpdateData(Array2DIndex index, GridItem data, GridCoordinate position)
    {
      var newData = new StoredGridData()
                    {
                      Data = data,
                      Position = position,
                    };

      var oldData = _visibleGridItems.Swap(index, newData);

      DataChanged.Invoke(oldData, newData);
    }

    public event Action<StoredGridData, StoredGridData> DataChanged;

    /// <summary>
    ///  Converts the given grid coordinate into a an index that can be used to get the data in
    ///  <see cref="_visibleGridItems"/>.
    /// </summary>
    private Array2DIndex ConvertToGridItemIndex(GridCoordinate coordinate)
    {
      var correctedX = MathUtils.PositiveRemainder(coordinate.X, _visibleGridItems.Width);
      var correctedY = MathUtils.PositiveRemainder(coordinate.Y, _visibleGridItems.Height);

      var arrayIndex = new Array2DIndex(correctedX, correctedY);

      return arrayIndex;
    }

    /// <summary>
    ///  Converts an array index into the expected grid coordinate for the given index.
    /// </summary>
    /// <param name="arrayIndex"> Zero-based index of the array. </param>
    /// <returns> The given data converted to a grid coordinate. </returns>
    private GridCoordinate ConvertToGridCoordinate(Array2DIndex arrayIndex)
    {
      var difViewX = _viewCurrentPosition.X - arrayIndex.X;
      var signX = difViewX >= 0 ? 1 : -1;

      // Normalize the offset from the "current" view position.
      // 
      // This explanation just talks about X, the same theory holds for Y. Imagine a number line
      // ranging from 0 to maxViewX).
      // 
      // If the current position is on the right (so it's position is maxViewX), then the very left
      // position (position 0) of the view really just represents one world unit to the right of the
      // current position.  If we simply subtracted these two positions, however, it would be
      // reported it as a difference of -ViewWidth units away We want to convert it to a difference
      // of (1).
      // 
      // We know we have to do convert values if it's more than half the size away (that's the only
      // time that wrapping occurs when putting the values into the view). The conversion is fairly
      // straight forward when given examples:
      //  - When we're `maxViewX` view units away, we're really only 1 world unit away.
      //  - When we're `maxViewX - 1` units away we're really only 2 world units away.
      //  - When we're `maxViewX - N` units away, we're really only N world units away.
      // 
      // The pattern to find N (the world units away) is to take `ViewWidth` and "subtract"
      // `(maxViewX - N)`. `maxViewX - N` happens to be diffViewX, so then we just have to "subtract"
      // it from `ViewWidth`. "subtract" is in quotes because actually, when the difference is
      // negative, we want to add `ViewWidth` (otherwise we end up in very negative territory). So we
      // add when `maxViewX - N` is negative, subtract when `maxViewX - N` is positive. 
      var offsetX = Math.Abs(difViewX) > _numUnitsWideHalfThreshold
        ? difViewX + -signX * _visibleGridItems.Width
        : difViewX;

      // all of the above but now for Y
      var difViewY = _viewCurrentPosition.Y - arrayIndex.Y;
      var signY = difViewY >= 0 ? 1 : -1;

      var offsetY = Math.Abs(difViewY) > _numUnitsHighHalfThreshold
        ? difViewY + -signY * _visibleGridItems.Height
        : difViewY;

      var gridPosition = _currentPosition.Offset(-offsetX, -offsetY);

      ValidateConversion(gridPosition, arrayIndex);

      return gridPosition;
    }

    /// <summary>
    ///  (Only available in DEBUG builds) Verifies that the conversion from an ArrayIndex to a
    ///  GridCoordinate was successful.
    /// </summary>
    [Conditional("DEBUG")]
    private void ValidateConversion(GridCoordinate gridPosition, Array2DIndex arrayIndex)
    {
      var worked = ConvertToGridItemIndex(gridPosition).X == arrayIndex.X
                   && ConvertToGridItemIndex(gridPosition).Y == arrayIndex.Y;

      if (worked)
        return;

      // to aid in debugging
      worked = ConvertToGridItemIndex(gridPosition).X == arrayIndex.X
               && ConvertToGridItemIndex(gridPosition).Y == arrayIndex.Y;

      var message = new StringBuilder();
      message.AppendFormat("Array Index ({0}) was converted into Grid Position ({1})", arrayIndex, gridPosition);
      message.AppendFormat("But we expected ({0},{1})",
                           ConvertToGridItemIndex(gridPosition).X,
                           ConvertToGridItemIndex(gridPosition).Y);

      Debug.Log(message);
    }

    // TODO better figure out what else needs to be done here
    public struct StoredGridData
    {
      public Chunk Chunk;
      public GridCoordinate Position;
      public GridItem Data;
    }
  }
}