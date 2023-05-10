using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ActiveAffer : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    async void Load()
    {
        await Task.Delay(4000);
        _objectList.ForEach(gameObject => gameObject.SetActive(true));

    }



}
