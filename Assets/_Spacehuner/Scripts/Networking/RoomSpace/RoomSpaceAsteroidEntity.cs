using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus.Schema;

public class RoomSpaceAsteroidEntity : Schema
{
	[Type(0, "string")]
	public string Id = default;

	[Type(1, "number")]
	public float PosX = default;

	[Type(2, "number")]
	public float PosY = default;

	[Type(3, "number")]
	public float PosZ = default;

	[Type(4, "number")]
	public float RotX = default;

	[Type(5, "number")]
	public float RotY = default;

	[Type(6, "number")]
	public float RotZ = default;

	[Type(7, "number")]
	public float RotW = default;

	public RoomSpaceAsteroidEntity Clone(RoomSpaceAsteroidEntity entity)
	{
		return new RoomSpaceAsteroidEntity()
		{
			Id = entity.Id,

			PosX = entity.PosX,
			PosY = entity.PosY,
			PosZ = entity.PosZ,

			RotX = entity.RotX,
			RotY = entity.RotY,
			RotZ = entity.RotZ,
			RotW = entity.RotW,
		};
	}
}
