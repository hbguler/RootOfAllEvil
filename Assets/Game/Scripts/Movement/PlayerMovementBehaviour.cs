using UnityEngine;

namespace Game.Scripts.Movement
{
    public class PlayerMovementBehaviour : MonoBehaviour
    {
        [SerializeField] private CharacterMovementBehaviour _cmb;

        public void Initialize()
        {
            _cmb.Initialize();
        }

        public void Update()
        {
            Vector3 _movementVector = _cmb.GetMovementVector();
            
            if (Input.GetKeyDown(KeyCode.W))
                _movementVector += Vector3.forward;
            if (Input.GetKeyDown(KeyCode.S))
                _movementVector += Vector3.back;
            if (Input.GetKeyDown(KeyCode.A))
                _movementVector += Vector3.left;
            if (Input.GetKeyDown(KeyCode.D))
                _movementVector += Vector3.right;

            if (Input.GetKeyUp(KeyCode.W))
                _movementVector -= Vector3.forward;
            if (Input.GetKeyUp(KeyCode.S))
                _movementVector -= Vector3.back;
            if (Input.GetKeyUp(KeyCode.A))
                _movementVector -= Vector3.left;
            if (Input.GetKeyUp(KeyCode.D))
                _movementVector -= Vector3.right;

            _cmb.SetMovementVector(_movementVector);
        }
    }
}
