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

    public void Initialise(LoreItem item, int invIndex)
    {
        //stuff that would normally go in start
        Debug.Log("Init");
        inventoryIcon = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();
        label = GetComponentInChildren<TextMeshProUGUI>();
        layoutIndex = gameObject.transform.GetSiblingIndex();

        //passing relevant data anout the asset/inventory
        Debug.Log("Item: " + item.name);
        inventoryIcon.sprite = item.objectSprite;
        label.text = item.objectName;
        inventoryIndex = invIndex;
    }

    void Inspect()
    {
        //if audio show transcript and play sound

    }
}
