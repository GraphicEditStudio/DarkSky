using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Weapons
{
    [CreateAssetMenu(menuName = "DarkSky/Weapon Settings")]
    public class WeaponSettings : ScriptableObject
    {
        [Header("Visuals")]
        public GameObject ModelPrefab;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;
        
        [Header("Types")]
        public WeaponHandType HandType;
        public bool IsMelee;

        [Header("Shoot Configuration")]
        public LayerMask HitMask;
        public Vector3 Spread = new (0.1f, 0.1f, 0.1f);
        public float FireRate = 0.25f;

        [Header("Trail Config")] 
        public Material Material;
        public AnimationCurve WidthCurve;
        public float Duration;
        public float MinVertexDistance = 0.1f;
        public Gradient Color;
        public float MissDistance = 100f;
        public float SimulationSpeed = 100f;

        private MonoBehaviour ActiveMonoBehaviour;
        private GameObject Model; // might not need
        private float LastShootTime = 0;
        private ParticleSystem ShootSystem;
        private ObjectPool<TrailRenderer> TrailPool;

        public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour)
        {
            this.ActiveMonoBehaviour = ActiveMonoBehaviour;
            LastShootTime = 0;
            TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
            Model = Instantiate(ModelPrefab);
            Model.transform.SetParent(Parent);
            Model.transform.localPosition = PositionOffset;
            Model.transform.localRotation = Quaternion.Euler(RotationOffset);

            ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
        }

        public void EnableModel()
        {
            Model.SetActive(true);
        }

        public void DisableModel()
        {
            Model.SetActive(false);
        }

        public void Shoot()
        {
            if (Time.time > FireRate + LastShootTime)
            {
                LastShootTime = Time.time;
                ShootSystem.Play();
                var shootDirection = ShootSystem.transform.forward;
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

                var hitPoint = ShootSystem.transform.position + (shootDirection * MissDistance);
                var castHit = new RaycastHit();
                if (Physics.Raycast(
                        ShootSystem.transform.forward,
                        shootDirection,
                        out RaycastHit hit,
                        float.MaxValue,
                        HitMask
                    )
                   )
                {
                    hitPoint = hit.point;
                    castHit = hit;
                }
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        ShootSystem.transform.position,
                        hitPoint,
                        castHit
                    )
                );
            }
        }

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