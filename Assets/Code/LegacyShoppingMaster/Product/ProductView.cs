using System;
using UnityEngine;


public class ProductView : MonoBehaviour
{

    // ToDo: Вызывать при тапе на товар.
    public event Action<ProductView> SelectedForPurchase;


    [SerializeField] private int _id;


    public int Id => _id;

}

