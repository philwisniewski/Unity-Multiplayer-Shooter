using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor;
using TMPro;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;
    [SerializeField] TMP_Text ammoText;

    PhotonView PV;

    GunInfo gunInfo;

    int bulletsLeft;
    int bulletsShot;
    bool shooting, readyToShoot, reloading;


    public GameObject muzzleFlash;
    public Transform gunTip;


    void Awake() {
        PV = GetComponent<PhotonView>();
        gunInfo = (GunInfo) itemInfo;
        bulletsLeft = gunInfo.magazineSize;
        readyToShoot = true;
        cam = transform.parent.parent.GetChild(0).GetComponent<Camera>();
    }

    private void Update() {
        
        if (transform.childCount > 0 && transform.GetChild(0).gameObject.activeInHierarchy) {
            MyInput();
            ammoText.text = bulletsLeft + " / " + gunInfo.magazineSize;
        }
        
    }

    public override void Use()
    {
        Shoot();
    }

    private void Shoot() {
        if (cam == null)
        {
            return;
        }

        if (bulletsLeft <= 0) {
            return;
        }

        readyToShoot = false;

        float x = Random.Range(- gunInfo.spread, gunInfo.spread);
        float y = Random.Range(- gunInfo.spread, gunInfo.spread);

        


        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)+ new Vector3(x, y, 0));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            hit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(((GunInfo) itemInfo).damage);
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);


        }

        
        if (bulletsShot <= 0)
        {
            bulletsShot = gunInfo.bulletsPerTap;
        }
        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", gunInfo.timeBetweenShooting);

        print("Bullets shot: " + bulletsShot + ", Bullets left: " + bulletsLeft);
        if (bulletsShot > 0 && bulletsLeft > 0) {
            Invoke("Shoot", gunInfo.timeBetweenShots);
        }
        


    }

    private void ResetShot() {
        readyToShoot = true;
    }


    private void MyInput() {
        if (gunInfo.allowButtonHold) {
            shooting = Input.GetKey(KeyCode.Mouse0);
        } else {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
            if (shooting)
            {
                print("Shooting!");
            }
        }

        

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < gunInfo.magazineSize && !reloading) {
            Reload();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0) {
            bulletsShot = gunInfo.bulletsPerTap;
            Use();
        }



        
    }

    private void Reload() {
        reloading = true;
        Invoke("ReloadFinished", gunInfo.reloadTime);
    }

    private void ReloadFinished() {
        bulletsLeft = gunInfo.magazineSize;
        reloading = false;
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal) {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if (colliders.Length != 0) {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 10f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }
}
