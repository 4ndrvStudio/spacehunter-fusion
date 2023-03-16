using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.Define;
using SH.Networking.Space;


public class Asteroid : MonoBehaviour
{
    private void OnParticleCollision(GameObject other) {
        Debug.Log(other.name);
        // RoomSpaceManager.Instance.LeaveRoom(() =>
        // {
        //     UIManager.Instance.LoadScene(SceneName.SceneStation);
        // });
    }
}
