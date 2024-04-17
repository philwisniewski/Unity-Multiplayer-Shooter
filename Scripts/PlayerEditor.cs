using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerEditor : MonoBehaviour
{
    // head, chest, arms, waist, legs
    Dictionary<string, int> nameToVal = new Dictionary<string, int>();

    [SerializeField] TMP_Text headText;
    [SerializeField] TMP_Text chestText;
    [SerializeField] TMP_Text armsText;
    [SerializeField] TMP_Text waistText;
    [SerializeField] TMP_Text legsText;

    private void Update() {
        headText.text = "Head Color: " + MaterialHolder.Instance.materialNames[nameToVal["head"]];
        chestText.text = "Chest Color: " + MaterialHolder.Instance.materialNames[nameToVal["chest"]];
        armsText.text = "Arms Color: " + MaterialHolder.Instance.materialNames[nameToVal["arms"]];
        waistText.text = "Waist Color: " + MaterialHolder.Instance.materialNames[nameToVal["waist"]];
        legsText.text = "Legs Color: " + MaterialHolder.Instance.materialNames[nameToVal["legs"]];
    }

    private void Awake() {
        nameToVal.Add("head", 4);
        nameToVal.Add("chest", 4);
        nameToVal.Add("arms", 4);
        nameToVal.Add("waist", 4);
        nameToVal.Add("legs", 4);

        Hashtable hash = new Hashtable();
        hash.Add("head", nameToVal["head"]);
        hash.Add("chest", nameToVal["chest"]);
        hash.Add("arms", nameToVal["arms"]);
        hash.Add("waist", nameToVal["waist"]);
        hash.Add("legs", nameToVal["legs"]);

        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }


    public void IncrementColor(string bodyPart) {
        if (nameToVal[bodyPart] >= MaterialHolder.Instance.materials.Length - 1) {
            nameToVal[bodyPart] = 0;
        } else {
            nameToVal[bodyPart]++;
        }
        
        Hashtable hash = new Hashtable();
        hash.Add(bodyPart, nameToVal[bodyPart]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void DecrementColor(string bodyPart) {
        if (nameToVal[bodyPart] <= 0) {
            nameToVal[bodyPart] = MaterialHolder.Instance.materials.Length - 1;
        } else {
            nameToVal[bodyPart]--;
        }
        
        Hashtable hash = new Hashtable();
        hash.Add(bodyPart, nameToVal[bodyPart]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        
    }
}
