using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLines : MonoBehaviour
{
    [SerializeField] private LRController LRController;
    [SerializeField] private Transform[] _points;

    // Start is called before the first frame update
    void Start()
        => LRController.SetUpLinesClassic(_points);
}
