using UnityEngine;
using TMPro;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings"), Space(5)]
    [SerializeField] float fireRate = 1.5f;
    [SerializeField] int maxAmmoCapacity = 20;
    [SerializeField] int magazine = 500;

    [Header("References"), Space(5)]
    [SerializeField] TextMeshProUGUI ammoText, magazineText;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform posBulletSpawn;
    [SerializeField] Transform cam;
    [SerializeField] Transform defaultWeaponPos, aimingPos;
    [SerializeField] AudioClip gunShotSound, reloadSound;

    Animator anim;

    AudioSource audioSource;

    float nextTimeToFire = 0f;

    int currentAmmo;

    bool isReloading = false;

    bool isAiming = false;

    bool allowedToReload = true;


    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        anim.enabled = false;

        currentAmmo = maxAmmoCapacity;

        ammoText.SetText(currentAmmo.ToString());
        magazineText.SetText(magazine.ToString());
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire && currentAmmo > 0 && !isReloading)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && magazine > 0 && currentAmmo < maxAmmoCapacity && !isReloading && allowedToReload)
        {

            StartCoroutine(Reloading());
        }

        if (Input.GetMouseButtonDown(1) && !isReloading)
        {
            if (isAiming)
            {
                gameObject.transform.position = defaultWeaponPos.position;
                isAiming = false;
                allowedToReload = true;
            }
            else
            {
                gameObject.transform.position = aimingPos.position;
                isAiming = true;
                allowedToReload = false;
            }
        }
    }

    void Shoot()
    {
        Instantiate(bullet, posBulletSpawn.position, Quaternion.LookRotation(posBulletSpawn.forward));
        audioSource.PlayOneShot(gunShotSound);
        currentAmmo--;
        ammoText.SetText(currentAmmo.ToString());
    }

    IEnumerator Reloading()
    {
        isReloading = true;
        anim.enabled = true;
        anim.Play("Reload");
        audioSource.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(2f);
        if(maxAmmoCapacity - currentAmmo <= magazine)
        {
            magazine -= (maxAmmoCapacity - currentAmmo);
            currentAmmo = maxAmmoCapacity;
            ammoText.SetText(currentAmmo.ToString());
            magazineText.SetText(magazine.ToString());
        }
        else
        {
            currentAmmo += magazine;
            magazine = 0;
            magazineText.SetText(magazine.ToString());
        }
        anim.Play("ReloadBack");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.Play("Default");
        anim.enabled = false;
        isReloading = false;
    }
}
