using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SH.Multiplayer
{
    public class NPC_Movement : MonoBehaviour
    {

        [SerializeField] private NPC_Brain _npcBrain;

        [SerializeField] private List<Transform> _pathTransform = new List<Transform>();
        private List<Vector3> _pathPosition = new List<Vector3>();

        Sequence movementSequence = DOTween.Sequence();

        bool RequireUpdate = false;

        void Start() {
            
            _pathTransform.ForEach(pathTransform => {
                _pathPosition.Add(pathTransform.position);
            });

            movementSequence.Append(transform.DOPath(_pathPosition.ToArray(),5f,PathType.CubicBezier,PathMode.Full3D).SetLoops(-1).SetEase(Ease.Linear));
                
        
        }

    }

}
