using System;
using System.Collections.Generic;
using Ozkaal.Gameplay.Gameplay.Player;
using Ozkaal.Gameplay.Gameplay.UI;
using UnityEngine;

namespace Gameplay.Creature.UI
{
    public class CreatureUI : MonoBehaviour
    {
        private void OnEnable()
        {
            foreach (var creature in CreatureManager.Creatures)
                Connect(creature);
            
            CreatureManager.OnCreatureCreated += Connect;
            CreatureManager.OnCreatureDestroyed += Disconnect;
        }
        private void OnDisable()
        {
            CreatureManager.OnCreatureCreated -= Connect;
            CreatureManager.OnCreatureDestroyed -= Disconnect;
            
            foreach (var creature in CreatureManager.Creatures)
                Disconnect(creature);

        }
        private void Connect(Creature creature)
        {
            creature.OnTalk += OnCreatureTalk;
        }

        private void Disconnect(Creature creature)
        {
            creature.OnTalk -= OnCreatureTalk;
        }


        private void OnCreatureTalk(Creature creature)
        {
            
        }
    }
}