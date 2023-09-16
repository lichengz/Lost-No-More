using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public enum WeaponType
    {
        Melee,
        Range,
    }

    public WeaponType weaponType;
    public Transform projectileSpawnPosition;
    public SpriteRenderer weaponRenderer;
}
