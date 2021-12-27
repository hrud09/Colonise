using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPointControl : MonoBehaviour
{
    public int Free = 0;
    public Vector3 GridPointPosition;

    private void Awake()
    {
        GridPointPosition = transform.position;
    }
}
