using System;
using System.Collections.Generic;


public class ProductStore
{

    private Dictionary<int, Product> _products = new Dictionary<int, Product>();


    public Product GetProduct(int productId)
    {
        if (!_products.ContainsKey(productId))
            throw new ArgumentException($"Can't find product with id = {productId}");
        return _products[productId];
    }

    public void Initialize(IEnumerable<Product> products)
    {
        foreach (var product in products)
            _products.Add(product.Id, product);
    }

}

