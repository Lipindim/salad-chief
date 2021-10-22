using System;
using UnityEngine;

[Serializable]
public class SpeedInterval
{
    public float Time => _time;
    public float Speed => _speed;

    [SerializeField] private float _speed;
    [SerializeField] private float _time;
}
