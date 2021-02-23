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

    [SerializeField] private string PopUpName;


    private bool showing = false;

    private Vector3 offScreenUp;
    private Vector3 offScreenDown;
    private Vector3 offScreenLeft;
    private Vector3 offScreenRight;
    private Vector3 centre;
    private Vector3[] screenOffsets = new Vector3[4];

    private RectTransform rect;
    private Vector3 showingPosition;
    private Vector3 hiddenPosition;


    private void Start()
    {
        GameManager.Pause.AddListener(Toggle);

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


    public void Toggle(string identifier)
    {
        if (identifier == PopUpName)
        {
            if (showing)
            {
                Hide();
            }
            else
            {
                Show();
            }

            showing = !showing;
        }
    }



    public void Show()
    {
        LeanTween.cancel(gameObject);
        LeanTween.move(rect, showingPosition, 0.5f).setEase(LeanTweenType.easeOutQuint).setOnComplete(() => { rect.localPosition = showingPosition; });
        Debug.Log("Show " + showingPosition);
    }

    public void Hide()
    {
        LeanTween.cancel(gameObject);
        LeanTween.move(rect, hiddenPosition, 0.5f).setEase(LeanTweenType.easeOutQuint).setOnComplete(() => { rect.localPosition = hiddenPosition; });
        Debug.Log("Hide " + hiddenPosition);
    }

}
