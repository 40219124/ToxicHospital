using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory
{
    public List<AudioLore> Recordings = new List<AudioLore>();
    public List<VisualLore> Letters = new List<VisualLore>();
    public List<VisualLore> Reports = new List<VisualLore>();



    //singletonifying the class
    //----------------------------
    private static Inventory instance;
    private Inventory() { }

    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Inventory();
            }
            return instance;
        }
    }
    //----------------------------


    public void AddItem(LoreItem item)
    {
        Debug.LogWarning("Remove this testing code when finished.");
        int index = -1;

        if (item.classification == LoreType.AudioLog)
        {
            AudioLore recording = (AudioLore)item;
            Recordings.Add(recording);
            index = Recordings.Count - 1;
        }
        else if (item.classification == LoreType.Letter)
        {
            VisualLore letter = (VisualLore)item;
            Letters.Add(letter);
            index = Letters.Count - 1;
        }
        else if (item.classification == LoreType.Report)
        {
            VisualLore report = (VisualLore)item;
            Reports.Add(report);
            index = Reports.Count - 1;
        }
        else
        {
            Debug.LogError("Invalid enum value for item.classification/LoreType (" + item.classification + ") on object : " + item.name + ", " + item.objectName);
        }
        AddItemToUI(item, index);
    }



    private void AddItemToUI(LoreItem item, int inventoryIndex)
    {
        //get the object containg the inventory layouts
        GameObject layoutContainer = GameObject.FindGameObjectWithTag("InvLayouts");

        //get the child layouts as transforms
        List<Transform> layouts = new List<Transform>();
        foreach (Transform t in layoutContainer.transform)
        {
            layouts.Add(t);
        }

        Transform layout = layouts[(int)item.classification];

        var prefab = Resources.Load<GameObject>("Prefabs/InventoryItemUI");
        GameObject cell = GameObject.Instantiate(prefab);
        cell.gameObject.transform.SetParent(layout.transform);

        cell.gameObject.GetComponent<InspectItem>().Initialise(item, inventoryIndex);

    }

}
