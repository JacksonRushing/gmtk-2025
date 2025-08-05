using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    public float velocityLookaheadDuration = 1.0f;
    public List<Anchor> trackAnchors;

    public Anchor getClosestAnchor(Car car)
    {
        if (trackAnchors == null || trackAnchors.Count == 0)
        {
            Debug.LogError("No anchors, attempted to get closest anchor");
            return null;
        }


        Vector3 velocity = car.getVelocity();

        Vector3 position = car.transform.position + (velocity * velocityLookaheadDuration);

        float closestDistance = float.MaxValue;
        Anchor closestAnchor = null;
        foreach (Anchor anchor in trackAnchors)
        {
            anchor.highlighted = false;
            float distance = Vector3.Distance(position, anchor.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestAnchor = anchor;
            }

        }
        closestAnchor.highlighted = true;
        return closestAnchor;
    }
}
