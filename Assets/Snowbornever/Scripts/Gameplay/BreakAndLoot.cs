using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakAndLoot : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private GameObject shadow;
    [SerializeField] 
    private DroppableRewardConfigSO _dropRewardConfig;
    private ItemSO loot;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        var main = _particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        shadow = transform.Find("shadow").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer ("hitbox")) return;
        _particleSystem.Play();
        _spriteRenderer.enabled = false;
        _boxCollider2D.enabled = false;
        shadow.SetActive(false);
        DropAllRewards(transform.position);
    }

    void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
    
    private void DropAllRewards(Vector3 position)
    {
        DropGroup specialDropItem = _dropRewardConfig.DropSpecialItem(); 
        if (specialDropItem != null) // drops a special item if any 
            DropOneReward(specialDropItem, position);
        // Drop items
        foreach (DropGroup dropGroup in _dropRewardConfig.DropGroups)
        {
            float randValue = UnityEngine.Random.value;
            if (dropGroup.DropRate >= randValue)
            {
                DropOneReward(dropGroup, position);
            }
            else
            {
                break;
            }
        }
    }

    private void DropOneReward(DropGroup dropGroup, Vector3 position)
    {
        float dropDice = UnityEngine.Random.value;
        float _currentRate = 0.0f;

        ItemSO item = null;
        GameObject itemPrefab = null;

        foreach (DropItem dropItem in dropGroup.Drops)
        {
            _currentRate += dropItem.ItemDropRate;
            if (_currentRate >= dropDice)
            {
                item = dropItem.Item;
                itemPrefab = dropItem.Item.Prefab;
                break;
            }
        }

        float randAngle = UnityEngine.Random.value * Mathf.PI * 2;
        GameObject collectibleItem = GameObject.Instantiate(itemPrefab,
            position + itemPrefab.transform.localPosition +
            _dropRewardConfig.ScatteringDistance * (Mathf.Cos(randAngle) * Vector3.forward + Mathf.Sin(randAngle) * Vector3.right),
            Quaternion.identity);
        collectibleItem.GetComponent<CollectableItem>().AnimateItem();
    }
}
