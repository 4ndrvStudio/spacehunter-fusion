using System;

namespace SH.Models.Azure
{
    public class ClaimItemRequestModel {
        public string ItemId;
        public int Level;
    }
    public class ClaimItemsResponeModel {
        public string PlayFabId;
        public string ItemId;
        public string ItemInstanceId;
        public string DisplayName;
    }
 
}