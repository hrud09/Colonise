using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemControl : MonoBehaviour
{

    public static GridSystemControl Instance;


    public List<GridPointControl> GridPoints = new List<GridPointControl>();


    private void Awake()
    {

        Instance = this;

        foreach (Transform child in transform)
        {
            GridPoints.Add(child.GetComponent<GridPointControl>());
        }
    }

    public GridPointControl FreePosition()
    {
        Shuffle(GridPoints);

        for (int i = 0; i < GridPoints.Count; i++)
        {
            if (GridPoints[i].Free == 0)
            {
                GridPoints[i].Free = 1;
                return GridPoints[i];
            }
        }

        return null;
    }

    void Shuffle(List<GridPointControl> a)
    {
        // Loops through array
        for (int i = a.Count - 1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            int rnd = Random.Range(0, i);

            // Save the value of the current i, otherwise it'll overright when we swap the values
            GridPointControl temp = a[i];

            // Swap the new and old values
            a[i] = a[rnd];
            a[rnd] = temp;
        }

    }
}
