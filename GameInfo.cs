using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameInfo
{
    public string townName;
    public int level;   //IMP - For internal use only... will not be displayed
    public int points;
    public int pointsToNextLevel;
    public int pointsOfPreviousLevel;
    public int structuresLeft;
    public int residentialStructuresAvailable;
    public int natureStructuresAvailable;
    public int entertainmentStructuresAvailable;
    public int industryStructuresAvailable;
    
    public GameInfo(string townName, int level, int points, int structuresLeft, int residentialStructuresAvailable, int natureStructuresAvailable, int entertainmentStructuresAvailable, int industryStructuresAvailable){
        this.townName = townName;
        this.level = level;
        this.points = points;
        this.structuresLeft = structuresLeft;
        
        this.residentialStructuresAvailable = residentialStructuresAvailable;
        this.natureStructuresAvailable = natureStructuresAvailable;
        this.entertainmentStructuresAvailable = entertainmentStructuresAvailable;
        this.industryStructuresAvailable = industryStructuresAvailable;
        
        //IMP - Do the below in start game function
        //pointsToNextLevel = GameManager.instance.GetLevelPoints(1);
        //pointsOfPreviousLevel = 0;
    }
}
