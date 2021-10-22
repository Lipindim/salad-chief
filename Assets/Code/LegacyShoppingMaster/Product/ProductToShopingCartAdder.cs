using System.Collections.Generic;


public class ProductToShopingCartAdder
{

    private readonly ShoppingCart _shoppingCart;


    public ProductToShopingCartAdder(IEnumerable<ProductView> productViews, ShoppingCart shoppingCart)
    {
        _shoppingCart = shoppingCart;
        foreach (var productView in productViews)
            productView.SelectedForPurchase += OnSelectedForPurchase;
    }

    private void OnSelectedForPurchase(ProductView productView)
    {
        _shoppingCart.AddProduct(productView.Id);
    }

}

