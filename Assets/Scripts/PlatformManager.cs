using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager Instance;

    public List<PlatfrormControl> platfrorms = new List<PlatfrormControl>();

    public PeopleManager peopleManager;

    private void Awake()
    {

        Instance = this;

        foreach (Transform child in transform)
        {
            PlatfrormControl childPlatform = child.GetComponent<PlatfrormControl>();
            childPlatform.platformManager = this;
            childPlatform.assignCharacter();
            platfrorms.Add(childPlatform);

        }
    }


    public void SpawnPlayerInFreePlatform(bool frombutton = false)
    {
        ShufflePlatforms(platfrorms);
        for (int i = 0; i < platfrorms.Count; i++)
        {
            if (platfrorms[i].Character == null)
            {
                platfrorms[i].SpawnACharacter(frombutton);
                break;
            }
        }

    }

    void ShufflePlatforms(List<PlatfrormControl> a)
    {
        // Loops through array
        for (int i = a.Count - 1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            int rnd = Random.Range(0, i);

            // Save the value of the current i, otherwise it'll overright when we swap the values
            PlatfrormControl temp = a[i];

            // Swap the new and old values
            a[i] = a[rnd];
            a[rnd] = temp;
        }

    }
}
