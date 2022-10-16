using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using SimpleFSM;
using UnityEngine;

public class CutState : BaseState
{
    public GameObject State;
    public GameObject Default;

    public IngredientDefinition Definition;

    [SerializeField]
    private float _delay = 0.5f;

    private void Start()
    {
        State.SetActive(false);
        Default.SetActive(true);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Default.SetActive(false);
        State.SetActive(true);

        var rgd = GetComponent<Rigidbody>();
        rgd.isKinematic = true;

        StartCoroutine(Wait());
        //transform.DOMove(oala.transform.position, 1);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_delay);

        var oala = GameObject.FindWithTag("Oala");
        transform.DOJump(oala.transform.position, 5, 1, 1);

        transform.DOScale(Vector3.zero, 1).SetEase(Ease.OutExpo).SetDelay(.5f).onComplete += OnComplete;
    }

    private void OnComplete()
    {
        Destroy(gameObject);
    }

    // public override void OnExit()
    // {
    //     base.OnExit();
    //     var oala = GameObject.FindWithTag("Oala");
    //     transform.DOJump(oala.transform.position, 5, 1, 1);
    //     Debug.Log("Exit");
    // }
}