using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeProductStore : MonoBehaviour
{

    [SerializeField] private Product[] _products;
    private ProductStore _productStore = new ProductStore();
    public ProductStore ProductStore => _productStore;
    private void Start()
    {
        ProductStore.Initialize(_products);
    }
}
