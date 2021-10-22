using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private Vector3 _direction;
    private CharacterController _controller;
    [SerializeField] private float _speed;

    private void Start()
    {
        _direction.z = _speed;
        _controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        _controller.Move(_direction * Time.deltaTime);
    }
}
