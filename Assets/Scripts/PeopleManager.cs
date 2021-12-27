using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{

    public static PeopleManager Instance;

    public List<CharacterControl> characters = new List<CharacterControl>();
    public CharacterControl Man, Woman, Boy, Girl;

    private void Awake()
    {
        Instance = this;
    }

    public CharacterControl spawnCharacter(bool InitialCharacters)
    {
        int maleNumber = 0;
        int femaleNumber = 0;

        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].peopleType == PeopleType.Boy || characters[i].peopleType == PeopleType.Man)
            {
                maleNumber++;
            }
            else
            {
                femaleNumber++;
            }
        }

        CharacterControl characterToGive = null;

        if (maleNumber == femaleNumber)
        {
            if (InitialCharacters)
            {
                characterToGive = Random.Range(0, 2) == 0 ? Man : Woman;

            }
            else
            {
                characterToGive = Random.Range(0, 2) == 0 ? Boy : Girl;

            }
        }
        else if (maleNumber > femaleNumber)
        {
            if (InitialCharacters)
            {
                characterToGive = Woman;
            }
            else
            {
                characterToGive = Girl;
            }
        }
        else
        {
            if (InitialCharacters)
            {
                characterToGive = Man;
            }
            else
            {
                characterToGive = Boy;
            }
        }

        return characterToGive;

    }
}
