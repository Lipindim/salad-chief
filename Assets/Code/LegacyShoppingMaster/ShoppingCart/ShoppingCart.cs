using System.Collections.Generic;


public class ShoppingCart
{

    private readonly ProductStore _productStore;
    private Dictionary<int, int> _productsCount = new Dictionary<int, int>();


    public ShoppingCart(ProductStore productStore)
    {
        _productStore = productStore;
    }


    public void AddProduct(int productId)
    {
        if (_productsCount.ContainsKey(productId))
            _productsCount[productId]++;
        else
            _productsCount[productId] = 1;
    }

    public float GetCost()
    {
        float cost = 0;
        foreach (var productCount in _productsCount)
        {
            var product = _productStore.GetProduct(productCount.Key);
            cost += product.Cost * productCount.Value;
        }
        return cost;
    }

    public void Clear()
    {
        _productsCount.Clear();
    }

}
