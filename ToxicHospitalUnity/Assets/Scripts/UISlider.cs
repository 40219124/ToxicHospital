using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISlider : MonoBehaviour
{
    protected Selectable lastSelected;

    public enum HideDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public HideDirection Direction;

    [SerializeField] protected string popUpName;
    [SerializeField] protected float easeTime;


    public static bool anyShowing = false;
    protected bool individualShowing = false;

    protected Vector3[] screenOffsets = new Vector3[4];

    protected RectTransform rect;
    protected Vector3 showingPosition;
    protected Vector3 hiddenPosition;
    protected static bool uiLock = false;


    private void Start()
    {
        //must subscribe method to each event individually
        GameManager.PauseEvent?.AddListener(Toggle);
        MainMenuManager.PauseEvent.AddListener(Toggle);
        GameManager.ToggleIventoryEvent?.AddListener(Toggle);

        screenOffsets[0] = new Vector3(0, Screen.height, 0);
        screenOffsets[1] = new Vector3(0, -Screen.height, 0);
        screenOffsets[2] = new Vector3(-Screen.width, 0, 0);
        screenOffsets[3] = new Vector3(Screen.width, 0, 0);
        Vector3 centre = new Vector3(0, 0, 0);

        rect = GetComponent<RectTransform>();
        showingPosition = rect.localPosition;
        hiddenPosition = showingPosition + screenOffsets[(int)Direction];

        rect.localPosition = hiddenPosition;

        lastSelected = GetComponentInChildren<Selectable>();
    }


    public virtual void Toggle(string identifier)
    {
        //if event intended for this individual
        if (identifier == popUpName)
        {
            //if this individual isn't showing and the UI isn't locked
            if (!individualShowing)
            {
                Show();
            }
            else
            {
                //hide
                Hide();
            }

        }
    }

    protected virtual void SelectSelectable()
    {
        lastSelected?.Select();
    }

    protected virtual void DeselectSelectable()
    {
        //Selectable current = lastSelected;
        Selectable current = EventSystem.current.currentSelectedGameObject?.GetComponent<Selectable>();
        if (current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            lastSelected = current;
        }
    }


    public virtual void Show()
    {
        if (!UISlider.uiLock)
        {
            //show this individual
            LeanTween.cancel(gameObject);
            LeanTween.move(rect, showingPosition, easeTime).setEase(LeanTweenType.easeOutQuint).setOnComplete(() => { rect.localPosition = showingPosition; });
            //Logger.Log("Show " + showingPosition);}

            //and lock the UI until it is hidden
            individualShowing = true;
            UISlider.uiLock = true;

            SelectSelectable();
        }

        //do nothing with UI Lock in place
    }

    public virtual void Hide()
    {
        //hide this individual
        LeanTween.cancel(gameObject);
        LeanTween.move(rect, hiddenPosition, easeTime).setEase(LeanTweenType.easeOutQuint).setOnComplete(() => { rect.localPosition = hiddenPosition; });
        //Logger.Log("Hide " + hiddenPosition);

        //free the UI lock
        individualShowing = false;
        UISlider.uiLock = false;

        DeselectSelectable();

        //do nothing with UI Lock in place
    }

    public bool IsShowing { get { return individualShowing; } }
}
