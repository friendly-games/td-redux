using System;
using System.Collections.Generic;
using System.Linq;

namespace NineByteGames.Tdx.World
{
  /// <summary>
  ///  Contains all of the chunks that make up the currently loaded world.
  /// </summary>
  public class WorldGrid
  {
    // TODO make this bigger (bigger makes it slower to start)
    public const int NumberOfChunksWide = 4;
    public const int NumberOfChunksHigh = 4;

    private readonly Chunk[] _chunks;

    public WorldGrid()
    {
      _chunks = new Chunk[NumberOfChunksHigh * NumberOfChunksWide];

      for (int y = 0; y < NumberOfChunksHigh; y++)
      {
        for (int x = 0; x < NumberOfChunksWide; x++)
        {
          _chunks[CalculateIndex(x, y)] = new Chunk(new ChunkCoordinate(x, y));
        }
      }
    }

    /// <summary>
    ///  Gets the chunk at the specified coordinate.
    /// </summary>
    public Chunk this[ChunkCoordinate coordinate]
    {
      get { return _chunks[CalculateIndex(coordinate.X, coordinate.Y)]; }
      set { _chunks[CalculateIndex(coordinate.X, coordinate.Y)] = value; }
    }

    private int CalculateIndex(int x, int y)
    {
      return x + y * NumberOfChunksWide;
    }
  }
}