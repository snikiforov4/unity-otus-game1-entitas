using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _mainCameraTransform;

    void Start()
    {
        _mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(_mainCameraTransform);
    }
}
