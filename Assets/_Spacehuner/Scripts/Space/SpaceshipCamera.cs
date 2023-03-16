using SH.Networking.Space;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SpaceshipCamera : MonoBehaviour
{

	public Transform targetPoint;

	public float moveSpeed;

	public float rotateSpeed;

	public bool shouldFollow;

	[SerializeField] private bool _isLoading = true;



	private async void Start()
	{
		if (!_isLoading)
			return;
		UIManager.Instance.ShowWaiting();
		await Task.Delay(2000);
		UIManager.Instance.HideWaiting();

		var entities = FindObjectsOfType<RoomSpaceNetworkEntityView>();
		foreach(var entity in entities)
        {
			if (entity.IsMine)
				targetPoint = entity.LookPoint;
        }
	}

	private void LateUpdate()
	{
		if(targetPoint != null)
        {
			base.transform.position = Vector3.Lerp(base.transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, targetPoint.rotation, rotateSpeed * Time.deltaTime);
		}
	}

	public void ResetCamera()
	{
		shouldFollow = true;
		base.transform.position = targetPoint.position;
		base.transform.rotation = targetPoint.rotation;
	}
}
