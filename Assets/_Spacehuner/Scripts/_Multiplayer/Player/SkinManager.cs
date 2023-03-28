using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.ConfigData;


[System.Serializable]
public class Properties {
    public int trait1;
    public int trait2;
    public int trait3;
    public int trait4;
    public int trait5;
    public int trait6;
    public int trait7;
    public int trait8;
} 

[System.Serializable]
public class CharacterInfor {
    public string Name;
    public int hunterType;
    public Properties Properties;
}

public class SkinManager : MonoBehaviour
{
    
    public SkinCharacterConfig SkinConfig;
    public List<SkinnedMeshRenderer> _traitsSRC = new List<SkinnedMeshRenderer>();

    //for test
    [SerializeField] private CharacterInfor _characterInfor = new CharacterInfor();

    void Start() {
       LoadSkin(_characterInfor);
       
    }

    public void LoadSkin( CharacterInfor characterInfo)
    {
       for (int i = 0; i < SkinConfig.TraitList.GetType().GetFields().Length-1; i++) {
           int index = (int) characterInfo.Properties.GetType().GetFields()[i].GetValue(characterInfo.Properties);
           var _trait = SkinConfig.TraitList.GetType().GetFields()[i];
            Trait trait = _trait.GetValue(SkinConfig.TraitList) as Trait;
           if(trait.Name == TraitName.Weapon || index == 0) continue;
           SetupSkinnedMesh(_traitsSRC[i], trait.Meshes[index-1], trait.Materials[index-1].Payload,index);
       }
    }

    private void SetupSkinnedMesh(SkinnedMeshRenderer skinnedMesh, Mesh mesh, Material[] materials, int index) {
        skinnedMesh.sharedMesh = mesh;
        skinnedMesh.materials = materials;
    }

}

