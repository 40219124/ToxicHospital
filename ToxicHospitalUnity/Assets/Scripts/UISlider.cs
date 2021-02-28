using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UISlider : MonoBehaviour
{


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
        GameManager.PauseEvent.AddListener(Toggle);
        GameManager.ToggleIventoryEvent.AddListener(Toggle);

        screenOffsets[0] = new Vector3(0, Screen.height, 0);
        screenOffsets[1] = new Vector3(0, -Screen.height, 0);
        screenOffsets[2] = new Vector3(-Screen.width, 0, 0);
        screenOffsets[3] = new Vector3(Screen.width, 0, 0);
        Vector3 centre = new Vector3(0, 0, 0);

        rect = GetComponent<RectTransform>();
        showingPosition = rect.localPosition;
        hiddenPosition = showingPosition + screenOffsets[(int)Direction];

        rect.localPosition = hiddenPosition;
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



    public virtual void Show()
    {
        if (!UISlider.uiLock)
        {
            //show this individual
            LeanTween.cancel(gameObject);
            LeanTween.move(rect, showingPosition, easeTime).setEase(LeanTweenType.easeOutQuint).setOnComplete(() => { rect.localPosition = showingPosition; });
            //Debug.Log("Show " + showingPosition);}

            //and lock the UI until it is hidden
            individualShowing = true;
            UISlider.uiLock = true;
        }

        //do nothing with UI Lock in place
    }

    public virtual void Hide()
    {
        if (UISlider.uiLock)
        {
            //hide this individual
            LeanTween.cancel(gameObject);
            LeanTween.move(rect, hiddenPosition, easeTime).setEase(LeanTweenType.easeOutQuint).setOnComplete(() => { rect.localPosition = hiddenPosition; });
            //Debug.Log("Hide " + hiddenPosition);

            //free the UI lock
            individualShowing = false;
            UISlider.uiLock = false;
        }

        //do nothing with UI Lock in place
    }

    public bool IsShowing { get { return individualShowing; } }
}
