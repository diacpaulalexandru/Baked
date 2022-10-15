using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameController : MonoBehaviour
{
    [Serializable]
    public class StateDefinition
    {
        [Serializable]
        public class ItemDefinition
        {
            public GameObject Item;
            public int Count;
        }

        public ItemDefinition[] Items;
    }

    public StateDefinition[] States;

    private int _level;
    
    private void Start()
    {
        SpawnState();
    }

    private void SpawnState()
    {
        var state = States[_level];
        
        foreach (var itemDefinition in state.Items)
        {
            for (var i = 0; i < itemDefinition.Count; i++)
            {
                var instance = Object.Instantiate(itemDefinition.Item);
            }
            
        }
    }
    
}
