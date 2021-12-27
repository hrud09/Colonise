using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMesh : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
