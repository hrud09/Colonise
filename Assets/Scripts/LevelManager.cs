using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{

    Camera mainCamera;

    public PlayerControl player01, player02;
    public PlayerControl hitPlayer;

    LayerMask playerLayer;


    public Color selectionCOlor, DeselectionColor, PerfectMatchColor;

    public LineRenderer connectingLine;
    float connectingLineYElevation = -.2f;
    public GameObject SelectionIndicator;

    public Color CLCSelected, CLCDeselection, CLCPerfectMatch;
    public Material CLCMaterial;

    public ParticleSystem loveParticle;

    public PlayerControl Boy, Girl;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        playerLayer = LayerMask.GetMask("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.MaxValue, playerLayer))
            {
                hitPlayer = hit.transform.GetComponent<PlayerControl>();

                if (hitPlayer.moneyPanelControl)
                {
                    if (hitPlayer.moneyPanelControl.hasMoney)
                    {
                        hitPlayer.moneyPanelControl.collectMoney();
                    }
                }

                if (hitPlayer.MatingAction || !hitPlayer.canMate)
                {
                    ConnectingLineActivity();

                    return; // if hit player's mating is ghoing on
                }
                if (hitPlayer.peopleType == PeopleType.Man || hitPlayer.peopleType == PeopleType.Woman)
                {
                    if (!player01)
                    {
                        player01 = hitPlayer;
                        player01.isSelected = true;
                        player01.EnableSelctedImage(selectionCOlor);
                    }

                    else if (player01 && hitPlayer != player01)
                    {
                        if (player02 && hitPlayer != player02)
                        {
                            player02.DisableSelectedImage();
                            player02.isSelected = false;
                            player02 = null;
                        }

                        player02 = hitPlayer;

                        if (player01.peopleType == PeopleType.Man)
                        {
                            if (player02.peopleType == PeopleType.Man)
                            {
                                player01.EnableSelctedImage(DeselectionColor);
                                player02.EnableSelctedImage(DeselectionColor);

                                CLCMaterial.color = CLCDeselection;
                                

                            }
                            else if (player02.peopleType == PeopleType.Woman)
                            {
                                player01.EnableSelctedImage(PerfectMatchColor);
                                player02.EnableSelctedImage(PerfectMatchColor);

                                CLCMaterial.color = CLCPerfectMatch;


                            }
                        }


                        else if (player01.peopleType == PeopleType.Woman)
                        {
                            if (player02.peopleType == PeopleType.Man)
                            {
                                player01.EnableSelctedImage(PerfectMatchColor);
                                player02.EnableSelctedImage(PerfectMatchColor);

                                CLCMaterial.color = CLCPerfectMatch;


                            }
                            else if (player02.peopleType == PeopleType.Woman)
                            {
                                player01.EnableSelctedImage(DeselectionColor);
                                player02.EnableSelctedImage(DeselectionColor);

                                CLCMaterial.color = CLCDeselection;


                            }
                        }
                        player02.isSelected = true;


                    }
                }
 
            }
            
            else if (player01)
            {
                if (player02)
                {
                    player01.EnableSelctedImage(selectionCOlor);
                    CLCMaterial.color = CLCSelected;

                    player02.DisableSelectedImage();
                    player02.isSelected = false;
                    player02 = null;
                }
                //hitPlayer = null;
            }

            ConnectingLineActivity();
        }

        if(Input.GetMouseButtonUp(0))
        {
            connectingLine.enabled = false;
            SelectionIndicator.SetActive(false);

            MatePlayer();

        }
    }


    public void ConnectingLineActivity()
    {
        float radius = .5f;
        float angle;

        Vector3 connectingLineFirstPosition = Vector3.zero, connectingLineSecondPosition = Vector3.zero;
        Vector3 direction;


        if (player01)
        {

            connectingLineFirstPosition = new Vector3(player01.transform.position.x, connectingLineYElevation, player01.transform.position.z);

            connectingLine.enabled = true;
            SelectionIndicator.SetActive(true);

            if (!player02)
            {
                LayerMask groundLayer = LayerMask.GetMask("Ground");

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, float.MaxValue, groundLayer))
                {
                    connectingLineSecondPosition = new Vector3(hit.point.x, connectingLineYElevation, hit.point.z);

                    direction = connectingLineSecondPosition - connectingLineFirstPosition;
                    angle = Mathf.Atan2(direction.z, direction.x);
                    connectingLineFirstPosition += new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));

                    SelectionIndicator.transform.position = connectingLineSecondPosition;

                    if (Vector3.Distance(connectingLineFirstPosition, connectingLineSecondPosition) > .75f)
                    {
                        connectingLine.enabled = true;
                        SelectionIndicator.SetActive(true);

                    }
                    else
                    {
                        connectingLine.enabled = false;
                        SelectionIndicator.SetActive(false);
                    }


                    direction = connectingLineFirstPosition - connectingLineSecondPosition;
                    angle = Mathf.Atan2(direction.z, direction.x);
                    connectingLineSecondPosition += new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));



                    connectingLine.SetPosition(0, connectingLineFirstPosition);
                    connectingLine.SetPosition(1, connectingLineSecondPosition);
                }

            }

            else if(player02)
            {
                connectingLineSecondPosition = new Vector3(player02.transform.position.x, connectingLineYElevation, player02.transform.position.z);

                SelectionIndicator.SetActive(false);

                direction = connectingLineSecondPosition - connectingLineFirstPosition;
                Debug.DrawRay(connectingLineFirstPosition, direction, Color.black);
                angle = Mathf.Atan2(direction.z, direction.x);
                connectingLineFirstPosition += new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));

                direction = connectingLineFirstPosition - connectingLineSecondPosition;
                angle = Mathf.Atan2(direction.z, direction.x);
                connectingLineSecondPosition += new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));


                connectingLine.SetPosition(0, connectingLineFirstPosition);
                connectingLine.SetPosition(1, connectingLineSecondPosition);

            }
        }

    }


    public void MatePlayer()
    {
        if (player01 && player02)
        {
            //TODO: Also check if they are opposite sex and adult and has the time to mate

            if ((player01.peopleType == PeopleType.Man && player02.peopleType == PeopleType.Woman) ||
                (player01.peopleType == PeopleType.Woman && player02.peopleType == PeopleType.Man))
            {

                //Vector3 middlePosition = (player01.transform.position + player02.transform.position) / 2f;

                //player01.agent.SetDestination(middlePosition);
                //player02.agent.SetDestination(middlePosition);

                player01.agent.SetDestination(player02.transform.position);
                player02.agent.SetDestination(player02.transform.position);

                StartCoroutine(MateTwoPlayers(player01, player02));
            }
            else
            {
                player01.DisableSelectedImage();
                player02.DisableSelectedImage();

                player01.isSelected = false;
                player02.isSelected = false;

            }
        }
        else if(player01)
        {
            player01.DisableSelectedImage();
            player01.isSelected = false;
        }
        player01 = null;
        player02 = null;

    }


    IEnumerator MateTwoPlayers(PlayerControl player_01, PlayerControl player_02)
    {
        StartCoroutine(player_01.CheckForDestinationReached(true));
        StartCoroutine(player_02.CheckForDestinationReached(false));

        while (true)
        {

            float distance = Vector3.Distance(player_01.transform.position, player_02.transform.position);
            if (distance < 1f)
            {
                player_01.ReachedDestination = true;
                player_02.ReachedDestination = true;
                break;
            }
            yield return null;
        }


        Vector3 middlePosition = (player_01.transform.position + player_02.transform.position) / 2f + new Vector3(0f, 1f, 0f);
        ParticleSystem newloveParticle = Instantiate(loveParticle);
        newloveParticle.transform.position = middlePosition;

        DOVirtual.DelayedCall(1.5f, () =>
        {
            PlayerControl playerToSpawn;

            PeopleGenderCount();

            if (peopleCount.maleCount == peopleCount.femaleCount)
            {
                playerToSpawn = Random.Range(0, 2) == 0 ? Boy : Girl;

            }
            else if (peopleCount.maleCount > peopleCount.femaleCount)
            {
                playerToSpawn = Girl;
            }
            else
            {
                playerToSpawn = Boy;
            }


            middlePosition = (player_01.transform.position + player_02.transform.position) / 2f;

            PlayerControl newPlayer = Instantiate(playerToSpawn);
            newPlayer.transform.position = middlePosition;


            DOVirtual.DelayedCall(.75f, () =>
            {
                player_01.MatingAction = false;
                player_02.MatingAction = false;

                player_01.canMate = false;
                player_02.canMate = false;

                //player_01.progressBarControl.ActivateFillBar(9f, player_01);
                //player_02.progressBarControl.ActivateFillBar(9f, player_02);
            });


            DOVirtual.DelayedCall(3f, () =>
            {
                player_01.GoToNewDestination();
                player_02.GoToNewDestination();
            });

        });

    }


    class peopleTypeCount
    {
        public int maleCount;
        public int femaleCount;
    }

    peopleTypeCount peopleCount = new peopleTypeCount();

    public void PeopleGenderCount()
    {
        PlayerControl[] players = FindObjectsOfType<PlayerControl>();

        peopleCount.maleCount = 0;
        peopleCount.femaleCount = 0;

        foreach (PlayerControl player in players)
        {
            if (player.peopleType == PeopleType.Boy || player.peopleType == PeopleType.Man)
            {
                peopleCount.maleCount++;
            }
            else
            {
                peopleCount.femaleCount++;
            }
        }
    }

}

