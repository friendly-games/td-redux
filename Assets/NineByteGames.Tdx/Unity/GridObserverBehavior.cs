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

    public void Start()
    {
      _templates = GetComponent<TemplatesBehavior>();

      // TODO get the grid from elsewhere
      Initialize(new ViewableGrid());
    }

    /// <summary> Sets up the Observer to watch the given grid. </summary>
    public void Initialize(ViewableGrid grid)
    {
      _viewableGrid = grid;
      _viewableGrid.GridItemChanged += HandleGridItemChanged;

      for (int x = 0; x < _viewableGrid.Width; x++)
      {
        for (int y = 0; y < _viewableGrid.Height; y++)
        {
          var position = new GridPosition(x, y);
          var item = _viewableGrid[position];

          UpdateSprite(position, item);
        }
      }
    }

    /// <summary> Callback to invoke when a GridItem changes. </summary>
    private void HandleGridItemChanged(GridPosition position, GridItem oldvalue, GridItem newvalue)
    {
      UpdateSprite(position, newvalue);
    }

    /// <summary> Updates the sprite for the given item at the given position. </summary>
    private void UpdateSprite(GridPosition position, GridItem item)
    {
      var tileTemplate = _templates.Tiles.First(t => t.Name == item.Type);
      var template = tileTemplate.Template;

      // TODO don't create a new object each time.
      // TODO remove old items
      var newObject = template.Clone(position.ToUpperRight(Vector2.zero));
      newObject.SetParent(gameObject);
    }
  }
}