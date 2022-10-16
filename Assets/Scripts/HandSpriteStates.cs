using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSpriteStates : MonoBehaviour
{
    [SerializeField]
    private Sprite _default;
    
    [SerializeField]
    private Sprite _grab;
    
    [SerializeField]
    private Sprite _grabBottle;

    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private GrabController _grabController;
    
    private void Update()
    {
        if (_grabController.Current == null)
        {
            _renderer.sprite = _default;
            return;
        }
        
        if (_grabController.Current.GetComponent<InHandFoodState>())
        {
            _renderer.sprite = _grab;
            return;
        }
        
        if (_grabController.Current.GetComponent<BottleController>())
        {
            _renderer.sprite = _grabBottle;
        }
    }
}
