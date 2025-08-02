using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float radius = 0.375f;
    public float width = 0.2f;
    public Transform wheelMesh;

    // Update is called once per frame
    void Update()
    {
        wheelMesh.transform.localScale = new Vector3(radius * 2, width, radius * 2);

    }
}
