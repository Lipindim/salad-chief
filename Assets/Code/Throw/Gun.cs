using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class Gun : MonoBehaviour
{
    [SerializeField] private Rigidbody[] _bullets;
    [SerializeField] private float _shotForce = 1.0f;
    [SerializeField] private int _reloadTimeInMiliseconds = 200;

    private int _currentIndex = 0;

    private void Start()
    {
        _bullets.First().gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) || _currentIndex >= _bullets.Length)
            return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        _bullets[_currentIndex].AddForce((ray.direction + Camera.main.transform.up * 2.5f) * _shotForce);
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
