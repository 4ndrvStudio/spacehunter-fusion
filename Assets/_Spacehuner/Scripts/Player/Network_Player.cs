using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.Events;
using TMPro;
using SH.PlayerData;

namespace SH.Multiplayer
{
    public class Network_Player : NetworkBehaviour, IPlayerLeft
    {
        public static Network_Player Local { get; set; }

        [SerializeField] private NetworkRigidbody _netRigid;
        [SerializeField] private Network_PlayerState _playerState;
        public Network_PlayerState PlayerState => _playerState;

        [SerializeField] private Network_PlayerAnimation _playerAnimation;
        public Network_PlayerAnimation PlayerAnimation => _playerAnimation;

        [SerializeField] private Network_PlayerMovement _playerMovement;
        public Network_PlayerMovement PlayerMovement => _playerMovement;

        [SerializeField] private Network_WeaponCollider _networkWeaponCollider;
        [SerializeField] private Network_WeaponManager _weaponManager;
        public Network_WeaponManager WeaponManager => _weaponManager;

        [SerializeField] private Network_PlayerCombat _playerCombat;
        public Network_PlayerCombat PlayerCombat=> _playerCombat;

     
        private GameObject _body;
        [SerializeField] private Transform _lookPoint;
  
        [Networked(OnChanged = nameof(OnNickNameChanged))]
        public NetworkString<_16> nickName { get; set; }
        
      
        [SerializeField]  private List<GameObject> _bodyList = new List<GameObject>();

        [Networked(OnChanged = nameof(OnBodyChanged))]
        public byte Body {get; set;}


        //Display Data 
        [SerializeField] private TextMeshPro _nameUI;


        //fortest  
        public Network_AnimatorHook AnimatorHook;




        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                
                Local = this;
                UIControllerManager.Instance.ShowGotoMiningBtn(false);

                RPC_SetBody((int)PlayerDataManager.Character.Data.CharacterInUse.CharacterType);
                //RPC_SetBody(3);
            

                if((int)Runner.CurrentScene > 1 ) 
                    UIControllerManager.Instance.DisplayController();
                else 
                    UIControllerManager.Instance.HideAllController();

                //UIManager.Instance.ShowChat();

                RPC_SetNickName(PlayerDataManager.DisplayName);
            
            }
        }

        public void PlayerLeft(PlayerRef player)
        {
            if (player == Object.InputAuthority)
                Runner.Despawn(Object);
        }

        static void OnNickNameChanged(Changed<Network_Player> changed)
        {

            changed.Behaviour.OnNickNameChanged();
        }

        private void OnNickNameChanged()
        {
            Debug.Log($"Nickname changed for player to {nickName} for player {gameObject.name}");
            _nameUI.text = nickName.ToString();
        }

        

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetNickName(string nickName, RpcInfo info = default)
        {
            Debug.Log($"[RPC] SetNickName {nickName}");
            this.nickName = nickName;
        }

        
        static void OnBodyChanged(Changed<Network_Player> changed)
        {
            changed.Behaviour.OnBodyChanged();
        }

        private void OnBodyChanged()
        {
            this._body =  Instantiate(_bodyList[(int)Body -1 ],this.transform);


            if(Object.HasInputAuthority) {
                    var targetLookPoint = Resources.Load<GameObject>("Camera/LookPoint");
                    GameObject lookPoint =Instantiate(targetLookPoint);
                    lookPoint.GetComponent<TargetLookPoint>().TargetFollow = _body.transform;
                    CameraManager.Instance.SetAimTarget(_body.transform, lookPoint.transform);
            }
      

            _netRigid.InterpolationTarget = this._body.transform;

            Animator animator = gameObject.GetComponentInChildren<Animator>();
            _playerState.Anim = animator;
            _playerAnimation.Anim = animator;
            
            AnimatorHook = this.gameObject.GetComponentInChildren<Network_AnimatorHook>();

            AnimatorHook.SetWeaponCollider(_networkWeaponCollider);
            AnimatorHook.SetComboVFXList(_playerCombat.ComboVFXList);
         
            Network_WeaponHook weaponHook = this.gameObject.GetComponentInChildren<Network_WeaponHook>();
            
            _weaponManager.SetupWeapon(weaponHook.WeaponHolder, AnimatorHook);
            
        }
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetBody(int body, RpcInfo info = default)
        {
            this.Body = (byte)body ;
        }
        

    }

}
