using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DifferenceTextController : MonoBehaviour
{
    // Данный скрипт контролирует текст, отображающий изменения в Total Damage
    [SerializeField] private float _speedOfMoving;
    [SerializeField] private float _speedOfDisappearing;
    [SerializeField] private float _waitBeforeDisappear;
    public string TextToShow;
    private Text _text;
    public Text Text => _text;

    private void Start()
    {
        _text = transform.GetChild(0).gameObject.GetComponent<Text>();
        _text.text = TextToShow;
        StartCoroutine(CoolDown());
    }
    
    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(_waitBeforeDisappear);
        Color _color = _text.color;
        Vector3 _localScale = transform.localScale;
        while (_color.a > 0)
        {
            yield return new WaitForEndOfFrame();
            _localScale = new Vector3(_localScale.x, _localScale.y + _speedOfMoving * Time.deltaTime, _localScale.z);
            _color = new Color(_color.r, _color.g, _color.b, _color.a - _speedOfDisappearing * Time.deltaTime);
            transform.localScale = _localScale;
            _text.color = _color;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
