using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private Color lineColor;

    [SerializeField] private List<Transform> nodes = new List<Transform>();

    private Transform[] pathTransform;

    private void Start()
    {
        pathTransform = GetComponentsInChildren<Transform>();

        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != transform)
                nodes.Add(pathTransform[i]);
        }
    }

    // On draw gizmos is accessing to the program each frame like update methode
    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 currNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;

            if (i > 0)
                previousNode = nodes[i - 1].position;
            else if (i == 0 && nodes.Count > 1)
                previousNode = nodes[nodes.Count - 1].position;

            Gizmos.DrawLine(previousNode, currNode);

            Gizmos.DrawWireSphere(currNode, 0.5f);
        }
    }
}
