using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualLoreCanvasManager : MonoBehaviour
{
    static VisualLoreCanvasManager instance;
    public static VisualLoreCanvasManager Instance { get { return instance; } }

    [SerializeField]
    private Image generalBackdrop;
    [SerializeField]
    private SpriteRenderer visualLoreImage;
    [SerializeField]
    private Image textBackdrop;
    [SerializeField]
    private Text visualLoreText;

    private bool showingText = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowUIText(false);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            ReadInput();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            ExitInput();
        }
    }

    public void UpdateAndOpen(VisualLore vl)
    {
        visualLoreImage.sprite = vl.objectSprite;
        visualLoreText.text = vl.objectText;
        OpenUI();
    }

    private void OpenUI()
    {
        gameObject.SetActive(true);
        ShowUIText(false);
    }

    private void CloseUI()
    {
        ShowUIText(false);
        gameObject.SetActive(false);
    }

    private void ShowUIText(bool showText)
    {
        generalBackdrop.enabled = !showText;
        textBackdrop.enabled = showText;
        visualLoreText.enabled = showText;
        showingText = showText;
    }

    private void ExitInput()
    {
        // if reading exit that
        if (showingText)
        {
            ShowUIText(false);
        }
        else
        {
            CloseUI();
        }

    }

    private void ReadInput()
    {
        // display text
        ShowUIText(!showingText);
    }

    public void ExitButtonPush()
    {
        ExitInput();
    }

    public void ReadButtonPush()
    {
        ReadInput();
    }
}
