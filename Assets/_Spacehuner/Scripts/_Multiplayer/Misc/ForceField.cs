using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer
{
    public class ForceField : MonoBehaviour
    {
        [SerializeField] Material _forceFieldMaterial;

        [SerializeField] MeshRenderer _meshRenderer;

        string[] toggleProperties = new string[4] {"_PLAYER1Toggle", "_PLAYER2Toggle", "_PLAYER3Toggle", "_PLAYER4Toggle"};
		string[] positionProperties = new string[4] {"_PositionPLAYER1", "_PositionPLAYER2", "_PositionPLAYER3", "_PositionPLAYER4"};

		void Awake()
		{
			_forceFieldMaterial = new Material(_forceFieldMaterial);
			_meshRenderer.material = _forceFieldMaterial;
		}

		void Update()
		{
                if (Network_Player.Local != null)
					_forceFieldMaterial.SetVector(positionProperties[0], Network_Player.Local.transform.position);

				_forceFieldMaterial.SetInt(toggleProperties[0], Network_Player.Local==null ? 0 : 1);
			
		}
    }

}
