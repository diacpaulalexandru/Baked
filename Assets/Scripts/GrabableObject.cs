using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableObject : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    public Func<bool> CanPick;

    public event Action Grabbed;
    
    public event Action Dropped;
    
    public void PickUp()
    {
        Grabbed?.Invoke();
    }

    public void Drop()
    {
        Dropped?.Invoke();
    }

    public bool TryPick()
    {
        return CanPick.AllTrue();
    }
}