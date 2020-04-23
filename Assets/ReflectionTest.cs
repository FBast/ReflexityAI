using System;
using System.Reflection;
using UnityEngine;

public class ReflectionTest : MonoBehaviour {

    public ReflectedData FromData;
    public ReflectedData ToData;
    
    private void Start() {
        FieldInfo fieldInfo = typeof(ReflectionTest).GetField(nameof(FromData));
        ToData = (ReflectedData) fieldInfo.GetValue(this);
        InvokeRepeating(nameof(Increase), 0, 1);
    }
    
    public void Increase() {
        FromData.Value++;
    }

}

[Serializable]
public class ReflectedData {

    public int Value;

}

