using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SH.Define;
using SH.Networking.Space;


public class PlanetStation : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Spaceship")
        {
            RoomSpaceManager.Instance.LeaveRoom(() =>
            {
                 UIManager.Instance.LoadScene(SceneName.SceneStation);
            });
        }
    }

    public void OnBackClick()
    {
        RoomSpaceManager.Instance.LeaveRoom(() =>
        {
             UIManager.Instance.LoadScene(SceneName.SceneStation);
        });
    }
}
