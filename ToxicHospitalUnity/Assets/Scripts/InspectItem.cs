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

    private List<Button> tabs = new List<Button>();

    //must be able to access the actual inventory in the event it needs to later
    LoreType type;
    int inventoryIndex;

    public void Initialise(LoreItem item, int invIndex)
    {
        //stuff that would normally go in start
        inventoryIcon = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();
        label = GetComponentInChildren<TextMeshProUGUI>();
        layoutIndex = gameObject.transform.GetSiblingIndex();
        LoreReader = VisualLoreCanvasManager.Instance;
        button.onClick.AddListener(Inspect);
        GameObject tabsContainer = GameObject.FindGameObjectWithTag("InvTabs");
        foreach (Transform t in tabsContainer.transform)
        {
            tabs.Add(t.GetComponent<Button>());
        }


        //passing relevant data anout the asset/inventory
        inventoryIcon.sprite = item.objectSprite;
        label.text = item.objectName;
        inventoryIndex = invIndex;
        type = item.classification;


        SetNavigation();
    }

    void Inspect()
    {

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

            LoreReader.UpdateAndOpen(item);
        }
        else
        {
            AudioLore item = Inventory.Instance.Recordings[inventoryIndex];
            RecordingPlayer.Instance.Activate(item);
        }


    }

    private void SetNavigation()
    {
        //linking UI together
        Navigation temp;
        if (inventoryIndex == 0)
        {
            //set this button to link to the appropriate tab
            temp = button.navigation;
            temp.selectOnLeft = tabs[(int)type];
            button.navigation = temp;

            //link the tab to this button
            temp = tabs[(int)type].navigation;
            temp.selectOnRight = button;
        }
        else
        {
            //find this button's place in the transform heirarchy of the layout it is in
            Button[] selectables = gameObject.transform.parent.GetComponentsInChildren<Button>();
            int uiIndex = gameObject.transform.GetSiblingIndex();

            //link this button to the previous button
            temp = button.navigation;
            temp.selectOnLeft = selectables[uiIndex - 1];
            button.navigation = temp;

            //link previous button to this button
            temp = selectables[uiIndex - 1].navigation;
            temp.selectOnRight = button;
            selectables[uiIndex - 1].navigation = temp;
        }
    }
}
