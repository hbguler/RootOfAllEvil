using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Game.Scripts.Levels
{
    public class LevelBehaviour : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour _cb;
        public virtual void Initialize()
        {
            _cb.Initialize();
        }
    }
}
