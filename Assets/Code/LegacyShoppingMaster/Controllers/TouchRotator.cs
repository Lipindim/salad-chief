using UnityEngine;


public class TouchRotator : MonoBehaviour
{
    [SerializeField] private Transform _rotateObject;
    [SerializeField] private float _maxVerticalAngle = 25.0f;
    [SerializeField] private float _maxHorizontalAngle = 17.5f;

    private Quaternion _startRotation;

    private void Start()
    {
        _startRotation = _rotateObject.rotation;
    }

    private void Update()
    {
        #if UNITY_EDITOR
            ProcessingMouseClick();
        #else
            ProcessingTouch();
        #endif
    }

    private void ProcessingMouseClick()
    {
        if (Input.GetMouseButton(0))
            RotateObject(Input.mousePosition.x, Input.mousePosition.y);
    }

    private void ProcessingTouch()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            RotateObject(touch.position.x, touch.position.y);
        }    
    }

    private void RotateObject(float xTouchPosition, float yTouchPosition)
    {
        float horizontalAngle = (xTouchPosition / Screen.width - 0.5f) * _maxHorizontalAngle * 2;
        if (Mathf.Abs(_maxHorizontalAngle) < Mathf.Abs(horizontalAngle))
            horizontalAngle = Mathf.Sign(horizontalAngle) * _maxHorizontalAngle;
        
        float verticalAngle = (yTouchPosition / Screen.height - 0.5f) * _maxVerticalAngle * 2;
        if (Mathf.Abs(_maxVerticalAngle) < Mathf.Abs(verticalAngle))
            verticalAngle = Mathf.Sign(verticalAngle) * _maxVerticalAngle;
        
        transform.rotation = _startRotation * Quaternion.Euler(verticalAngle, horizontalAngle, 0);
    }
}

