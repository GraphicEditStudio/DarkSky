using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inventory.Items;
using UnityEngine;
using UnityEngine.Pool;

namespace Weapons
{
    [CreateAssetMenu(menuName = "DarkSky/Weapon Settings")]
    public class WeaponSettings : ItemScriptable
    {
        [Header("Visuals")] public Vector3 PositionOffset;
        public Vector3 RotationOffset;

        [Header("Types")] public WeaponHandType HandType;
        public bool IsMelee;
        public bool AutoFire;

        [Header("Damage")] public float Damage;

        [Header("Shoot Configuration")] public LayerMask HitMask;
        public Vector3 Spread = new(0.1f, 0.1f, 0.1f);
        public float FireRate = 0.25f;
        public int BulletsPerFire = 1;
        public float SpeadNumber;

        [Header("Trail Config")] public Material Material;
        public AnimationCurve WidthCurve;
        public float Duration;
        public float MinVertexDistance = 0.1f;
        public Gradient Color;
        public float MissDistance = 100f;
        public float SimulationSpeed = 100f;

        [Header("Ammo")] public AmmoSettings InitialAmmo;
        public int ClipSize;
        public int AmmoCapacity;
        public float ReloadTime;

        private MonoBehaviour ActiveMonoBehaviour;
        private GameObject Model; // might not need
        private float LastShootTime = 0;
        private ParticleSystem ShootSystem;
        private ObjectPool<TrailRenderer> TrailPool;

        public AmmoHandler AmmoHandler;

        public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour)
        {
            this.ActiveMonoBehaviour = ActiveMonoBehaviour;
            LastShootTime = 0;
            TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
            Model = Instantiate(Prefab, Parent);
            Model.transform.localPosition = PositionOffset;
            Model.transform.localRotation = Quaternion.Euler(RotationOffset);
            ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
            this.AmmoHandler = new AmmoHandler();
            AmmoHandler.Initialize(0, ClipSize, 0, AmmoCapacity);
        }

        public void EnableModel()
        {
            if (ShootSystem)
            {
                ShootSystem.Stop(true);
            }
            Model.SetActive(true);
        }

        public void DisableModel()
        {
            if (ShootSystem)
            {
                ShootSystem.Stop(true);
            }
            Model.SetActive(false);
        }

        public (bool didShoot, IEnumerable<(RaycastHit? CastHit, Vector3 HitPoint)> hits) Shoot()
        {
            if (Time.time > FireRate + LastShootTime)
            {
                if (!IsMelee)
                {
                    var canShoot = AmmoHandler.GrabAmmo();
                    if (canShoot)
                    {
                        LastShootTime = Time.time;
                        if (ShootSystem)
                        {
                            // MuzzleFlash
                            ShootSystem.Play(true);
                        }
                        return (true, ShootSpread());
                    }
                    else
                    {
                        Debug.Log("Out of Ammo");
                    }
                }
                else
                {
                    Debug.Log("Melee Attack");
                }
            }

            return (false, new List<(RaycastHit? CastHit, Vector3 HitPoint)>());
        }

        private IEnumerable<(RaycastHit? CastHit, Vector3 HitPoint)> ShootSpread()
        {
            var screenPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var point = Camera.main.ScreenToWorldPoint(screenPoint);
            var forward = Camera.main.transform.forward;
            return Enumerable.Range(1, BulletsPerFire).Select(b => ShootBullet(point, forward));
        }

        private (RaycastHit?, Vector3) ShootBullet(Vector3 shootingPoint, Vector3 direction)
        {
            var shootDirection = direction;
            shootDirection += new Vector3(
                Random.Range(
                    -Spread.x,
                    Spread.x
                ),
                Random.Range(
                    -Spread.y,
                    Spread.y
                ),
                Random.Range(
                    -Spread.y,
                    Spread.y
                )
            );
            shootDirection.Normalize();
            var hitPoint = direction + (shootDirection * MissDistance);
            RaycastHit? castHit = null;
            if (Physics.Raycast(
                    shootingPoint,
                    shootDirection,
                    out RaycastHit hit,
                    float.MaxValue,
                    HitMask,
                    QueryTriggerInteraction.Ignore
                )
               )
            {
                hitPoint = hit.point;
                castHit = hit;
            }

            //Debug.DrawRay(shootingPoint, hitPoint, UnityEngine.Color.red, 20);
            // ActiveMonoBehaviour.StartCoroutine(
            //     PlayTrail(
            //         shootingPoint, //ShootSystem.transform.position,
            //         hitPoint,
            //         castHit ?? new RaycastHit()
            //     )
            // );

            return (castHit, hitPoint);
        }

        // failed code for trail... might revisit later
        private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit)
        {
            var instance = TrailPool.Get();
            instance.gameObject.SetActive(true);
            instance.transform.position = StartPoint;
            yield return null;
            instance.emitting = true;
            var distance = Vector3.Distance(StartPoint, EndPoint);
            var remainingDistance = distance;
            while (remainingDistance > 0)
            {
                instance.transform.position = Vector3.Lerp(
                    StartPoint,
                    EndPoint,
                    Mathf.Clamp01(1 - (remainingDistance / distance))
                );
                remainingDistance -= SimulationSpeed * Time.deltaTime;
                yield return null;
            }

            instance.transform.position = EndPoint;

            if (Hit.collider != null)
            {
                //HandleImpact(Hit.transform.gameObject, EndPoint, Hit.normal, ImpactType, 0);
                Debug.Log("HIT SOMETHING!");
            }

            yield return new WaitForSeconds(Duration);
            yield return null;
            instance.emitting = false;
            instance.gameObject.SetActive(false);
            TrailPool.Release(instance);
        }

        private TrailRenderer CreateTrail()
        {
            var instance = new GameObject("Bullet Trail");
            TrailRenderer trail = instance.AddComponent<TrailRenderer>();
            trail.colorGradient = Color;
            trail.material = Material;
            trail.widthCurve = WidthCurve;
            trail.time = Duration;
            trail.minVertexDistance = MinVertexDistance;
            trail.emitting = false;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            return trail;
        }
    }
}