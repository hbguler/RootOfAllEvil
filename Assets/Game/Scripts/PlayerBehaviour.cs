using Game.Scripts.Movement;
using UnityEngine;

namespace Game.Scripts
{
    public class PlayerBehaviour : CharacterBehaviour
    {
        [SerializeField] private PlayerMovementBehaviour _pmb;

        public void Initialize()
        {
            _pmb.Initialize();
        }
    }
}
