using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ListDisplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    private RectTransform _rectTransform;

    [SerializeField]
    private TMP_Text _generalText;
    
    private Dictionary<IngredientDefinition, List<CutState>> _dict;

    private void Awake()
    {
        var controller = FindObjectOfType<GameController>();
        controller.Spawned += Spawned;
    }

    private void Update()
    {
        if (_rectTransform.childCount == 0)
            _generalText.text = "Haide bea, ai incredere!";
    }

    private void Spawned()
    {
        for (int i = 0; i < _rectTransform.childCount; i++)
        {
            var ch = _rectTransform.GetChild(i);
            Destroy(ch.gameObject);
        }

        _generalText.text = string.Empty;
        
        _dict = new Dictionary<IngredientDefinition, List<CutState>>();

        var items = FindObjectsOfType<CutState>();

        foreach (var target in items)
        {
            if (_dict.ContainsKey(target.Definition) == false)
                _dict.Add(target.Definition, new List<CutState>());

            _dict[target.Definition].Add(target);
        }

        foreach (var entry in _dict)
        {
            var instance = Instantiate(_prefab, _rectTransform);
            instance.transform.localScale = Vector3.one;
            instance.GetComponent<ListElement>().Setup(entry.Key, entry.Value);
        }
    }
}