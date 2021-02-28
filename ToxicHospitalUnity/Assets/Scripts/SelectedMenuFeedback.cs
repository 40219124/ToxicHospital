using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Selectable))]
public class SelectedMenuFeedback : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    private Selectable selectable;

    private void Start()
    {
        selectable = GetComponent<Selectable>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectable.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        MenuSFXController.Instance.OnItemSelected();
    }
}
