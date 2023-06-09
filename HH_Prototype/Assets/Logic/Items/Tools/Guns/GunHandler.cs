using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour, IInteractable, ICraftable, IEquippable
{
    [Header("Gun Information")]
    [SerializeField] GunData gunData;
    [SerializeField] GameObject gfx;
    [SerializeField] Transform muzzle;

    [Header("Bullet Trail Information")]
    [SerializeField] LineRenderer bulletTrail;
    [SerializeField] float trailFadeDuration;

    [Header("SFX")]
    [SerializeField] int sfxIndex;


    //data for which shoot interaction is being used
    public delegate void ShootInteraction();
    private ShootInteraction shootInteraction;

    bool equipped = false;

    float mouse1Current = 0, mouse2Current = 0;
    bool fired = false;
    float currentTime = 0;

    [Header(" ")]
    [HideInInspector]
    public Vector3 target = Vector3.zero;
    [SerializeField] bool debug = true;

    void Start()
    {
        SetUpGun();
    }

    private void Update()
    {
        //iterates timer up
        currentTime += Time.deltaTime;

        if(gunData != null)
        {
            transform.LookAt(target);

            //runs selected shooting alg
            if (equipped && !gunData.reloading && (shootInteraction != null))
            {
                shootInteraction();
            }

            

            //counts down timer if you are reloading
            if (gunData.reloading)
            {
                if (currentTime >= gunData.reloadTime)
                {
                    if (debug)
                    {
                        Debug.Log("recurse reload");
                    }

                    Reload();
                }
            }

            //live updates localpos
            if (debug)
            {
                transform.localPosition = gunData.gunOffset;
            }
        }
        else
        {
            Debug.Log("equip falsing");
            Equip(false);
        }
    }

    /*!
     * takes in mouse input and updates which mouse key is currently being pressed down
     */
    public void Interact(float mouse1, float mouse2)
    {
        mouse1Current = mouse1; mouse2Current = mouse2;
        UpdateUI();
    }
    public void Craft()
    {
        gunData.unlocked = true;
        SetParentAndResetTransform(PlayerController.instance.transform);
        SetUpGun();
        Equip(false);
    }
    public void Equip(bool equip)
    {
        Debug.Log(equip + " equipped?");
        equipped = equip;
        gfx.SetActive(equip);
    }

    /*!
     * sets up gun and all necessitated parts for the gun
     */
    public void SetUpGun()
    {
        //clears all funcs
        shootInteraction -= AutoShoot;
        shootInteraction -= SemiShoot;
        shootInteraction -= ChargeShoot;

        if (gunData != null)
        {
            transform.localPosition = gunData.gunOffset;

            //sets up selected shooting func based on fRate type
            switch (gunData.gunType)
            {
                case GunData.GunType.Auto:
                    shootInteraction += AutoShoot;
                    break;

                case GunData.GunType.Semi:
                    shootInteraction += SemiShoot;
                    break;

                case GunData.GunType.Charge:
                    shootInteraction += ChargeShoot;
                    break;
            }
        }

    }

    public void SetUpGunWithTransform(Transform tForm)
    {
        SetParentAndResetTransform(tForm);
        SetUpGun();
    }

    /*!
     * sets gundata which holds all info to how the gun will perform based off values found within gundata
     */
    public void SetGunData(GunData gData)
    {
        if(gData != null)
        {
            Debug.Log("setting gun data to: " + gData.name);
        }
        gunData = gData;
        SetUpGun();
    }

    /*! 
     * sets parent object of where gun will be, 
     */
    public void SetParentAndResetTransform(Transform parent)
    {
        transform.parent = parent;

        if(gunData != null)
            transform.localPosition = gunData.gunOffset;
    }

    /*!
     * checks if the gun is currently able to shoot
     */
    private bool CanShoot()
    {
        return !gunData.reloading && (currentTime > (1f / (FRateToSeconds()))); 
    }

    /*!
     * takes fire rate and returns its bullets per second
     */
    private float FRateToSeconds()
    {
        return gunData.fireRate / 60f;
    }

   

    public bool Equipped => equipped;

    public bool Reloading => gunData.reloading;

    public GunData GunData => gunData;

    public ShootInteraction ShootFunction => shootInteraction;
    /*!
     * reload interaction for both checking if you can reload, and are reloading
     */
    public void Reload()
    {
        if(!gunData)
        {
            Debug.Log("wa");
            return;
        }

        //if you already are reloading, have a full mag, or dont have enough ammo
        if((gunData.currentAmmo == gunData.magSize) || (ReadBulletAmountOfType() <= 0))
        {
            if (debug)
                Debug.Log("returning in relation to ammo amount parameter not being sufficient");

            Debug.Log(gunData.currentAmmo);
            Debug.Log(ReadBulletAmountOfType());
            gunData.reloading = false;
            return;
        }

        //starts reloading process, starts reload timer and calls OnReload for gfx
        if (gunData.reloading == false) 
        {
            if (debug)
            {
                Debug.Log("Start reload");
            }

            gunData.reloading = true;
            currentTime = 0;
            OnReload();
        }
        //finishes reloading process, and properly allocates ammo and inventory
        else
        {
            if (debug)
            {
                Debug.Log("finish reload");
            }
            Debug.Log("ja");
            currentTime = 0;
            gunData.reloading = false;

            gunData.currentAmmo += Inventory.instance.GrabAmountOfIndex(gunData.magSize - gunData.currentAmmo, 0);
        }

        UpdateUI();
    }

    /*!
     * checks which Type of damage weapon does and reads bullet amount of that type
     */
    private int ReadBulletAmountOfType()
    {
        return Inventory.instance.ReadAmountOfIndex(gunData.magSize, 0);
    }

    /*!
     * automatic shooting, shoots as long as mouse is held down at certain fire rate
     */
    void AutoShoot()
    {

        if ((gunData.currentAmmo > 0))
        {
            
            if ((mouse1Current > 0f) && CanShoot())
            {
                if (debug)
                {
                    Debug.Log("Fire Shot");
                }

                gunData.currentAmmo--;
                for (int i = 0; i < gunData.bulletsPerShot; i++)
                {
                    ShootBullet();
                }

                OnShot();
            }
        }
        UpdateUI();
    }

    /*!
     * shoots a shot every mouse click
     */
    void SemiShoot()
    {

        if ((gunData.currentAmmo > 0))
        {
            if(mouse1Current > 0f)
            {
                if (!fired)
                {
                    //fire shot
                    if (debug)
                    {
                        Debug.Log("firing shot");
                    }

                    gunData.currentAmmo--;
                    //shoot each bullet
                    fired = true;
                    for (int i = 0; i < gunData.bulletsPerShot; i++)
                    {
                        ShootBullet();
                    }
                        

                    currentTime = 0;
                    OnShot();
                }
            }
            //if mouse is released
            else
            {
                if (CanShoot())
                {
                    if (debug)
                    {
                        Debug.Log("Can Shoot Again");
                    }

                    currentTime = 0;
                    fired = false;
                }
            }
        }
        UpdateUI();
    }

    /*!
     * hold down shoot button for time of fire rate before releasing to complete shot
     */
    void ChargeShoot()
    {
        if (debug)
        {
            if (fired && (currentTime >= FRateToSeconds()))
            {
                Debug.Log("charged!");
            }
        }

        if ((gunData.currentAmmo) > 0)
        {
            //charge process is complete
            if ((mouse1Current <= 0) && (fired) && (currentTime >= FRateToSeconds()))
            {
                
                if (debug)
                {
                    Debug.Log("release and shoot charge");
                }

                gunData.currentAmmo--;
                //shoot each bullet
                for (int i = 0; i < gunData.bulletsPerShot; i++)
                {
                    ShootBullet();
                }
                    

                fired = false;
                OnShot();
                return;
            }
            //if mouse is released before charging is complete
            else if((mouse1Current <= 0) && fired)
            {
                if (debug)
                {
                    Debug.Log("release before full charge");
                }
                currentTime = 0;
                fired = false;
            }

            //begin charging process
            if (!fired && mouse1Current > 0) 
            {
                if (debug)
                {
                    Debug.Log("Charge");
                }
                fired = true;
                currentTime = 0;
            }
        }

        UpdateUI();
    }

    //finds which direction the bullet will go
    public Vector3 GetShootingDirection()
    {
        //find where gun is pointing at
        Vector3 targetPos = muzzle.position + muzzle.forward * gunData.maxDistance;
        //applies spread
        targetPos = new Vector3(
            targetPos.x + Random.Range(-gunData.spread, gunData.spread),
            targetPos.y,
            targetPos.z + Random.Range(-gunData.spread, gunData.spread)
            );

        //find direction from muzzle to target
        Vector3 direction = targetPos - muzzle.position;
        return direction.normalized;
    }

    /*!
     * function to handle the moment a shot is supposed to happen
     */
    private void ShootBullet()
    {
        Debug.Log("shooting bullet");

        //if the raycast connects with an object
        if (Physics.Raycast(muzzle.position, GetShootingDirection(), out RaycastHit hitInfo, gunData.maxDistance))
        {
            if (debug)
            {
                Debug.Log("shot connected");
            }

            //checks if the object can be damaged, then damages said object
            Debug.Log(hitInfo.transform.name);
            IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
            damageable?.TakeDamage(gunData.damage, DamageStateMachine.DamageType.Physical);

            //creates a bullet trail
            CreateTrail(hitInfo.point);
        }
        //if the raycast does not connect
        else
        {
            if (debug)
            {
                Debug.Log("no shot connection");
            }

            //creates a trail in the direction the gun was pointing
            CreateTrail(muzzle.position + (GetShootingDirection() * gunData.maxDistance));
        }


        currentTime = 0;
    }

    /*
     * creates and fades a bullet trail from muzzle to position
     */
    private void CreateTrail(Vector3 end)
    {
        if(bulletTrail != null)
        {
            LineRenderer lr;
            if (ObjectPool.instance.ObjectExistsInPool(bulletTrail.gameObject))
            {
                int i = ObjectPool.instance.FindIndexOfObject(bulletTrail.gameObject);
                if (ObjectPool.instance.PoolHasReadyObject(i))
                {
                    lr = ObjectPool.instance.SpawnObject(i).GetComponent<LineRenderer>();
                    lr.SetPosition(0, muzzle.position);
                    lr.SetPosition(1, end);
                    StartCoroutine(FadeTrail(lr));
                }
            }
        }
    }

    /*
     * fades selected bullet trail
     */
    IEnumerator FadeTrail(LineRenderer lr)
    {
        float alpha = 1;
        while(alpha > 0)
        {
            alpha -= Mathf.Clamp01(Time.deltaTime / trailFadeDuration);

            lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alpha);
            lr.endColor = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, alpha);

            yield return null;
        }

        ObjectPool.instance.Recycle(lr.gameObject);
        lr.gameObject.SetActive(false);

    }

    /*!
     * most vfx is handled here
     */
    private void OnShot()
    {
        AudioController.instance.PlaySound(sfxIndex);
    }

    /*!
     * called when reload is first called, used for vfx and animation
     */
    public void OnReload()
    {
        //animation is handled here
    }

    private void UpdateUI()
    {
        Inventory.instance.UpdateUI();
        PlayerController.instance.UpdateUI();
    }

}
