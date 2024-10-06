using UnityEngine;

/// <summary>
/// Add the material to the PhysicsPM_MatDetector array and the physicmaterial to the mesh collider
/// on the surface ground eg., wood, dirt, etc. 
/// </summary>
[CreateAssetMenu(fileName = "Create PhysicMaterial", menuName = "New PhysicMaterial/Create New Material")]
public class PhysicMaterialCreator : ScriptableObject
{
    public PhysicMaterial pmMatKey;
    public new string name;
    public bool pmMatTrue;
    [Header("Set a value here for each new material")]
    public int matIntIdxValue; // This is used to set the matIdx

    public string PhysicsMaterialKey => pmMatKey ? pmMatKey.name : null;
}





