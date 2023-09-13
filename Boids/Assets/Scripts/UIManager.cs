using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public BoidManager boidManager;

    public void ToggleCohesion()
    {
        if (boidManager.cohesionOn)
        {
            boidManager.cohesionOn = false;
        }
        else if (!boidManager.cohesionOn)
        {
            boidManager.cohesionOn = true;
        }
        
        boidManager.ChangeAll();
    }
    
    public void ToggleAlignment()
    {
        if (boidManager.alignmentOn)
        {
            boidManager.alignmentOn = false;
        }
        else if (!boidManager.alignmentOn)
        {
            boidManager.alignmentOn = true;
        }
        
        boidManager.ChangeAll();
    }
    
    public void ToggleSeperation()
    {
        if (boidManager.seperationOn)
        {
            boidManager.seperationOn = false;
        }
        else if (!boidManager.seperationOn)
        {
            boidManager.seperationOn = true;
        }
        
        boidManager.ChangeAll();
    }  
    
    public void ToggleRacism()
    {
        if (boidManager.racismOn)
        {
            boidManager.racismOn = false;
        }
        else if (!boidManager.racismOn)
        {
            boidManager.racismOn = true;
        }
        
        boidManager.ChangeAll();
    }
}
