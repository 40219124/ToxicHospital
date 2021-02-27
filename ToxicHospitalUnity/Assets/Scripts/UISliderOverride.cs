using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UISliderOverride : UISlider
{


    //Only care about if it's already showing, not if any UISliders are showing. Ignore UILock
    public override void Show()
    {

        //show this individual
        LeanTween.cancel(gameObject);
        LeanTween.move(rect, showingPosition, easeTime).setEase(LeanTweenType.easeOutQuint).setOnComplete(() => { rect.localPosition = showingPosition; });
        //Debug.Log("Show " + showingPosition);}
    }


    public override void Hide()
    {
        //hide this individual
        LeanTween.cancel(gameObject);
        LeanTween.move(rect, hiddenPosition, easeTime).setEase(LeanTweenType.easeOutQuint).setOnComplete(() => { rect.localPosition = hiddenPosition; });
        //Debug.Log("Hide " + hiddenPosition);
    }
}
