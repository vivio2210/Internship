using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Luminosity.IO;

public class PlantableTile : MonoBehaviour
{
    private float _dryTime = 5.0f;
    private float _growBuff = 1f;
    private ItemData _itemData;
    private PlantableTile _plantableTile;
    private Coroutine _activateFertilizer;
    private Coroutine _activateCloud;
    public enum PlantableTileState
    {
        empty,
        planted,
        harvestable
    }

    private PlantableTileState tileState;

    [SerializeField, BoxGroup("General Setting")]
    private SpriteRenderer cropRenderer;
    [SerializeField, BoxGroup("General Setting")]
    private SpriteRenderer seedRenderer;
    //Temporary brown tinted tile RGB value
    private Glowing glow;
    //end of temporary RGB values
    [SerializeField, BoxGroup("PlantableTile Info"), ReadOnly]
    private CropData cropData;
    [SerializeField, BoxGroup("PlantableTile Info"), ReadOnly]
    private bool isDigged;
    [SerializeField, BoxGroup("PlantableTile Info"), ReadOnly]
    private float _growthLevel = 0f;
    public float growthLevel { get => _growthLevel; private set { _growthLevel = Mathf.Max(Mathf.Min(value, 1), 0); }}
    [SerializeField, BoxGroup("PlantableTile Info"), ReadOnly]
    private float _humidityLevel;
    public float humidityLevel { get => _humidityLevel; private set { _humidityLevel = Mathf.Max(value, 0); }}

    private Coroutine _humidityTimer;
    private Coroutine _psTimer;
    private OutliningTile _outliningTile;
    private waterTile _waterTile;
    private KarmaStats _karma;

    public delegate void NoArgs();
    public delegate void IDArgs(PlayerID playerID);
    public static NoArgs onFullyGrow;
    public static IDArgs onHarvested;
    private ParticleSystem _ps;

    private void Awake() 
    {
        _ps = GetComponentInChildren<ParticleSystem>();
        _karma = GetComponent<KarmaStats>();
        _humidityLevel = 0f;
        _outliningTile = GetComponentInChildren<OutliningTile>();
        _waterTile = GetComponentInChildren<waterTile>();
        glow = this.transform.GetChild(0).GetComponent<Glowing>();
    }

    private void Start() 
    {
        _ps.Stop();
        UpdateCropRenderer();
    }

    private void ChangeGrowBuff(float value)
    {
        _growBuff = value;
    }

    public PlantableTileState GetTileState(){
        return tileState;
    }

