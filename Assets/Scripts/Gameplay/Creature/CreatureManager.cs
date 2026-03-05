using System;
using System.Collections.Generic;
using UnityEngine;

public static class CreatureManager
{
    public static event Action<Creature> OnCreatureCreated;
    public static event Action<Creature> OnCreatureDestroyed;
        
    private static List<Creature> creatures = new();
        
    public static IReadOnlyList<Creature> Creatures => creatures;

    public static Creature CreateCreature()
    {
        var creature = new Creature();
        creatures.Add(creature);
        OnCreatureCreated?.Invoke(creature);
        return creature;
    }

    public static bool DestroyCreature(Creature creature)
    {
        if (creatures.Remove(creature))
        {
            OnCreatureDestroyed?.Invoke(creature);
            return true;
        }
        return false;
    }
}