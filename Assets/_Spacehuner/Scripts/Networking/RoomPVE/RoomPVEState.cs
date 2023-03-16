using Colyseus.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Networking.PVE
{
    public class RoomPVEState : Schema
    {
        [Type(0, "map", typeof(MapSchema<RoomPVENetworkEntity>))]
        public MapSchema<RoomPVENetworkEntity> NetworkEntities = new MapSchema<RoomPVENetworkEntity>();

        [Type(1, "map", typeof(MapSchema<RoomPVENetworkEnemy>))]
        public MapSchema<RoomPVENetworkEnemy> NetworkEnemies = new MapSchema<RoomPVENetworkEnemy>();

    }
}
