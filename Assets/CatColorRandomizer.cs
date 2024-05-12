using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatColorRandomizer : MonoBehaviour
{
    [SerializeField] Colors[] colors;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Material _material;

    private void Awake()
    {
        _material = spriteRenderer.material;
        SetRandomColor();
        
    }

    void SetRandomColor()
    {
        int _rand = UnityEngine.Random.Range(0, colors.Length);
        Colors _selectedColors = colors[_rand];

        _material.SetColor("_Color_1", _selectedColors.color[0]);
        _material.SetColor("_Color_2", _selectedColors.color[1]);
        _material.SetColor("_Color_3", _selectedColors.color[2]);
    }

    [Serializable]
    public class Colors
    {
        public Color[] color;
    }
}
