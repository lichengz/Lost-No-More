using TopDownCharacter2D.Attacks.Range;
using UnityEngine;


[CreateAssetMenu(fileName = "RangeWeapon", menuName = "Inventory/RangeWeapon")]
public class ItemRangeWeaponSO : ItemSO
{
    [Tooltip("Configuration of range weapon")]
    [SerializeField]
    private RangedAttackConfig _rangedAttackConfig = default;
    
    public RangedAttackConfig RangedAttackConfig => _rangedAttackConfig;
}