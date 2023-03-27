using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "CropDB", menuName = "DataManager/CropDatabase", order = 1)]
public class CropDB : ScriptableObject
{
    [SerializeField, BoxGroup("Crop Database Info"), ReorderableList]
    private List<CropData> _allCropData = new List<CropData>();
    public List<CropData> allCropData { get { return _allCropData; } private set { _allCropData = value; }}

    [SerializeField, BoxGroup("General Settings")]
    private GameObject _cropPrefabs;
    public GameObject cropPrefabs { get => _cropPrefabs; private set { _cropPrefabs = value; }}
    [SerializeField, BoxGroup("General Settings")]
    private GameObject _seedBagPrefabs;
    public GameObject seedBagPrefabs { get => _seedBagPrefabs; private set { _seedBagPrefabs = value; }}

    public CropData GetCropDataByID(string value_string)
    {
        for (int i = 0; i < allCropData.Count; i++)
        {
            if(allCropData[i].cropID == value_string)
                return allCropData[i];
        }

        Debug.LogError($"<b><color=blue>CropDatabase</color>: Can't fine crop data with id: <color=red>{value_string}</color>.</b>");

        return null;
    }
}
