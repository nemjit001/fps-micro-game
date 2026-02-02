using System.Collections.Generic;
using UnityEngine;

public class OverlayCameraManager : MonoBehaviour
{
    [SerializeField]
    List<Camera> _overlayCameras = new List<Camera>();

    Camera _parentCamera = null;

    void Start()
    {
        _parentCamera = GetComponent<Camera>();
    }

    void Update()
    {
        // Match all overlay camera FOVs
        foreach (var camera in _overlayCameras)
        {
            camera.fieldOfView = _parentCamera.fieldOfView;
        }
    }
}
