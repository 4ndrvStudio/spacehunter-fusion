using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using SH.Multiplayer;
using SH;

public class ActiveAffer : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectList = new List<GameObject>();
    private bool _isInit;
    void OnEnable()
    {
        UIControllerManager.UIControllerEvent += ActiveTest;
    }
    void OnDisable()
    {
        UIControllerManager.UIControllerEvent -= ActiveTest;
    }
    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    async void Load()
    {
        _isInit = true;
        await Task.Delay(4500);
        _objectList.ForEach(gameObject => gameObject.SetActive(true));
        _isInit = true;

    }
    public void ActiveTest(bool isActive)
    {
        if(_isInit == false) return;

        _objectList.ForEach(gameObject => gameObject.SetActive(isActive));

    }


}
