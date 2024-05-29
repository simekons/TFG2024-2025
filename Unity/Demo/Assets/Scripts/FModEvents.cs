using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FModEvents : MonoBehaviour
{
    [field: Header("Red")]
    [field: SerializeField] public EventReference red { get; private set; }

    [field: Header("Green")]
    [field: SerializeField] public EventReference green { get; private set; }
    
    [field: Header("Blue")]
    [field: SerializeField] public EventReference blue { get; private set; }

    [field: Header("Yellow")]
    [field: SerializeField] public EventReference yellow { get; private set; }

    [field: Header("Pink")]
    [field: SerializeField] public EventReference pink { get; private set; }


    public static FModEvents instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one FMod Events instance in the scene");
        }

        instance = this;
    }
}
