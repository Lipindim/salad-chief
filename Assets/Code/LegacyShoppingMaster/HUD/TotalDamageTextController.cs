using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TotalDamageTextController : MonoBehaviour
{
    [SerializeField] private float _changingSpeed; // Сколько секунд нужно, что-бы перевести всю сумму в TotalDamage
    [SerializeField] private float _waitBeforeChanging;
    [SerializeField] private ProductDestroy _productDestroy;
    private Text _text;
    private void Start()
    {
        _text = GetComponent<Text>();
        _text.text = "Total damage: \n" + GlobalVars.TotalDamage +"$";
        _productDestroy.ProductDestroyedEvent += Change;
    }
    public void Change(float _howMuchToChange)
    {
        StartCoroutine(Changing(_howMuchToChange));
    }
    IEnumerator Changing(float _changeAmount)
    {
        float _fullChangeAmount = _changeAmount;
        yield return new WaitForSeconds(_waitBeforeChanging);
        while (_changeAmount > 0)
        {
            yield return new WaitForEndOfFrame();
            //Без такой проверки иногда получалось, что игрок получал немного больше урона, чем наносил и в Total Damage выходили не круглые числа
            if (_changeAmount > _fullChangeAmount / _changingSpeed * Time.deltaTime) { 
                GlobalVars.TotalDamage += _fullChangeAmount / _changingSpeed * Time.deltaTime;
                _changeAmount -= _fullChangeAmount / _changingSpeed * Time.deltaTime;
            }
            else
            {
                GlobalVars.TotalDamage += _changeAmount;
                _changeAmount -= _changeAmount;
            }
            _text.text = "Total damage: \n" + Mathf.Round(GlobalVars.TotalDamage) + "$";
            yield return null;
        }
    }
}