using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Structure", menuName = "Structure")]
public class Structure : ScriptableObject
{
    //IMP - Store this info in structuremodel script
    public STRUCTURE_ID structureId;
    public string structureName;
    public string description;
    public STRUCTURE_CATEGORY structureCategory;
    public GameObject structurePrefab;

    public bool canCombine;

    public GameObject structureCombineOneSidePrefab;
    public GameObject structureCombineBothSidesPrefab;

    public float yHeight;   //IMP = For indicator scaling
    public bool canPlaceOnTop;

    public bool is45DegreeRotation;

    public int points;  //NOTE - Base points
    public Sprite structureIcon;
    
    //SECTION - Scoring
    public NearStructureScore[] nearStructureScores;
}
