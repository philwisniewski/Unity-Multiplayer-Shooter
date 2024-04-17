using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialHolder : MonoBehaviour
{
    public static MaterialHolder Instance = null;
    
    public Material[] materials;
    public String[] materialNames;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }
}
