using System;
using UnityEngine;

[Serializable]
public class SpeedInterval
{
    public Vector3 Position => _position;
    public float Speed => _speed;

    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _position;
}
