using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Networking.PVE
{
    public class RoomPVEEntityFactory
    {
        private Dictionary<string, RoomPVENetworkEntity> _entities;
        private Dictionary<string, RoomPVENetworkEntityView> _entityViews;

        public RoomPVEEntityFactory(Dictionary<string, RoomPVENetworkEntity> entities, Dictionary<string, RoomPVENetworkEntityView> entityViews)
        {
            this._entities = entities;
            this._entityViews = entityViews;
        }

        public void RegisterNetworkEntityView(RoomPVENetworkEntity entity, RoomPVENetworkEntityView view, bool isMine)
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