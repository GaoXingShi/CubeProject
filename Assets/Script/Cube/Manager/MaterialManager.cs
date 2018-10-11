
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : Singleton<MaterialManager>
{
    public Material noneMaterial,
        bombx4Material,
        bombx8Material,
        bombHorizontalMaterial,
        bombVerticaMateriall,
        bindingMaterial,
        showMaterial;
    private Dictionary<TriggerCubeScript.ETriggerState, Material> materialDict;

    private new void Awake()
    {
        base.Awake();
        materialDict = new Dictionary<TriggerCubeScript.ETriggerState, Material>();
        materialDict.Add(TriggerCubeScript.ETriggerState.None, noneMaterial);
        materialDict.Add(TriggerCubeScript.ETriggerState.Bombx4, bombx4Material);
        materialDict.Add(TriggerCubeScript.ETriggerState.Bombx8, bombx8Material);
        materialDict.Add(TriggerCubeScript.ETriggerState.BombHorizontal, bombHorizontalMaterial);
        materialDict.Add(TriggerCubeScript.ETriggerState.BombVertical, bombVerticaMateriall);
        materialDict.Add(TriggerCubeScript.ETriggerState.Binding, bindingMaterial);
        materialDict.Add(TriggerCubeScript.ETriggerState.Show, showMaterial);
    }

    public Material GetTriggerMaterial(TriggerCubeScript.ETriggerState _trigger)
    {
        return materialDict[_trigger];
    }
}
