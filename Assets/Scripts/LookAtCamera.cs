using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [Button]
    private void Update()
    {
        // transform.LookAt(Camera.main.transform);
        transform.rotation = Camera.main.transform.rotation;
    }
}