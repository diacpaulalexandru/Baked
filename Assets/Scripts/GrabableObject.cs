using System;
using UnityEngine;

public class GrabableObject : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

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
    
}