using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButtonControl : MonoBehaviour
{

    public Image fillBar;

    float fillBarAmount = 0;

    private void Update()
    {
        fillBar.fillAmount = fillBarAmount / 100f;
    }

    private void Start()
    {
        StartCoroutine(fillBarActivity());
    }

    public void OnSpawnButtonPressed()
    {
        fillBarAmount += 10f;
        if (fillBarAmount >= 100f)
        {
            PlatformManager.Instance.SpawnPlayerInFreePlatform(true);
            fillBarAmount = 0;
        }
    }

    IEnumerator fillBarActivity()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (fillBarAmount >= 100f)
            {
                PlatformManager.Instance.SpawnPlayerInFreePlatform(true);
                fillBarAmount = 0;
            }
            else
            {
                fillBarAmount += 10f;
            }
        }
    }
}
