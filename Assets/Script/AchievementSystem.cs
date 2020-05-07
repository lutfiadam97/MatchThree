using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSystem : Observer
{

    public Image achievementBanner;
    public Text achievementText;

    //Event
    TileEvent cookiesEvent, cakeEvent, vitaminEvent;

    void Start()
    {
        PlayerPrefs.DeleteAll();
        
        //Buat event
        cookiesEvent = new CookiesTileEvent(3);
        cakeEvent = new CakeTileEvent(10);
        vitaminEvent = new VitaminTileEvent(5);

        foreach (var poi in FindObjectsOfType<PointOfInterest>())
        {
            poi.RegisterObserver(this);
        }
    }

    public override void OnNotify(string value)
    {
        string key;
        
        //Seleksi event yang terjadi, dan panggil class event nya
        if(value.Equals("Cookies event"))
        {
            cookiesEvent.OnMatch();
            if (cookiesEvent.AchievementCompleted())
            {
                key = "Match first cookies";
                NotifyAchievement(key, value);
            }
        }

        if (value.Equals("Cake event"))
        {
            cakeEvent.OnMatch();
            if (cakeEvent.AchievementCompleted())
            {
                key = "Match 10 cake";
                NotifyAchievement(key, value);
            }
        }

        if (value.Equals("Vitamin event"))
        {

            vitaminEvent.OnMatch();
            if (vitaminEvent.AchievementCompleted())
            {
                key = "Match 5 vitamin";
                NotifyAchievement(key, value);
            }
        }          
    }

    void NotifyAchievement(string key, string value)
    {
        //check jika achievement sudah diperoleh
        if (PlayerPrefs.GetInt(value) == 1)
            return;

        PlayerPrefs.SetInt(value, 1);
        achievementText.text = key + " Unlocked !";
        
        //pop up notifikasi
        StartCoroutine(ShowAchievementBanner());
    }

    void ActivateAchievementBanner(bool active)
    {
        achievementBanner.gameObject.SetActive(active);
    }

    IEnumerator ShowAchievementBanner()
    {
        ActivateAchievementBanner(true);
        yield return new WaitForSeconds(2f);
        ActivateAchievementBanner(false);
    }
}
public class CookiesTileEvent: TileEvent
{
    private int matchCount;
    private int requiredAmount;
    
    public CookiesTileEvent(int amount)
    {
        requiredAmount = amount;
    }
    
    public override void OnMatch()
    {
        matchCount++;
    }
    
    public override bool AchievementCompleted()
    {
        if(matchCount == requiredAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
public class CakeTileEvent: TileEvent
{
    private int matchCount;
    private int requiredAmount;
    
    public CakeTileEvent(int amount)
    {
        requiredAmount = amount;
    }
    
    public override void OnMatch()
    {
        matchCount++;
    }
    
    public override bool AchievementCompleted()
    {
        if(matchCount == requiredAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
public class VitaminTileEvent: TileEvent
{
    private int matchCount;
    private int requiredAmount;
    
    public VitaminTileEvent(int amount)
    {
        requiredAmount = amount;
    }
    
    public override void OnMatch()
    {
        matchCount++;
    }
    
    public override bool AchievementCompleted()
    {
        if(matchCount == requiredAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}