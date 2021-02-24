using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InspectItem : MonoBehaviour
{
    private Image inventoryIcon;
    private Button button;
    private TextMeshProUGUI label;
    private int layoutIndex;

    //must be able to access the actual inventory in the event it needs to later
    LoreType type;
    int inventoryIndex;
    void Start()
    {
        Debug.Log("Start");

    }

    public void Initialise(LoreItem item, int invIndex)
    {
        Debug.Log("Init");
        inventoryIcon = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();
        label = GetComponentInChildren<TextMeshProUGUI>();
        layoutIndex = gameObject.transform.GetSiblingIndex();


        Debug.Log("Item: " + item.name);
        inventoryIcon.sprite = item.objectSprite;
        label.text = item.objectName;
        inventoryIndex = invIndex;
    }

    void Inspect()
    {
        //TODO, make it show text or play audio
    }
}
