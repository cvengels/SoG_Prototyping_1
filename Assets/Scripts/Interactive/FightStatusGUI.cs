using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FightStatusGUI : MonoBehaviour
{

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject indicator;
    
    [SerializeField] private GameObject catHeadUI;
    [SerializeField] private GameObject mouseHeadUI;

    [SerializeField] private Vector2 normalHeadSize = new Vector2(0.6f, 0.6f);
    [SerializeField] private Vector2 bigHeadSize = new Vector2(2f, 2f);

    [SerializeField] private float relativePosition;

    [SerializeField] private SpriteRenderer indicatorColor;
    
    private float backgroundWidth;
    private Vector2 indicatorPostion;

    private void Awake()
    {
        backgroundWidth = background.transform.lossyScale.x / 2;
        indicatorColor = indicator.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        catHeadUI.transform.localScale = Vector2.Lerp(catHeadUI.transform.localScale, normalHeadSize, 0.1f);
        mouseHeadUI.transform.localScale = Vector2.Lerp(mouseHeadUI.transform.localScale, normalHeadSize, 0.1f);

        if (relativePosition < 0.5f)
        {
            indicatorColor.color = Color.Lerp(Color.red, Color.yellow, Mathf.Lerp(0f, 1f, relativePosition * 2));
        }
        else
        {
            indicatorColor.color = Color.Lerp(Color.green, Color.yellow, Mathf.Lerp(1f, 0f,(relativePosition - 0.5f) * 2));
        }
    }

    public void AdjustIndicator(CharType character, int buttonPressesTotal, int buttonPressesCurrent)
    {
        if (character == CharType.Cat)
        {
            catHeadUI.transform.localScale = bigHeadSize;
        }
        else if (character == CharType.Mouse)
        {
            mouseHeadUI.transform.localScale = bigHeadSize;
        }
        
        relativePosition = (float)buttonPressesCurrent / (float)buttonPressesTotal;
        print($"Indicator position: {relativePosition}");
        
        indicatorPostion = new Vector2(Mathf.Lerp(backgroundWidth * -1, backgroundWidth, relativePosition), 0f);

        indicator.transform.localPosition = indicatorPostion;
    }
}
