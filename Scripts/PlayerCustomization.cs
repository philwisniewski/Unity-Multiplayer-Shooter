using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;

public class PlayerCustomization : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    [SerializeField] MeshRenderer playerHead;
    [SerializeField] MeshRenderer playerChest;
    [SerializeField] MeshRenderer playerRightArm;
    [SerializeField] MeshRenderer playerLeftArm;
    [SerializeField] MeshRenderer playerRightHand;
    [SerializeField] MeshRenderer playerLeftHand;
    [SerializeField] MeshRenderer playerWaist;
    [SerializeField] MeshRenderer playerCrotch;
    [SerializeField] MeshRenderer playerLeftLeg;
    [SerializeField] MeshRenderer playerRightLeg;
    
    private void Awake() {
        PV = GetComponent<PhotonView>();
    }

    void Start() {

        playerHead.material = MaterialHolder.Instance.materials[(int) PV.Owner.CustomProperties["head"]];
        playerChest.material = MaterialHolder.Instance.materials[(int) PV.Owner.CustomProperties["chest"]];
        playerRightArm.material = MaterialHolder.Instance.materials[(int) PV.Owner.CustomProperties["arms"]];
        playerLeftArm.material = MaterialHolder.Instance.materials[(int) PV.Owner.CustomProperties["arms"]]; 
        playerRightHand.material = MaterialHolder.Instance.materials[(int) PV.Owner.CustomProperties["arms"]];
        playerLeftHand.material = MaterialHolder.Instance.materials[(int) PV.Owner.CustomProperties["arms"]]; 
        playerWaist.material = MaterialHolder.Instance.materials[(int) PV.Owner.CustomProperties["waist"]];
        playerCrotch.material = MaterialHolder.Instance.materials[(int) PV.Owner.CustomProperties["waist"]];
        playerLeftLeg.material = MaterialHolder.Instance.materials[(int) PV.Owner.CustomProperties["legs"]];
        playerRightLeg.material = MaterialHolder.Instance.materials[(int) PV.Owner.CustomProperties["legs"]];
        
    }

    
}
