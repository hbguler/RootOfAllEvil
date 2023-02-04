using System.Collections.Generic;
using Game.Scripts.Levels;
using UnityEngine;

namespace Game.Scripts
{
    public class TimeSwitcher : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _pastObjects;
        [SerializeField] private List<GameObject> _futureObjects;
        
        
        public void UpdateState(TimeState timeState)
        {
            foreach (GameObject pastObject in _pastObjects)
            {
                pastObject.gameObject.SetActive(timeState == TimeState.Past);
            }

            foreach (GameObject futureObject in _futureObjects)
            {
                futureObject.gameObject.SetActive(timeState == TimeState.Future);
            }
        }
    }
}
