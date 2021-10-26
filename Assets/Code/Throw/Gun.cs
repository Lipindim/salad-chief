using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class Gun : MonoBehaviour
{
    [SerializeField] private Rigidbody[] _bullets;
    [SerializeField] private float _shotForce = 1.0f;
    [SerializeField] private int _reloadTimeInMiliseconds = 200;
    [SerializeField] private float _upForceMultiplier = 2.5f;
    [SerializeField] private Camera _aimCamera;
    [SerializeField] private float _cameraMultiplier = 1.5f;

    private int _currentIndex = 0;

    private void Start()
    {
        _bullets.First().gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) || _currentIndex >= _bullets.Length)
            return;

        var relativeMousePosition = new Vector3(Input.mousePosition.x + (Input.mousePosition.x - Screen.width / 2) * _cameraMultiplier, Input.mousePosition.y, 0);
        var ray = _aimCamera.ScreenPointToRay(relativeMousePosition);
        _bullets[_currentIndex].AddForce((ray.direction + _aimCamera.transform.up * _upForceMultiplier) * _shotForce);
        _bullets[_currentIndex].useGravity = true;
        _currentIndex++;
        Reload();
    }

    private async void Reload()
    {
        await Task.Delay(_reloadTimeInMiliseconds);
        if (_currentIndex == _bullets.Length)
            return;

        _bullets[_currentIndex].gameObject.SetActive(true);
    }
}
