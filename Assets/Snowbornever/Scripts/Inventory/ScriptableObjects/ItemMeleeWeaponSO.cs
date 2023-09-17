using TopDownCharacter2D.Attacks.Melee;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Inventory/MeleeWeapon")]
public class ItemMeleeWeaponSO : ItemSO
{
    [Tooltip("Configuration of melee weapon")]
    [SerializeField]
    private MeleeAttackConfig _meleeAttackConfig = default;
    
    public MeleeAttackConfig MeleeAttackConfig => _meleeAttackConfig;
}