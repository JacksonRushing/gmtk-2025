using Unity.Mathematics;
using UnityEngine;

[ExecuteAlways]
public class Speedometer : MonoBehaviour
{
    public float speed;
    public float topSpeed;

    [SerializeField] private float speedRatio;
    public float startAngleRad;
    public float maxAngleRad;
    public Shapes.Disc revCircle;
    void Start()
    {
        revCircle.AngRadiansStart = startAngleRad;
    }
    [ExecuteAlways]
    void Update()
    {
        revCircle.AngRadiansStart = startAngleRad;

        speedRatio = Mathf.Abs(speed) / topSpeed;

        revCircle.AngRadiansEnd = Mathf.Lerp(startAngleRad, maxAngleRad, speedRatio);
        // revCircle.AngRadiansEnd = Mathf.Clamp(revCircle.AngRadiansEnd, startAngleRad, maxAngleRad);
    }
}
