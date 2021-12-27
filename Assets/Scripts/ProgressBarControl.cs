using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ProgressBarControl : MonoBehaviour
{
    Transform cam;

    public GameObject fillBarGO;
    public Image fillBar;


    public MoneyPanelControl moneyPanelControl;

    public void ActivateFillBar(float timeToFill, float startDelay ,CharacterControl characterControl)
    {

        if(moneyPanelControl) moneyPanelControl.ProgressBarActivate();

        DOVirtual.DelayedCall(startDelay, () =>
        {
            fillBarGO.transform.DOScale(1f, .25f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                fillBar.DOFillAmount(1f, timeToFill).OnComplete(() =>
                {
                    if (moneyPanelControl) moneyPanelControl.ProgressBarDeativate();
                    characterControl.EndOfProgressBar();
                    //playerControl.EndOfProgressBar();
                });

            });

            fillBarGO.SetActive(true);
        });


    }


    void Start()
    {
        cam = Camera.main.transform;
        fillBar.fillAmount = 0f;
        ResetPanel();
    }


    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }

    public void ResetPanel()
    {
        fillBar.fillAmount = 0f;
        fillBarGO.SetActive(false);
        fillBarGO.transform.localScale = Vector3.zero;
    }
}
