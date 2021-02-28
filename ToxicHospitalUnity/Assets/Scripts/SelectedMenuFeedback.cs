using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedMenuFeedback : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        MenuSFXController.Instance.OnItemSelected();
    }
}