    public bool AttemptPlantCrop(CropData value_crop, PlayerID playerID)
    {
        if(tileState == PlantableTileState.empty)
        {
            cropData = value_crop;
            tileState = PlantableTileState.planted;
            seedRenderer.gameObject.SetActive(true);
            //random seed image 
            if (Random.Range(0f, 1f) >= 0.5)
            {
                seedRenderer.sprite = cropData.cropSeedImage1;
            }
            else
            { 
                seedRenderer.sprite = cropData.cropSeedImage2;
            }

            UpdateCropRenderer();

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AttemptHarvestCrop(PlayerID playerID)
    {
        if(tileState == PlantableTileState.harvestable)
        {
            glow.GlowOff();
            CropController cropController = Instantiate(GM.instance.cropDB.cropPrefabs, transform.position + Vector3.up, Quaternion.identity).GetComponent<CropController>();
            cropController.SetupCrop(cropData.cropID);
            
            _karma.SetPlayerHarvest(playerID);
            cropController.transform.GetComponent<KarmaStats>().CopyKarmaStats(this.gameObject.GetComponent<KarmaStats>());
            ResetTile();
            if(onHarvested != null)
                onHarvested.Invoke(playerID);

            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetTile()
    {
        cropData = null;
        growthLevel = 0;
        tileState = PlantableTileState.empty;
        glow.GlowOff();

        _karma.isWaterOnce = false;
        _karma.isWaterTwice = false;

        UpdateCropRenderer();
    }

    [Button]
    public void WaterTile()
    {        
        if(_humidityTimer != null) StopCoroutine(_humidityTimer);

        _humidityTimer = StartCoroutine(Evaporate(_dryTime));

    }

    [Button]
    public void UpdateCropRenderer()
    {
        if(tileState == PlantableTileState.empty || cropData == null)
        {
            cropRenderer.sprite = null;
            seedRenderer.sprite = null;
        }
        else if(tileState == PlantableTileState.planted)
        {
            if(growthLevel > 0.75f)
            {
                seedRenderer.gameObject.SetActive(false);
                cropRenderer.gameObject.SetActive(true);
                cropRenderer.sprite = cropData.cropSproutThreeImage;
            }
            else if(growthLevel > 0.5f)
            {   
                seedRenderer.gameObject.SetActive(false);
                cropRenderer.gameObject.SetActive(true);
                cropRenderer.sprite = cropData.cropSproutTwoImage;
            }
            else if(growthLevel > 0.25f)
            {  
                seedRenderer.gameObject.SetActive(false);
                cropRenderer.gameObject.SetActive(true);
                cropRenderer.sprite = cropData.cropSproutOneImage;
            }
            else
            {
                cropRenderer.gameObject.SetActive(false);
                seedRenderer.gameObject.SetActive(true);
            }
        }
        else if(tileState == PlantableTileState.harvestable)
        {
            seedRenderer.gameObject.SetActive(false);
            cropRenderer.gameObject.SetActive(true);
            cropRenderer.sprite = cropData.cropHarvestableImage;
        }
    }

    public void FocusingOnTile(bool value_bool)
    {
        _outliningTile.UpdateOutline(value_bool);
    }

    private void AttemptGrowCrop()
    {   
        if(tileState == PlantableTileState.planted)
        {
            growthLevel = ((growthLevel * cropData.growDuration) + (Time.deltaTime*_growBuff)) / cropData.growDuration;

            if(growthLevel == 1f)
            {
                tileState = PlantableTileState.harvestable;
                glow.GlowOn();

                if(onFullyGrow != null)
                    onFullyGrow.Invoke();
            }

            UpdateCropRenderer();
        }
    } 

    public void FertilizeCrop(string itemID)
    {
        if(tileState == PlantableTileState.planted)
        {
            growthLevel = 1f;
            //seedRenderer.enabled = false;
            //cropRenderer.gameObject.SetActive(true);
            //UpdateCropRenderer();
            AttemptGrowCrop();
            
            if(_psTimer != null) StopCoroutine(_psTimer);
            _psTimer = StartCoroutine(GrowParticle(0.5f));
        }
    }

    public void ShadeCrop(string itemID)
    {
        if(tileState == PlantableTileState.planted)
        {
            if(_activateCloud != null) StopCoroutine(_activateCloud);
            _activateCloud = StartCoroutine(ActivateCloud(GM.instance.itemDB.GetItemDataByID(itemID).itemDuration));

        }
    }
    
    IEnumerator Evaporate(float duration)
    {
        humidityLevel = duration;
        while(humidityLevel > 0)
        {
            humidityLevel -= Time.deltaTime;
            _waterTile.UpdateWaterStages(humidityLevel/duration);
            AttemptGrowCrop();
            yield return null;
        }
    }

    IEnumerator ActivateFertilizer(float duration){
        
        Debug.Log("Growing faster");
        _growBuff += 0.5f;
        while(duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        _growBuff -= 0.5f;
        Debug.Log("Fertilizer Deactivated.");
    }

    IEnumerator ActivateCloud(float duration){
        Debug.Log("Growing Slower");
        _growBuff = 0.5f;
        while(duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        _growBuff += 0.5f;
        Debug.Log("Cloud Deactivated.");
    }    

    IEnumerator GrowParticle(float duration)
    {
        _ps.Play();
        while(duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        _ps.Stop();
    }
}


