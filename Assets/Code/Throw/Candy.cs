using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;


public class Candy : MonoBehaviour
{
    //[SerializeField] private MainController _mainController;
    [SerializeField] private Transform _throwPoint;
    [SerializeField] private Transform _dropPoint;
    
    [SerializeField] private Vector3 minThrow;
    [SerializeField] private Vector3 maxThrow;

    public int team = 1;
    
    private Rigidbody _rigidbody;
    private MeshRenderer _renderer;
    private Collider _collider;

    public bool _isActive = false;
    private bool _isThrowed = false;
    private IEnumerator _jumpCoroutine;

    private Plane _plane;
    private Vector3 _v3Offset;
    private bool _objectMouseDown;
    private List<Vector3> _ballPos = new List<Vector3> ();
    private List<float> _ballTime = new List<float> ();
    [SerializeField] private float _factor = 230.0f;

    private void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _renderer = gameObject.GetComponent<MeshRenderer>();
        _collider = gameObject.GetComponent<Collider>();
        SetActive(_isActive);
    }

    public void SetActive(bool active)
    {
        _isActive = active;
        if (active)
        {
            gameObject.transform.position = _throwPoint.position;
            StartJump();
            //_collider.enabled = false;
            _rigidbody.useGravity = false;
        }
        else
        {
            gameObject.transform.position = _dropPoint.position;
            StopJump();
            //_collider.enabled = true;
            _rigidbody.useGravity = true;
        }
    }

    private void StopJump()
    {
        if (_jumpCoroutine == null) return;
        StopCoroutine(_jumpCoroutine);
        _jumpCoroutine = null;
    }

    private void StartJump()
    {
        _jumpCoroutine = GraduallyJumpCoroutine(0.3f);
        StartCoroutine(_jumpCoroutine);
    }

    private void SetThrowed(bool isThrowed)
    {
        _isThrowed = isThrowed;
        if (isThrowed)
        {
            StopJump();
        }
    }

    private void ResetBall()
    {
        _isThrowed = false;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        StopAllCoroutines();

        //ball move to initial position
        StartCoroutine(MoveBackToInitialPositionCoroutine(0.3f));
    }

    private void MissedThrow()
    {
        ResetBall();
        //_mainController.OnCandyMissedHit(this);
    }
    
    private void OnMouseDown ()
    {
        if (!_isActive)
            return;
        _plane.SetNormalAndPosition(Camera.main.transform.forward, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dist;
        _plane.Raycast(ray, out dist);
        _v3Offset = transform.position - ray.GetPoint(dist);     
        _objectMouseDown = true;

        _ballPos.Clear();
        _ballTime.Clear();
        _ballTime.Add(Time.time);
        _ballPos.Add(transform.position);
		
        SetThrowed(true);
    }
    
    private void OnMouseDrag()
    {
        if (!_objectMouseDown) return;
        if (!_isActive) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        _plane.Raycast(ray, out var dist);
        Vector3 v3Pos = ray.GetPoint(dist);
        v3Pos.z = transform.position.z;
        _v3Offset.z = 0;
        transform.position = v3Pos + _v3Offset;

        if (_ballPos.Count > 0) {
            if (_ballPos.Count <= 4)
            {
                if (!(Vector3.Distance(transform.position, _ballPos[_ballPos.Count - 1]) >= 0.01f)) return;
                _ballTime.Add(Time.time);
                _ballPos.Add(transform.position);
            } else
            {
                if (!(Vector3.Distance(transform.position, _ballPos[_ballPos.Count - 1]) >= 0.01f)) return;
                _ballTime.RemoveAt(0);
                _ballPos.RemoveAt(0);
                _ballTime.Add(Time.time);
                _ballPos.Add(transform.position);
            }
				
        } else {
            _ballPos.Add(transform.position);
        }
    }
    
    private void OnMouseUp()
    {
        if (!_objectMouseDown) return;
        if (!_isActive) return;

        var ballPositionIndex = _ballPos.Count - 2;

        if (ballPositionIndex < 0) ballPositionIndex = 0;

        Vector3 force = transform.position - _ballPos[ballPositionIndex];
		
        //if downside
        if (transform.position.y <= _ballPos[ballPositionIndex].y) {
            ResetBall();
            return;
        }

        //if not swipe
        if (force.magnitude < 0.02f) {
            ResetBall();
            return;
        }

        force.z = force.magnitude;
        force /= (Time.time - _ballTime[ballPositionIndex]);
        force.y /= 2f;
        force.x /= 2f;
	
        force.x = Mathf.Clamp(force.x, minThrow.x, maxThrow.x);
        force.y = Mathf.Clamp(force.y, minThrow.y, maxThrow.y);
        force.z = Mathf.Clamp(force.z, minThrow.z, maxThrow.z);
        force = force.normalized;
        force = force.normalized + transform.up;

        SetThrowed(true);
		_rigidbody.useGravity = true;
        _rigidbody.AddForce(force * _factor);

        Invoke(nameof(MissedThrow), 1.5f);
    }
    
    IEnumerator GraduallyJumpCoroutine(float tm)
    {
        while (!_isThrowed) {
            yield return new WaitForSeconds(0.5f);
		
            transform.position = _throwPoint.position;

            float i = 0f;
            float rate = 1.0f / tm;
            Vector3 from = _throwPoint.position;
            Vector3 to = new Vector3(from.x, from.y + 0.3f, from.z);

            while (i<1.0f) {
                i += rate * Time.deltaTime;
                transform.position = Vector3.Lerp(from, to, i);
                yield return 0f;
            }

            i = 0f;
            rate = 1.0f / (tm / 0.7f);

            Vector3 bump = from;
            bump.y -= 0.05f;

            while (i<1.0f) {
                i += rate * Time.deltaTime;
                transform.position = Vector3.Lerp(to, bump, i);
                yield return 0f;
            }

            i = 0f;
            rate = 1.0f / (tm / 1.1f);

            while (i<1.0f) {
                i += rate * Time.deltaTime;
                transform.position = Vector3.Lerp(bump, from, i);
                yield return 0f;
            }
        }
    }
    
    IEnumerator MoveBackToInitialPositionCoroutine(float tm)
    {
        _collider.enabled = false;
        var i = 0f;
        var rate = 1.0f / tm;
        var from = transform.position;
        var to = _throwPoint.position;
        while (i<1.0f) {
            i += rate * Time.deltaTime;
            transform.position = Vector3.Lerp(from, to, i);
            yield return 0f;
        }

        transform.position = _throwPoint.position;
        transform.localRotation = Quaternion.identity;
        StartJump();
        _collider.enabled = true;
    }
}
