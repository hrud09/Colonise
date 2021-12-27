using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;




public class PlayerControl : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;


    public PeopleType peopleType;


    public bool isSelected;
    public bool MatingAction;
    public bool ReachedDestination;
    public bool canMate = true;

    public GameObject SelectionImage;
    public ProgressBarControl progressBarControl;
    public float yElevation = -.3f;

    Animator animator;
    float velocity;

    public PlayerControl Man, Woman;

    public MoneyPanelControl moneyPanelControl;

    public bool InitialPlayer;

    public GridPointControl currentGridPoint;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();


        if(yElevation != 0)agent.baseOffset = yElevation;
        transform.DORotate(new Vector3(0f, Random.Range(0, 360), 0f), 0f, RotateMode.Fast);

        if(SelectionImage) SelectionImage.transform.localScale = Vector3.zero;

        canMate = true;


        if (peopleType == PeopleType.Boy || peopleType == PeopleType.Girl)
        {
            DOVirtual.DelayedCall(.25f, ()=>
            {
                //progressBarControl.ActivateFillBar(6f, this);

            });

        }

        if(moneyPanelControl) moneyPanelControl.playerControl = this;
    }



    private void Start()
    {
        agent.isStopped = true;

        if (InitialPlayer)
        {
            currentGridPoint = GridSystemControl.Instance.FreePosition();
            if (currentGridPoint)
            {
                transform.position = currentGridPoint.GridPointPosition;

            }

        }

        if (peopleType == PeopleType.Boy || peopleType == PeopleType.Girl)
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                agent.isStopped = false;

                currentGridPoint = GridSystemControl.Instance.FreePosition();

                if (currentGridPoint)
                {
                    agent.SetDestination(currentGridPoint.GridPointPosition);

                }
            });
        }

    }

    // Update is called once per frame
    void Update()
    {
        velocity = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("WalkSpeed", velocity);
    }

    public IEnumerator RandomWonder()
    {
        yield return new WaitForSeconds(Random.Range( 1f, 2.5f));

        while(true)
        {
            while (isSelected && MatingAction)
            {
                yield return new WaitForSeconds(.25f);
            }

            Vector3 randomPosition = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
            agent.SetDestination(transform.position + randomPosition);

            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }

    }

    public void GoToNewDestination()
    {
        if (currentGridPoint)
        {
            currentGridPoint.Free = 0;
        }

        currentGridPoint = currentGridPoint = GridSystemControl.Instance.FreePosition();
        if (currentGridPoint)
        {
            agent.SetDestination(currentGridPoint.GridPointPosition);

        }
    }

    public IEnumerator CheckForDestinationReached(bool FreeGridPoint)
    {
        MatingAction = true;
        agent.isStopped = false;

        animator.SetBool("Waving", false);

        if (currentGridPoint && FreeGridPoint)
        {
            currentGridPoint.Free = 0;
        }

        while (!ReachedDestination)
        {
            yield return null;
        }

        ReachedDestination = false;
        DisableSelectedImage();
    }


    public void EnableSelctedImage(Color selectionColor)
    {
        agent.isStopped = true;

        animator.SetBool("Waving", true);

        if (!DOTween.IsTweening(transform))
        {
            transform.DORotate(new Vector3(0f, 180f, 0f), .25f, RotateMode.FastBeyond360);
        }

        

        SelectionImage.GetComponent<SpriteRenderer>().color = selectionColor;

        if (!DOTween.IsTweening(SelectionImage.transform))
        {
            SelectionImage.transform.DOScale(.25f, .25f).SetEase(Ease.OutBack);
        }


        SelectionImage.SetActive(true);
    }

    public void DisableSelectedImage()
    {
        //agent.isStopped = false;
        animator.SetBool("Waving", false);
        SelectionImage.SetActive(false);
        SelectionImage.transform.localScale = Vector3.zero;
    }

    public void EndOfProgressBar()
    {
        if (peopleType == PeopleType.Man || peopleType == PeopleType.Woman)
        {
            canMate = true;
            progressBarControl.ResetPanel();
        }
        else if (peopleType == PeopleType.Boy)
        {
            PlayerControl newPeople = Instantiate(Man);
            newPeople.transform.position = transform.position;
            newPeople.currentGridPoint = currentGridPoint;
            Destroy(gameObject);
        }
        else if (peopleType == PeopleType.Girl)
        {
            PlayerControl newPeople = Instantiate(Woman);
            newPeople.transform.position = transform.position;
            newPeople.currentGridPoint = currentGridPoint;
            Destroy(gameObject);
        }


    }
}
