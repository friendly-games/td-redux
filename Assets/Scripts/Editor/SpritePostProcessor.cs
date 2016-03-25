using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//gist.github.com/Pikl/3c44aefa54c931555a12

public class SpritePostProcessor : AssetPostprocessor
{
  public int pixelsPerUnit = 32;
  public bool mipMapEnabled = false;
  public FilterMode filterMode = FilterMode.Bilinear;
  public TextureImporterFormat textureFormat = TextureImporterFormat.AutomaticTruecolor;

  public void OnPostprocessTexture(Texture2D texture)
  {
    TextureImporter ti = (assetImporter as TextureImporter);
    ti.spritePixelsPerUnit = pixelsPerUnit;
    //ti.filterMode = filterMode;

    //ti.mipmapEnabled = mipMapEnabled;
    //ti.textureFormat = textureFormat;
    //ti.alphaIsTransparency = true;

    TextureImporterSettings importerSettings = new TextureImporterSettings();
    ti.ReadTextureSettings(importerSettings);

    float size = Mathf.Max(texture.width, texture.height);

    int power = 1;
    while (power < size)
    {
      power *= 2;
    }

    power = Mathf.Clamp(power, 32, 8192);

    importerSettings.maxTextureSize = power;
    ti.SetTextureSettings(importerSettings);
  }
}