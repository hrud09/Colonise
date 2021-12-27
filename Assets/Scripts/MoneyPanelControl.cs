using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MoneyPanelControl : MonoBehaviour
{

    Transform cam;
    public bool hasMoney = false;
    RectTransform rect;

    int money = 0;

    public ParticleSystem moneyParticle;
    public PlayerControl playerControl;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        cam = Camera.main.transform;
        ResetPanel();


        //StartCoroutine(GenerateMoney());

    }



    public void collectMoney()
    {

        ParticleSystem newMoneyParticle = Instantiate(moneyParticle);
        newMoneyParticle.transform.position = playerControl.transform.position;

        money = 0;
        hasMoney = false;
        DisablePanel();
    }

    public void ProgressBarActivate()
    {
        rect.DOAnchorPosY(45f, .25f);
    }

    public void ProgressBarDeativate()
    {
        rect.DOAnchorPosY(17f, .25f);

    }

    public IEnumerator GenerateMoney()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.5f, 4f));

            money++;
            if (money >= 10f)
            {
                hasMoney = true;
                EnablePanel();
            }
            else
            {
                DisablePanel();
            }
        }
    }

    public void EnablePanel()
    {
        transform.DOScale(1f, .25f).SetEase(Ease.OutBack);
    }

    public void DisablePanel()
    {
        transform.localScale = Vector3.zero;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }

    public void ResetPanel()
    {
        transform.localScale = Vector3.zero;
    }
}
