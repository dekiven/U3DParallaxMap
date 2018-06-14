using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Datas/EnviromentData")]
[Serializable]
public class EnviromentData : BaseConfig
{
    [SerializeField]
    public float time;
}