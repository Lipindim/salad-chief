using UnityEngine;


public class GameInitializer : MonoBehaviour
{

    [SerializeField] private Product[] _products;


    private void Start()
    {
        var productStore = new ProductStore();
        productStore.Initialize(_products);
        var shoppingCart = new ShoppingCart(productStore);
        var productViews = FindObjectsOfType<ProductView>();
        var productToShoppingCartAdder = new ProductToShopingCartAdder(productViews, shoppingCart);
    }

}

