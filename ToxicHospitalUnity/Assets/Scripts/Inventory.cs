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
        switch (item.classification)
        {
            case LoreType.AudioLog:
                AudioLore recording = (AudioLore)item;
                Recordings.Add(recording);
                break;
            case LoreType.Letter:
                VisualLore letter = (VisualLore)item;
                Letters.Add(letter);
                break;
            case LoreType.Report:
                VisualLore report = (VisualLore)item;
                Reports.Add(report);
                break;
            default:
                break;
        }
    }

}
