using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject[] weaponSlot;
    public GameObject[] dashSlot;

    public GameObject[] slots;
    public GameObject[] itemCount;

    private bool[] isFull;
    private float[] numCollectItem;

    private static Inventory instance = null;

    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    public static Inventory GetInstance { get { return instance; } }

    private void Start()
    {
        isFull = new bool[slots.Length];
        numCollectItem = new float[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetActive(false);
            itemCount[i].SetActive(false);
        }

        for (int i = 0; i < weaponSlot.Length; i++)
        {
            weaponSlot[i].SetActive(false);
            dashSlot[i].SetActive(false);
        }
    }

    public void AddItemToInventory(Sprite item, Item.ItemType type)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (type == Item.ItemType.Passive)
            {
                if (slots[i].GetComponent<Image>().sprite == item)
                {
                    numCollectItem[i]++;

                    if (numCollectItem[i] >= 2)
                    {
                        itemCount[i].SetActive(true);
                        itemCount[i].GetComponent<TextMeshProUGUI>().text = numCollectItem[i].ToString();
                    }

                    break;
                }
                else if (isFull[i] == false) // fisrt time collect passive item
                {
                    slots[i].SetActive(true);
                    slots[i].GetComponent<Image>().sprite = item;

                    isFull[i] = true;
                    numCollectItem[i]++;

                    break;
                }
            }
            else if (type == Item.ItemType.Weapon)
            {
                if (item.name.Contains("Dash")) // add icon to the dash slot
                {
                    if (dashSlot[0].GetComponent<Image>().sprite == null)
                    {
                        dashSlot[0].SetActive(true);
                        dashSlot[0].GetComponent<Image>().sprite = item;
                    }
                    else if(dashSlot[0].GetComponent<Image>().sprite != item && 
                        dashSlot[1].GetComponent<Image>().sprite == null)
                    {
                        dashSlot[1].SetActive(true);
                        dashSlot[1].GetComponent<Image>().sprite = item;
                    }
                }
                else // add icon to the weapon slot
                {
                    if (weaponSlot[0].GetComponent<Image>().sprite == null)
                    {
                        weaponSlot[0].SetActive(true);
                        weaponSlot[0].GetComponent<Image>().sprite = item;
                    }
                    else if (dashSlot[0].GetComponent<Image>().sprite != item &&
                        weaponSlot[1].GetComponent<Image>().sprite == null)
                    {
                        weaponSlot[1].SetActive(true);
                        weaponSlot[1].GetComponent<Image>().sprite = item;
                    }
                }
            }
        }
    }
}
