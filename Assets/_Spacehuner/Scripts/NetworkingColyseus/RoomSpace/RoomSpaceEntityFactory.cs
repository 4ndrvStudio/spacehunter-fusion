using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Networking.Space
{
    public class RoomSpaceEntityFactory
    {
        private Dictionary<string, RoomSpaceNetworkEntity> _entities;
        private Dictionary<string, RoomSpaceNetworkEntityView> _entityViews;

        public RoomSpaceEntityFactory(Dictionary<string, RoomSpaceNetworkEntity> entities, Dictionary<string, RoomSpaceNetworkEntityView> entityViews)
        {
            this._entities = entities;
            this._entityViews = entityViews;
        }

        public void RegisterNetworkEntityView(RoomSpaceNetworkEntity entity, RoomSpaceNetworkEntityView view, bool isMine)
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
