using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Levels
{
    public enum TimeState
    {
        None = 0,
        Past = 1,
        Future = 2
    }
    public sealed class LevelBehaviour : MonoBehaviour
    {
        public static event Action TimeSwitched;

        [SerializeField] private PlayerBehaviour _player;
        [SerializeField] private List<EnemyBehaviour> _enemies;
        [SerializeField] private List<TimeSwitcher> _switchingObjects;

        [SerializeField] private TimeState _startingTimeState;
        private TimeState _currentTimeState;

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (_player.IsClimbing)
                {
                    return;
                }
                
                TimeSwitched?.Invoke();

                if (_currentTimeState == TimeState.Future)
                    _currentTimeState = TimeState.Past;
                else if (_currentTimeState == TimeState.Past)
                    _currentTimeState = TimeState.Future;
                
                UpdateTimeState();
            }
        }

        public void Initialize()
        {
            _player.Initialize();

            foreach (var enemy in _enemies)
            {
                enemy.Initialize(_player);
            }

            _currentTimeState = _startingTimeState;
            UpdateTimeState();
        }

        private void UpdateTimeState()
        {
            _player.SwitchCharacter(_currentTimeState);

            foreach (TimeSwitcher switchingObject in _switchingObjects)
            {
                switchingObject.UpdateState(_currentTimeState);
            }
        }
    }
}
