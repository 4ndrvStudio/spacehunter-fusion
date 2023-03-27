using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Networking.Mining
{
    public class RoomMiningEntityFactory
    {
        private Dictionary<string, RoomMiningNetworkEntity> _entities;
        private Dictionary<string, RoomMiningNetworkEntityView> _entityViews;

        public RoomMiningEntityFactory(Dictionary<string, RoomMiningNetworkEntity> entities, Dictionary<string, RoomMiningNetworkEntityView> entityViews)
        {
            this._entities = entities;
            this._entityViews = entityViews;
        }

        public void RegisterNetworkEntityView(RoomMiningNetworkEntity entity, RoomMiningNetworkEntityView view, bool isMine)
        {
            if (entity == null || view == null)
            {
                Debug.LogError("entity null");
                return;
            }
            if (!_entities.ContainsKey(entity.Id))
            {
                Debug.LogError("Can not find entity in room");
                return;
            }

            view.InitView(entity, isMine);
            _entityViews.Add(entity.Id, view);

        }
    }
}
