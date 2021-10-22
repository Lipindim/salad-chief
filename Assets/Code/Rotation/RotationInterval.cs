using System;
using UnityEngine;


[Serializable]
public class RotationInterval
{

    public Vector3 Angle => _angle;
    public float TimeSeconds => _timeSeconds;
    public float Speed => _speed;

    [SerializeField] private Vector3 _angle;
    [SerializeField] private float _timeSeconds;
    [SerializeField] private float _speed;

}

