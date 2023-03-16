using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer
{
    public class Network_RoomManager : MonoBehaviour
    {
        public static Network_RoomManager Instance;

        [SerializeField] private Transform _spawnPosition;

        void Awake() {
            if(Instance != null || Instance != this) {
                Destroy(this);
            }else {
                Instance = this;
            }
        }

        void OnDestroy() {
            if(Instance == this) Instance = null;
        }


        public Transform GetSpawnPosition() => _spawnPosition;


    }

}
