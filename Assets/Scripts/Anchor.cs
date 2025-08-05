using UnityEngine;

public class Anchor : MonoBehaviour
{
    public bool highlighted;

    [SerializeField] private GameObject highlight;
    [SerializeField] private Rigidbody rigidbody;


    // Update is called once per frame
    void Update()
    {
        highlight.SetActive(highlighted);
    }

    public Rigidbody GetRigidbody()
    {
        return rigidbody;
    }
}
