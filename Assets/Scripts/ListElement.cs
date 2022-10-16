using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListElement : MonoBehaviour
{
    private List<CutState> _list;

    private IngredientDefinition _def;

    public Image Image;
    public TMP_Text Text;
    public TMP_Text CountText;

    public void Setup(IngredientDefinition def, List<CutState> list)
    {
        _list = list;
        _def = def;
        CountText.text = list.Count.ToString();
        Text.text = def.PublicName;
        Image.sprite = def.FlySprite;
    }

    private void Update()
    {
        var count = 0;

        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i] != null)
                count++;
        }

        if (count == 0)
            Destroy(gameObject);
        else if (count == 1)
            CountText.text = string.Empty;
        else
            CountText.text = count.ToString();
    }
}