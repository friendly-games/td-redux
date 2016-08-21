using System;
using System.Collections.Generic;
using System.Linq;
using NineByteGames.Common;
using UnityEngine;

namespace NineByteGames.Tdx.Unity.View
{
  /// <summary> Unity implementation of IHealth. </summary>
  public class HealthBarBehavior : MonoBehaviour,
                                   IHealthView
  {
    private RectTransform _backgroundRect;
    private RectTransform _healthRect;

    public GameObject BackgroundView;
    public GameObject HealthView;
    private RectTransform _parentRect;

    public void Start()
    {
      _backgroundRect = BackgroundView.GetComponent<RectTransform>();
      _healthRect = HealthView.GetComponent<RectTransform>();
      _parentRect = HealthView.GetParent().GetComponent<RectTransform>();

      SetHealth(.20f);
    }

    public void SetHealth(float health)
    {
      float parentWidth = _parentRect.rect.width;

      float relativeWidthOfHealth = health * parentWidth;
      float relativeNonHealthWidth = parentWidth - relativeWidthOfHealth;

      _backgroundRect.sizeDelta = new Vector2(relativeNonHealthWidth, _backgroundRect.sizeDelta.y);
      _healthRect.sizeDelta = new Vector2(relativeWidthOfHealth, _healthRect.sizeDelta.y);
    }
  }
}