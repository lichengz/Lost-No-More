using TopDownCharacter2D.Attacks.Melee;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Equip Melee Weapon Event Channel")]
public class EquipMeleeWeaponEventChannelSO : DescriptionBaseSO
{
    public UnityAction<MeleeAttackConfig> OnEventRaised;

    public void RaiseEvent(MeleeAttackConfig value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
