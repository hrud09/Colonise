using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public Transform hitPlayer;
    public Transform hitPlatform;

    Camera mainCamera;
    LayerMask playerLayer;
    LayerMask groundLayer;
    LayerMask platfromLayer;

    float yElevation = .25f;

    public GameObject House;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        playerLayer = LayerMask.GetMask("Player");
        groundLayer = LayerMask.GetMask("Ground");
        platfromLayer = LayerMask.GetMask("Platform");


    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!hitPlayer)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, float.MaxValue, playerLayer))
                {
                    if (!hit.transform.GetComponent<CharacterControl>().mated)
                    {
                        hitPlayer = hit.transform;

                    }
                }
            }


        }


        if (Input.GetMouseButtonUp(0))
        {
            CharacterControl hitPlayerControl = null;
            PlatfrormControl hitPlatfrormControl = null;

            if (hitPlayer) hitPlayerControl = hitPlayer.GetComponent<CharacterControl>();
            if (hitPlatform) hitPlatfrormControl = hitPlatform.GetComponent<PlatfrormControl>();

            if (hitPlayerControl)
            {
                if (hitPlatfrormControl)
                {

                    if (hitPlatfrormControl.Character == null) // The platform is free and player can move here
                    {
                        hitPlayerControl.holdingPlatform.Character = null;
                        hitPlayerControl.holdingPlatform = hitPlatfrormControl;
                        hitPlayerControl.SnapToPosition();

                    }

                    if (hitPlatfrormControl.Character) // if there is already existing player check if they can mate if not swap
                    {
                        if (hitPlatfrormControl.Character.peopleType == PeopleType.Boy || hitPlatfrormControl.Character.peopleType == PeopleType.Girl
                            || hitPlayerControl.peopleType == PeopleType.Boy || hitPlayerControl.peopleType == PeopleType.Girl)
                        {
                            //TODO: Swap Players

                            SwapPlayer(hitPlayerControl, hitPlatfrormControl.Character);
                        }


                        else if ((hitPlayerControl.peopleType == PeopleType.Man && hitPlatfrormControl.Character.peopleType == PeopleType.Man)
                            || (hitPlayerControl.peopleType == PeopleType.Woman && hitPlatfrormControl.Character.peopleType == PeopleType.Woman))
                        {
                            //TODO: Swap Player
                            SwapPlayer(hitPlayerControl, hitPlatfrormControl.Character);

                        }

                        else //Swap Player
                        {
                            MatePlayer(hitPlayerControl, hitPlatfrormControl.Character);
                        }
                    }
                }

                else
                {
                    hitPlayerControl.SnapToPosition();
                }


            }


            hitPlayer = null;
            hitPlatform = null;
        }

        if (hitPlayer)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.MaxValue, groundLayer))
            {
                yElevation = hitPlayer.GetComponent<CharacterControl>().yElevation;

                Vector3 movePositiopn = new Vector3(hit.point.x, yElevation, hit.point.z);
                hitPlayer.transform.position = movePositiopn;

            }

            if (Physics.Raycast(ray, out hit, float.MaxValue, platfromLayer))
            {
                hitPlatform = hit.transform;
            }
            else
            {
                hitPlatform = null;
            }
        }


    }

    public void SwapPlayer(CharacterControl player01, CharacterControl player02)
    {
        PlatfrormControl tempPlatform = player01.holdingPlatform;
        player01.holdingPlatform = player02.holdingPlatform;
        player02.holdingPlatform = tempPlatform;

        player01.SnapToPosition();
        player02.SnapToPosition();
    }


    public void MatePlayer(CharacterControl player01, CharacterControl player02)
    {
        PlatfrormControl matingPlatform = player02.holdingPlatform;
        player01.holdingPlatform.Character = null;
        player01.holdingPlatform = matingPlatform;

        player01.SnapToMatingPosition(true);
        player02.SnapToMatingPosition(false);

        player01.mated = true;
        player02.mated = true;

        matingPlatform.Character = null;

        matingPlatform.SpawnACharacter();
    }

}
