using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    private Camera _camera;
    // Use this for initialization
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_camera.transform);
    }
}
