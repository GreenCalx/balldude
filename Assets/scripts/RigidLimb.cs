using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

[System.Serializable]
public class RigidLimb
{
    public GameObject limbPrefab, limbBone;
    public SpriteMeshInstance limbBoneMesh;
    [HideInInspector]
    public SpriteRenderer limbPrefabRenderer;
    [Range(0,40)]
    public float detachForce, detachRotationForce;


}
