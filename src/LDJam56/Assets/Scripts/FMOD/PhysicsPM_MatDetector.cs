using UnityEngine;
/// <summary>
/// Place this script on your player character. It gets the material values and sets the
/// matIdx to be used to set param, switches in your middleware. 
/// </summary>
public class PhysicsPM_MatDetector : MonoBehaviour
{
    [Header("Add the material from PhysicMaterialCreator here")]
    [SerializeField] private PhysicMaterialCreator[] physicsMatCreator;
    public int matIdx { get; private set; }

    private bool[] materialStatus;
    //change this if you're using a rigid body. 
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        if (rb == null)
        {        
                Debug.LogError("No Rigidbody found on this GameObject or its parent.");          
        }
        materialStatus = new bool[physicsMatCreator.Length];
    }

    private void OnTriggerEnter(Collider other)
    {
        PhysicMaterial material = other.sharedMaterial;
        if (material != null)
        {
            for (int i = 0; i < physicsMatCreator.Length; i++)
            {
                PhysicMaterialCreator physicsToSound = physicsMatCreator[i];
                if (material == physicsToSound.pmMatKey && !materialStatus[i])
                {
                    materialStatus[i] = true;
                    // use matIdx to set RTPC/Param for your footstep manager.
                    matIdx = physicsToSound.matIntIdxValue;
                    Debug.Log("mat " + matIdx);
                }
                else if (material != physicsToSound.pmMatKey && materialStatus[i])
                {
                    materialStatus[i] = false;
                    Debug.LogWarning("You probably need to assing a PM material to the script ");
                }
            }
        }
    }

}


