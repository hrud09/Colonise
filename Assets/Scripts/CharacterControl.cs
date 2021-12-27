using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public enum PeopleType
{
    Boy,
    Girl,
    Man,
    Woman
}


public class CharacterControl : MonoBehaviour
{

    public float yElevation = .25f;
    public PlatfrormControl holdingPlatform;

    public ProgressBarControl progressBarControl;

    public PeopleType peopleType;

    Animator animator;

    public bool mated = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (peopleType == PeopleType.Boy || peopleType == PeopleType.Girl)
        {
            progressBarControl.ActivateFillBar(4f, 1f, this);
        }
    }

    public IEnumerator moveTowaredHouse()
    {

        Transform target = GameManager.Instance.House.transform;
        float distance = Vector3.Distance(transform.position, target.position);

        Vector3 targetDirection = target.position - transform.position;

        float speed = 2.5f;
        float rotationSpeed = 10f;

        animator.SetBool("Walk", true);

        while (distance > .001f)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            float singleStep = rotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            yield return null;
        }

        gameObject.SetActive(false);
    }

    public void EndOfProgressBar()
    {
        progressBarControl.ResetPanel();
        PeopleManager.Instance.characters.Remove(this);
        holdingPlatform.SpawnFromChild(peopleType);
        Destroy(gameObject);
    }

    public void AppearPlayer()
    {
        Vector3 position = holdingPlatform.characterHolderPosition.position;
        position.y = yElevation;

        transform.position = position;

    }

    public void SnapToPosition()
    {
        holdingPlatform.Character = this;

        Vector3 position = holdingPlatform.characterHolderPosition.position;
        position.y = yElevation;

        transform.DOMove(position, .25f);

    }

    public void SnapToMatingPosition(bool firstPlayer)
    {
        Vector3 positionToMove = holdingPlatform.characterHolderPosition.position;
        positionToMove.y = yElevation;

        if (firstPlayer)
        {
            positionToMove.x -= .25f;
            transform.DOMove(positionToMove, .25f);
            transform.DORotate(new Vector3(0f, 90f, 0f), .25f, RotateMode.FastBeyond360);

            positionToMove.x -= .25f;
            transform.DOMove(positionToMove, .1f).SetDelay(1f);

        }
        else
        {
            positionToMove.x += .25f;
            transform.DOMove(positionToMove, .25f);
            transform.DORotate(new Vector3(0f, -90f, 0f), .25f, RotateMode.FastBeyond360);

            positionToMove.x += .25f;
            transform.DOMove(positionToMove, .1f).SetDelay(1f);
        }


        DOVirtual.DelayedCall(Random.Range(1.5f, 2.25f), () =>
        {
            StartCoroutine(moveTowaredHouse());
        });
    }
}
