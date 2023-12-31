using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemStats
{
    public int armor;
    public int speed;
    public int health;
    public float damage;
    public int HPRegeneration;
    public float critChance;

    internal void Sum(ItemStats stats)
    {
        armor += stats.armor;
        speed += stats.speed;
        health += stats.health;
        damage += stats.damage;
        HPRegeneration += stats.HPRegeneration;
        critChance += stats.critChance;
    }
}

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string Name;
    public ItemStats stats;
    public List<UpgradeData> upgrades;

    public void Init(string Name)
    {
        this.Name = Name;
        stats = new ItemStats();
        upgrades = new List<UpgradeData>();
    }

    public void Equip(Character character)
    {
        character.armor = stats.armor;
        character.Speed = stats.speed;
        character.MaxHP = stats.health;
        character.DamageBonus = stats.damage;
        character.HpRegenerationAmount = stats.HPRegeneration;
        character.CritChance = stats.critChance;
    }

    public void Unequip(Character character)
    {
        character.armor -= stats.armor;
        character.Speed = -stats.speed;
        character.MaxHP = -stats.health;
        character.DamageBonus = -stats.damage;
        character.HpRegenerationAmount -= stats.HPRegeneration;
        character.CritChance -= stats.critChance;
    }
}
