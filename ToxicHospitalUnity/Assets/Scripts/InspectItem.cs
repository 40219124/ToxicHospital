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
    private VisualLoreCanvasManager LoreReader;

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
        LoreReader = VisualLoreCanvasManager.Instance;
        button.onClick.AddListener(Inspect);


        //passing relevant data anout the asset/inventory
        Debug.Log("Item: " + item.name);
        inventoryIcon.sprite = item.objectSprite;
        label.text = item.objectName;
        inventoryIndex = invIndex;
        type = item.classification;

    }

    void Inspect()
    {
        Debug.Log("Inspecting a " + type);

        if (type != LoreType.AudioLog)
        {
            VisualLore item;

            //fetch item object from inventory
            if (type == LoreType.Letter)
            {
                item = Inventory.Instance.Letters[inventoryIndex];
            }
            else
            {
                item = Inventory.Instance.Reports[inventoryIndex];
            }

            Debug.Log("Inspecting " + item);
            LoreReader.UpdateAndOpen(item);
        }
        else
        {
            AudioLore item = Inventory.Instance.Recordings[inventoryIndex];
            RecordingPlayer.Instance.Activate(item);
        }


    }
}
