using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileEvent
{
    //Abstract class untuk base event dari tile
    
    //Apa yang terjadi jika tile match
    public abstract void OnMatch();
    //Check jika persyaratn event telah terpenuhi
    public abstract bool AchievementCompleted();
}