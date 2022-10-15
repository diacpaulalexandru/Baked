using System;
using UnityEngine;

public class GrabEventListener : MonoBehaviour
{
    public event Action Grab;
    
    public void DoGrab()
    {
        Grab?.Invoke();
    }
}