using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableObject : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    
    public void PickUp()
    {
        _rigidbody.isKinematic = true;
    }

    public void Drop()
    {
        _rigidbody.isKinematic = false;
    }
    
}