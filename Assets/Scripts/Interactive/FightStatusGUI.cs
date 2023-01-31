using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FightStatusGUI : MonoBehaviour
{

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject indicator;

    private float backgroundWidth;
    private Vector2 indicatorPostion;

    private void Awake()
    {
        backgroundWidth = background.transform.lossyScale.x / 2;
    }

    public void AdjustIndicator(int buttonPressesTotal, int buttonPressesCurrent)
    {
        float relativePosition = (float)buttonPressesCurrent / (float)buttonPressesTotal;
        print($"Indicator position: {relativePosition}");
        
        indicatorPostion = new Vector2(Mathf.Lerp(backgroundWidth * -1, backgroundWidth, relativePosition), 0f);

        indicator.transform.localPosition = indicatorPostion;
    }
}
