using UnityEngine;
using Random = System.Random;

namespace Gameplay.Creature
{
    public class WorldCreatureSymbolGroup
    {
        public CreatureSymbolGroupData CreatureSymbolGroupData { get; private set; }

        public WorldCreatureSymbolGroup(CreatureSymbolGroupData creatureSymbolGroupData)
        {
            CreatureSymbolGroupData = creatureSymbolGroupData;
        }
    }
}