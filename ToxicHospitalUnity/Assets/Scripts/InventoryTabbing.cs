using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryTabbing : MonoBehaviour, ISelectHandler
{
    private GameObject pageGroup;
    private List<Transform> pages = new List<Transform>();
    private int index;



    void Start()
    {
        pageGroup = GameObject.Find("PageGroup");

        foreach (Transform t in pageGroup.transform)
        {
            pages.Add(t);
        }
        //pages = new List<Transform>(pageGroup.GetComponentsInChildren<Transform>());

        // get index as the order the tab/page is listed in the hierarchy
        index = gameObject.transform.GetSiblingIndex();

        //add ShowTab to the Button component
        gameObject.GetComponent<Button>().onClick.AddListener(ShowTab);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("OnSelect");
        ShowTab();
    }

    private void ShowTab()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            if (i == index)
            {
                pages[i].gameObject.SetActive(true);
            }
            else
            {
                pages[i].gameObject.SetActive(false);
            }

        }
    }
}
