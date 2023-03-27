using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "CropData", menuName = "ScriptableObject/CropData", order = 1)]
public class CropData : ScriptableObject
{
    [SerializeField, BoxGroup("Crop Data Info"), ShowAssetPreview]
    private Sprite _cropIcon;
    public Sprite cropIcon { get { return _cropIcon; } private set { _cropIcon = value; }}
    [SerializeField, BoxGroup("Crop Data Info"), ShowAssetPreview]
    private Sprite _seedBagImage;
    public Sprite seedBagImage { get { return _seedBagImage; } private set { _seedBagImage = value; }}
    [HorizontalLine(color: EColor.Green)]
    [SerializeField, BoxGroup("Crop Data Info"), ShowAssetPreview]
    private Sprite _cropSeedImage1;
    public Sprite cropSeedImage1 { get { return _cropSeedImage1; } private set { _cropSeedImage1 = value; }}
    [SerializeField, BoxGroup("Crop Data Info"), ShowAssetPreview]
    private Sprite _cropSeedImage2;
    public Sprite cropSeedImage2 { get { return _cropSeedImage2; } private set { _cropSeedImage2 = value; }}
    [SerializeField, BoxGroup("Crop Data Info"), ShowAssetPreview]
    private Sprite _cropSproutOneImage;
    public Sprite cropSproutOneImage { get { return _cropSproutOneImage; } private set { _cropSproutOneImage = value; }}
    [SerializeField, BoxGroup("Crop Data Info"), ShowAssetPreview]
    private Sprite _cropSproutTwoImage;
    public Sprite cropSproutTwoImage { get { return _cropSproutTwoImage; } private set { _cropSproutTwoImage = value; }}
    [SerializeField, BoxGroup("Crop Data Info"), ShowAssetPreview]
    private Sprite _cropSproutThreeImage;
    public Sprite cropSproutThreeImage { get { return _cropSproutThreeImage; } private set { _cropSproutThreeImage = value; }}
    [SerializeField, BoxGroup("Crop Data Info"), ShowAssetPreview]
    private Sprite _cropHavestableImage;
    public Sprite cropHarvestableImage { get { return _cropHavestableImage; } private set { _cropHavestableImage = value; }}
    [HorizontalLine(color: EColor.Green)]
    [SerializeField, BoxGroup("Crop Data Info")]
    private string _cropID = "";
    public string cropID { get { return _cropID; } private set { _cropID = value; }}
    [SerializeField, BoxGroup("Crop Data Info")]
    private string _cropName = "";
    public string cropName { get { return _cropName; } private set { _cropName = value; }}
    [SerializeField, BoxGroup("Crop Data Info")]
    private int _baseHP = 0;
    public int baseHP { get { return _baseHP; } private set { _baseHP = value; }}
    [SerializeField, BoxGroup("Crop Data Info")]
    private int _growDuration = 0;
    public int growDuration { get { return _growDuration; } private set { _growDuration = value; }}
    [SerializeField, BoxGroup("Crop Data Info")]
    private int _baseBuyPrice = 0; // This value shouldn't get change by any script except by the balancing script.
    public int baseBuyPrice { get { return _baseBuyPrice; } private set { _baseBuyPrice = value; }}
    [SerializeField, BoxGroup("Crop Data Info")]
    private int _currentBuyPrice = 0;
    public int currentBuyPrice { get { return _currentBuyPrice; } private set { _currentBuyPrice = value; }}
    [SerializeField, BoxGroup("Crop Data Info")]
    private int _baseSellPrice = 0; // This value shouldn't get change by any script except by the balancing script.
    public int baseSellPrice { get { return _baseSellPrice; } private set { _baseSellPrice = value; }}
    [SerializeField, BoxGroup("Crop Data Info")]
    private int _currentSellPrice = 0;
    public int currentSellPrice { get { return _currentSellPrice; } private set { _currentSellPrice = value; }}

    public void SetCropID(string value_string) { cropID = value_string; }
    public void SetCropName(string value_string) { cropName = value_string; }
    public void SetBaseHP(int value_int) { baseHP = value_int; }
    public void SetGrowDuration(int value_int) { growDuration = value_int; }
    public void SetBaseBuyPrice(int value_int) { baseBuyPrice = value_int; }
    public void SetBaseSellPrice(int value_int) { baseSellPrice = value_int; }
    public void SetCurrentBuyPrice(int value_int) { currentBuyPrice = value_int; }
    public void SetCurrentSellPrice(int value_int) { currentSellPrice = value_int; }
}
