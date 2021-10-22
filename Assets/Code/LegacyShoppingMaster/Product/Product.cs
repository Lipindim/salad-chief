using UnityEngine;


[CreateAssetMenu(menuName = "Data/Product")]
public class Product : ScriptableObject
{

    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private float _cost;


    public int Id => _id;
    public string Name => _name;
    public float Cost => _cost;

}

