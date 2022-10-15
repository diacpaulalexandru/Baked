using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopController : MonoBehaviour
{
    public float ChopDuration;
    public bool Chopping { get; set; }

    [SerializeField]
    private Animator _animator;

    public string ChopAnimName = "Chop";
    
    public void Chop()
    {
        if(Chopping)
            return;
        StartCoroutine(ChopCoroutine());
    }

    private IEnumerator ChopCoroutine()
    {
        Chopping = true;
        _animator.SetBool(ChopAnimName, true);
        yield return new WaitForSeconds(ChopDuration);
        _animator.SetBool(ChopAnimName, false);
        Chopping = false;
    }
}