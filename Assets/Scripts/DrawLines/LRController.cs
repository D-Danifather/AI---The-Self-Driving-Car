using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRController : MonoBehaviour
{

    [SerializeField] private LineRenderer _lr;
    
    private Transform[] _paths;

    private int pathLength;

    // Start is called before the first frame update
    public void SetUpLinesClassic(Transform[] _paths)
    {
        pathLength = _paths.Length;
        _lr.positionCount = pathLength;
        this._paths = _paths;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < pathLength; i++)
        _lr.SetPosition(i, _paths[i].position);
    }
}
