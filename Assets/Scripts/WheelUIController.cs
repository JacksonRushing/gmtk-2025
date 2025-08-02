using System.Collections.Generic;
using UnityEngine;

public class WheelUIController : MonoBehaviour
{
    public List<Suspension> wheels;

    public List<Shapes.Rectangle> outlines;
    public List<Shapes.Rectangle> fills;

    public List<TMPro.TextMeshProUGUI> offsetTexts;

    void Update()
    {
        if (outlines.Count != 4 || fills.Count != 4)
        {
            Debug.LogError("invalid number of rects");
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            Color color = Color.Lerp(Color.white, Color.red, wheels[i].slipPercentage);

            if (wheels[i].didHit)
            {
                color.a = 1.0f;
            }
            else
            {
                color.a = 0.1f;
            }

            fills[i].Color = color;

            offsetTexts[i].text = wheels[i].Offset.ToString("0.00");
        }
    }
}
