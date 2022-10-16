using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Action Spawned;

    private Transform[] _targets;

    private void Start()
    {
        _targets = GameObject.FindGameObjectsWithTag("Target").Select(o => o.transform).ToArray();
        SpawnState();
    }

    private Transform GetPoint()
    {
        var dict = new Dictionary<Transform, float>();
        
        var items = FindObjectsOfType<CutState>();
        
        foreach (var target in _targets)
        {
            dict.Add(target, 0);
        }

        foreach (var target in _targets)
        {
            foreach (var item in items)
            {
                dict[target] += Vector3.Distance(target.position, item.transform.position);
            }
        }

        var lowest = float.MinValue;
        Transform res=null;
        
        foreach (var t in dict)
        {
            //Debug.Log($"{t.Key} weight {t.Value}");
            if (t.Value > lowest)
            {
                lowest = t.Value;
                res = t.Key;
            }
        }
        return res;
    }

    private void SpawnState()
    {
        var state = States[_level];

        foreach (var itemDefinition in state.Items)
        {
            for (var i = 0; i < itemDefinition.Count; i++)
            {
                var pt= GetPoint().position;
                var instance = Object.Instantiate(itemDefinition.Item);
                instance.transform.position = pt + new Vector3(0, 1, 0);
            }
        }

        Spawned?.Invoke();
    }

    public void TryProgress()
    {
        var items = FindObjectsOfType<CutState>();
        if (items.Length > 0)
        {
            Debug.Log("Not yet collected all items.");
            return;
        }

        _level++;

        if (_level < States.Length)
        {
            SpawnState();
            return;
        }

        Debug.Log("Game completed!");
    }
}