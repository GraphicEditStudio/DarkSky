using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(menuName = "DarkSky/Weapon Settings")]
    public class WeaponSettings : ScriptableObject
    {
        [Header("Visuals")]
        public GameObject Model;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;
        
        [Header("Types")] 
        public WeaponHandType HandType;
        public bool IsMelee;
    }
}