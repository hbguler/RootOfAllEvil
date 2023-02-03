using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Levels
{
    public sealed class LevelBehaviour : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour _player;
        [SerializeField] private List<EnemyBehaviour> _enemies;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _player.Initialize();

            foreach (var enemy in _enemies)
            {
                enemy.Initialize(_player);
            }
        }
    }
}
