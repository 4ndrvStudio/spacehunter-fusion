using SH.PlayerData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SH
{
    public class UIInventoryPopup : UIPopup
    {

        private bool _init = false;

        // this just for test
        enum WeaponInUse
        {
            MineralAxe,
            Sword
        }

        [SerializeField] private Button _closeBtn;
        [SerializeField] private Button _miningBtn;
        [SerializeField] private Button _swordBtn;
        [SerializeField] private Button _useWeaponBtn;
        [SerializeField] private WeaponInUse _weaponInUse;

        [SerializeField] private Image _displayWeaponImage;
        [SerializeField] private Sprite _mineralAxeImage;
        [SerializeField] private Sprite _swordImage;



        private void OnEnable() {
            InventoryManager.OnInventoryDataChange += UpdateView;
        }
        private void OnDisable() {
            InventoryManager.OnInventoryDataChange -= UpdateView;

        }

        private void Start()
        {
            Show();

            //this is for test
            _miningBtn.onClick.AddListener(() =>
            {
                _weaponInUse = WeaponInUse.MineralAxe;
                _displayWeaponImage.sprite = _mineralAxeImage;

            });
            _swordBtn.onClick.AddListener(() =>
            {
                _weaponInUse = WeaponInUse.Sword;
                _displayWeaponImage.sprite = _swordImage;
            });

            _closeBtn.onClick.AddListener(() =>
            {
                Hide();
            });
            _useWeaponBtn.onClick.AddListener(() =>
            {
                Hide();
            });
        }

        public override void Show(object customProperties = null)
        {
            base.Show(customProperties);
            Setup();
            
        }

        public override void Hide()
        {
            base.Hide();
            UIControllerManager.Instance.DisplayController();
        }

        private void Setup()
        {
            if (!_init)
            {
                _init = true;
            }
        }

        private void UpdateView() {
            Debug.Log("Update view" + InventoryManager.Instance.Items.Count);
        }


    }

}
