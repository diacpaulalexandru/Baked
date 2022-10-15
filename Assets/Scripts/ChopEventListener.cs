using System;
using UnityEngine;

public class ChopEventListener : MonoBehaviour
{
    public event Action Chop;
    
    public void DoChop()
    {
        Chop?.Invoke();
    }
}