using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private Transform projectilePool;
    
    [SerializeField] private Transform activeProjectilePool;
    
    [SerializeField] private Transform spawnPoint;
    
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private float projectileLifeTime, despawnLifetime;
    
    [SerializeField] private float fireRate;

    [SerializeField] private GameObject player;

    [SerializeField] private Transform spawnPointlvl2r;
    [SerializeField] private Transform spawnPointlvl2l;

    [SerializeField] private Transform spawnPointlvl4r;
    [SerializeField] private Transform spawnPointlvl4l;

    private bool _canSpawn = true;

    private void Start()
    {
        InputManager.Instance.FirePerformed += OnFirePerformed;
    }

    private void OnFirePerformed()
    {
        GameObject player = GameObject.Find("Player");
        LvlControler script_puntos = player.GetComponent<LvlControler>();
        int nivel_actual = script_puntos.Getnivel();

        if (!_canSpawn) return;

        _canSpawn = false;
        StartCoroutine(SpawnCooldown());

        GameObject projectile = null;





        if (nivel_actual == 0)
        {
            if (projectilePool.childCount <= 0)
            {
                // Aquí se asegura que la rotación coincida con el spawnPoint
                projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                projectile = projectilePool.GetChild(0).gameObject;
                projectile.transform.position = spawnPoint.position;
                projectile.transform.rotation = spawnPoint.rotation; // Ajustar la rotación aquí
                projectile.SetActive(true);
            }

        }




        if (nivel_actual == 1)
        {

            if (projectilePool.childCount <= 2)
            {
                // Aquí se asegura que la rotación coincida con el spawnPoint
                projectile = Instantiate(projectilePrefab, spawnPointlvl2r.position, spawnPoint.rotation);

                projectile = Instantiate(projectilePrefab, spawnPointlvl2l.position, spawnPoint.rotation);

                projectile.transform.SetParent(activeProjectilePool);
                StartCoroutine(DestroyProjectile(projectile.GetComponent<Projectile>()));
            }
            else
            {
                projectile = projectilePool.GetChild(0).gameObject;
                projectile.transform.position = spawnPointlvl2r.position;
                projectile.transform.rotation = spawnPoint.rotation;
                projectile.SetActive(true);


                projectile = projectilePool.GetChild(1).gameObject;
                projectile.transform.position = spawnPointlvl2l.position;
                projectile.transform.rotation = spawnPoint.rotation;
                projectile.SetActive(true);

            }

        }
        if (nivel_actual == 2)
        {

            if (projectilePool.childCount <= 3)
            {
                // Aquí se asegura que la rotación coincida con el spawnPoint
                projectile = Instantiate(projectilePrefab, spawnPointlvl2r.position, spawnPoint.rotation);

                projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

                projectile = Instantiate(projectilePrefab, spawnPointlvl2l.position, spawnPoint.rotation);
            }
            else
            {
                projectile = projectilePool.GetChild(0).gameObject;
                projectile.transform.position = spawnPointlvl2r.position;
                projectile.transform.rotation = spawnPoint.rotation;
                projectile.SetActive(true);

                projectile = projectilePool.GetChild(1).gameObject;
                projectile.transform.position = spawnPoint.position;
                projectile.transform.rotation = spawnPoint.rotation; // Ajustar la rotación aquí
                projectile.SetActive(true);

                projectile = projectilePool.GetChild(2).gameObject;
                projectile.transform.position = spawnPointlvl2l.position;
                projectile.transform.rotation = spawnPoint.rotation;
                projectile.SetActive(true);

            }

        }
        if (nivel_actual == 3)
        {

            if (projectilePool.childCount <= 4)
            {
                // Aquí se asegura que la rotación coincida con el spawnPoint
                projectile = Instantiate(projectilePrefab, spawnPointlvl2r.position, spawnPoint.rotation);

                projectile = Instantiate(projectilePrefab, spawnPointlvl4r.position, spawnPoint.rotation);

                projectile = Instantiate(projectilePrefab, spawnPointlvl2l.position, spawnPoint.rotation);

                projectile = Instantiate(projectilePrefab, spawnPointlvl4l.position, spawnPoint.rotation);
            }
            else
            {
                projectile = projectilePool.GetChild(0).gameObject;
                projectile.transform.position = spawnPointlvl2r.position;
                projectile.transform.rotation = spawnPoint.rotation;
                projectile.SetActive(true);


                projectile = projectilePool.GetChild(1).gameObject;
                projectile.transform.position = spawnPointlvl2l.position;
                projectile.transform.rotation = spawnPoint.rotation;
                projectile.SetActive(true);

                projectile = projectilePool.GetChild(2).gameObject;
                projectile.transform.position = spawnPointlvl4l.position;
                projectile.transform.rotation = spawnPoint.rotation;
                projectile.SetActive(true);


                projectile = projectilePool.GetChild(3).gameObject;
                projectile.transform.position = spawnPointlvl4r.position;
                projectile.transform.rotation = spawnPoint.rotation;
                projectile.SetActive(true);


            }

        }

        projectile.transform.SetParent(activeProjectilePool);
        StartCoroutine(DestroyProjectile(projectile.GetComponent<Projectile>()));

    }



    private IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(fireRate);
        _canSpawn = true;
    }

    private IEnumerator DestroyProjectile(Projectile projectile)
    {
        yield return new WaitForSeconds(projectileLifeTime);
        projectile.DestroyProjectile();
        yield return new WaitForSeconds(despawnLifetime);
        projectile.gameObject.SetActive(false);
        projectile.transform.SetParent(projectilePool);
    }
    public void cambiarvelocidad(float cambio)
    {
        fireRate = fireRate - cambio;
    }


}