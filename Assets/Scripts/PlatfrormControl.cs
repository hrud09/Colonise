using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatfrormControl : MonoBehaviour
{

    public bool isInitialPlatform;
    public Transform characterHolderPosition;

    public PlatformManager platformManager;

    public CharacterControl Character; //TODO: Later replaced with spawning system in a grid

    public ParticleSystem loveParticle;


    public void assignCharacter()
    {
        if (isInitialPlatform)
        {
            Character = Instantiate(platformManager.peopleManager.spawnCharacter(isInitialPlatform));
            platformManager.peopleManager.characters.Add(Character);
            Character.holdingPlatform = this;
            Character.AppearPlayer();

            isInitialPlatform = false;
        }

    }

    public void SpawnACharacter(bool fromButton = false)
    {
        float delay = 1f;

        if (fromButton)
        {
            delay = 0f;
        }
        else
        {
            ParticleSystem newLoveParticle = Instantiate(loveParticle);
            newLoveParticle.transform.position = characterHolderPosition.position + new Vector3(0f, 1f, 0f);
        }

        DOVirtual.DelayedCall(delay, () =>
        {
            Character = Instantiate(platformManager.peopleManager.spawnCharacter(false));
            platformManager.peopleManager.characters.Add(Character);
            Character.holdingPlatform = this;
            Character.AppearPlayer();

            isInitialPlatform = false;
        });

    }


    public void SpawnFromChild(PeopleType peopleType)
    {
        if (peopleType == PeopleType.Boy)
        {
            Character = Instantiate(platformManager.peopleManager.Man);
            platformManager.peopleManager.characters.Add(Character);
            Character.holdingPlatform = this;
            Character.AppearPlayer();

        }
        else
        {
            Character = Instantiate(platformManager.peopleManager.Woman);
            platformManager.peopleManager.characters.Add(Character);
            Character.holdingPlatform = this;
            Character.AppearPlayer();
        }
    }

}
