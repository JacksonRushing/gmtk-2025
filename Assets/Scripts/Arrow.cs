using UnityEngine;

[ExecuteAlways]
public class Arrow : MonoBehaviour
{
    public Vector3 start, end;
    public float lineWidth, arrowWidth, arrowLength;
    public Color color;


    [SerializeField] Shapes.Line line;
    [SerializeField] Shapes.Cone cone;
    public float length;
    [ExecuteAlways]
    void Update()
    {
        Vector3 normalizedDirection = (end - start).normalized;
        length = (end - start).magnitude;
        line.Start = start;
        line.End = start + (normalizedDirection * (length - arrowLength));

        Vector3 conePos = start + (normalizedDirection * (length - (arrowLength)));
        //Debug.Log(conePos);
        cone.transform.localPosition = conePos;
        cone.transform.forward = normalizedDirection;
        cone.Length = arrowLength;
        cone.Radius = arrowWidth / 2;

        line.Thickness = lineWidth;

        line.Color = color;
        cone.Color = color;
    }
}
