using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Threading.Tasks;

namespace SH.Multiplayer
{

    public class Network_Mineral : NetworkBehaviour
    {

        [HideInInspector] public Network_RoomMining Network_RoomMining = default;

        //network 
        [Networked] private NetworkBool _wasHit { get; set; }

        [Networked(OnChanged = nameof(OnHpChanged))]
        [SerializeField] private byte _hp { get; set; }
        [SerializeField] private bool _wasSetupHp;

        //local
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private GameObject _body;
        [SerializeField] private GameObject _destroyFX;



        public override void Spawned()
        {
            _healthBar.Setup(_hp);
        }

        void Update()
        {
            if (!_wasSetupHp)
            {
                _healthBar.Setup(_hp);
                _wasSetupHp = true;
            }
        }



        public void HitMineral(PlayerRef player)
        {
            if (Object == null) return;
            if (Object.HasStateAuthority == false) return;
            if (_wasHit) return;


            if (Runner.TryGetPlayerObject(player, out var playerNetworkObject))
            {

                playerNetworkObject.GetComponentInChildren<Network_WeaponCollider>().ToggleActiveCollider(CanHitName.Mineral, false);

            }
            _wasHit = true;
        }
        public override void FixedUpdateNetwork()
        {


            if (Object.HasStateAuthority && _wasHit)
            {
                _wasHit = false;
                _hp--;


                if (_hp <= 0)
                {

                    MineralDestroy();


                }
            }

        }

        async void MineralDestroy()
        {
            await Task.Delay(2000);
            Network_RoomMining.MineralCollected(Object);
        }


        static void OnHpChanged(Changed<Network_Mineral> changed)
        {
            changed.Behaviour.OnHpChanged();
        }
        private void OnHpChanged()
        {
            if (_hp <= 0)
            {
                _healthBar.gameObject.SetActive(false);
                _body.SetActive(false);
                _destroyFX.SetActive(true);
            }
            _healthBar.UpdateHealth(_hp);

        }



    }

}
