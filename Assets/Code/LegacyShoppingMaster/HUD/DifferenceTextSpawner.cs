using UnityEngine;
using UnityEngine.UI;

public class DifferenceTextSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab; //������ ����������� ������.
    [SerializeField] private ProductDestroy _productDestroy;
    private Transform _canvas;
    private GameObject _differenceText;
    private float _priceBoofer;
    private void Start()
    {
        _canvas = GameObject.Find("Canvas").transform;
        _productDestroy.ProductDestroyedEvent += Spawn;
    }
    private void Spawn(float _howMuchChanged)
    {
        if (_differenceText == null || _differenceText.GetComponent<DifferenceTextController>().Text.color.a < 0.75f)
        {
            _priceBoofer = _howMuchChanged;
            _differenceText = Instantiate(_prefab);
            _differenceText.GetComponent<DifferenceTextController>().TextToShow = "+" + _howMuchChanged + "$";
            _differenceText.transform.SetParent(_canvas);
            _differenceText.transform.localPosition = new Vector3(-6.5f, -18, 0); //������-�� ����� ����� ������������ �� Canvas, �� � ���� ������ �������� � ������������, ������� � �� �����������. 
        }
        else
        {
            _priceBoofer += _howMuchChanged;
            _differenceText.GetComponent<DifferenceTextController>().Text.text = "+" + _priceBoofer + "$";
        }
    }
}