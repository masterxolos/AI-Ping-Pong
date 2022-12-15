using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody _rb;
    [SerializeField] private float yMargin = 3.10f;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touches.Length > 0)
        {
            Vector3 touchPosition = Input.touches[0].position;
            touchPosition = mainCamera.ScreenToWorldPoint(touchPosition);
            Debug.Log(touchPosition);
            touchPosition.y = Mathf.Clamp(touchPosition.y, -yMargin, yMargin);
            
            touchPosition.x = transform.position.x;
            touchPosition.z = transform.position.z;
            transform.localPosition = touchPosition;
        }
    }
}
