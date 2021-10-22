using UnityEngine;

public class ProductDestroy : MonoBehaviour
{
    [SerializeField] public InitializeProductStore _initializeProductStore;
    [SerializeField] private bool _turnProductsPhysicsOff; 
    public delegate void ProductDestroyed(float _cost);
    public event ProductDestroyed ProductDestroyedEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Product")
        {
            //Выключение физики упавших объектов. Поидее должно помочь в оптимизации.
            if (_turnProductsPhysicsOff) {
                GameObject _collisionGO = other.gameObject;
                _collisionGO.GetComponent<Rigidbody>().isKinematic = true;
                _collisionGO.GetComponent<BoxCollider>().enabled = false;
            }
            Product _product = _initializeProductStore.ProductStore.GetProduct(other.GetComponent<ProductView>().Id);
            ProductDestroyedEvent?.Invoke(_product.Cost);
        }
    }
}
