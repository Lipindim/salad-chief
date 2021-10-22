using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class NewBall : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler
{
    [SerializeField] private int _forcePower = 100000;

    public event Action Throwed;

    private float _xOffset;
    private float _yOffset;
    private Vector3[] _targets = new Vector3[0];
    private Vector3 _target;
    private Rigidbody _rigidBody;
    private Vector3 _dragBeginPosition;

    private float _leftAndRightTargetAngle;
    private Vector3 _startPosition;
    private Vector3 _previousPosition;

    public void SetTarget(Vector3[] targets, float leftAndRightTargetAngle)
    {
        _targets = targets;
        _leftAndRightTargetAngle = leftAndRightTargetAngle;
    }

    public void SetOffsets(float xOffset, float yOffset)
    {
        _xOffset = xOffset;
        _yOffset = yOffset;
    }

    private bool _throwed;

    private void Start()
    {
        _startPosition = transform.position;
        _rigidBody = GetComponent<Rigidbody>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragBeginPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_throwed)
            return;

        transform.position = GetNewPosition(eventData.position);
        _previousPosition = transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_throwed)
            return;

        _target = GetTarget();

        var newPosition  = GetNewPosition(eventData.position);
        var direction = (_target - newPosition).normalized + transform.up * 2;
        var forcePower = (newPosition - _previousPosition).sqrMagnitude;
        if (forcePower < 0.0015)
            return;
        if (forcePower > 0.002f)
            forcePower = 0.002f;
        Debug.Log(forcePower);
        _rigidBody.AddForce(direction * _forcePower * forcePower);
        _rigidBody.useGravity = true;

        _throwed = true;
        Throwed?.Invoke();
    }

    private Vector3 GetNewPosition(Vector2 screenPosition)
    {
        var relativeXPosition = screenPosition.x - Screen.width / 2;
        var relativeXOffset = relativeXPosition / (Screen.width / 2);
        var relativeYPosition = screenPosition.y - Screen.height / 2;
        var relativeYOffset = relativeYPosition / (Screen.height / 2);
        var position = new Vector3(_startPosition.x + relativeXOffset * _xOffset,
            _startPosition.y + relativeYOffset * _yOffset,
            _startPosition.z);
        return position;
    }

    private Vector3 GetTarget()
    {
        float a = Mathf.Abs(transform.position.x -_dragBeginPosition.x);
        float b = Mathf.Abs(transform.position.y - _dragBeginPosition.y);
        float c = Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
        float sin = a / c;
        float angle = Mathf.Asin(sin) * 60;
        
        if (90 - angle <= _leftAndRightTargetAngle)
        {
            if (transform.position.x < _dragBeginPosition.x)
            {
                Debug.Log("Left target");
                return _targets[0];
            }
            else
            {
                Debug.Log("Right target");
                return _targets[2];
            }
        }
        else
        {
            Debug.Log("Middle target");
            return _targets[1];
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}

