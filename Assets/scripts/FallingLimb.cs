using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingLimb : MonoBehaviour
{
    [Range(0,10)]
    public float fadeTime = 1.0f, timeUntilStart = 2.0f;
    Color color_a1 = new Color(1,1,1,1);
    Color color_a0 = new Color(0,0,0,0);

    public List<RigidLimb> rigidLimbList;

    [SerializeField]
    int currentLimb = 0;
    bool isActive = true;

    private void Start()
    {
        ListInitialize();
        currentLimb = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LooseNextLimb();
        }
    }

    private void LooseNextLimb()
    {
        if (isActive)
        {
            RigidLimb limb = rigidLimbList[currentLimb];

            limb.limbPrefabRenderer.color = color_a1;

            limb.limbPrefab.SetActive(true);
            limb.limbPrefab.transform.position = limb.limbBone.transform.position;
            limb.limbPrefab.transform.rotation = Quaternion.identity;

            limb.limbPrefab.GetComponent<Rigidbody2D>().AddForce( new Vector2(0, limb.detachForce), ForceMode2D.Impulse);
            limb.limbPrefab.GetComponent<Rigidbody2D>().AddTorque( Random.Range(-limb.detachRotationForce, limb.detachRotationForce),ForceMode2D.Impulse);

            limb.limbBoneMesh.color = color_a0;
            limb.limbPrefab.GetComponent<Limb>().StartCoroutine( limb.limbPrefab.GetComponent<Limb>().fadeTo(1.0f, fadeTime, timeUntilStart));

            currentLimb++;

            if (currentLimb >= rigidLimbList.Count)
            {
                isActive = false;
            }


        }
    }

    private void ListInitialize()
    {
        for (int i=0;i<rigidLimbList.Count;i++)
        {
            rigidLimbList[i].limbPrefabRenderer = rigidLimbList[i].limbPrefab.GetComponent<SpriteRenderer>();
            rigidLimbList[i].limbPrefab.SetActive(false);
        }
    }

}
