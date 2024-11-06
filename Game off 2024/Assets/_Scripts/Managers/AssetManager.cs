using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorType
{
    Yellow = 1,
    Red = 2,
    Green = 3,
    Blue = 4,
}

public class AssetManager : MonoBehaviour
{
    [SerializeField] AssetData assetData;

    private static Dictionary<ColorType, Sprite> _blockSpriteMap = new();
    private static Dictionary<ColorType, Sprite> _blockNotiColor = new();
    private static Dictionary<ColorType, Color> _blockTypeToColor = new();

    
    public static Sprite GetBlockSprite(ColorType type)
    {
        return _blockSpriteMap[type];
    }

    public static Color GetBlockColor(ColorType type)
    {
        return _blockTypeToColor[type];
    }

    private void Awake()
    {
        foreach (var data in assetData.GetBlockSpriteData())
        {
            _blockSpriteMap.Add(data.blockType, data.blockSprite);
        }

        _blockTypeToColor.Add(ColorType.Yellow, Color.yellow);
        _blockTypeToColor.Add(ColorType.Red, Color.red);
        _blockTypeToColor.Add(ColorType.Green, Color.green);
        _blockTypeToColor.Add(ColorType.Blue, Color.blue);
    }
}
