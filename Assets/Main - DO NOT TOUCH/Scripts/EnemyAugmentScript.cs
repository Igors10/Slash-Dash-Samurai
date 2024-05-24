using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAugmentScript : MonoBehaviour
{
    List<string> AugmentProbability;
    int noAugment;
    int bubbleAugment;
    int explodeAugment;
    int levelCount;
    public string selectedAugment;
    // Start is called before the first frame update
    void Start()
    {
        AugmentProbability = new List<string>();
        levelCount = MainContextMAIN.LevelCount;
        SetProbability();
        SetPool();
        GetAugment();
        
    }

    void SetProbability()
    {
        noAugment = 10;

        if(levelCount > 3) 
        {
            bubbleAugment = 1 + levelCount/4;
            Mathf.Clamp(bubbleAugment, 0, 10);

            explodeAugment = 1 + levelCount / 4;
            Mathf.Clamp(explodeAugment, 0, 10);
        }
    }

    void SetPool()
    {
        for(int i = 0; i < noAugment; i++)
        {
            AugmentProbability.Add("0");
        }

        for (int i = 0; i < bubbleAugment; i++)
        {
            AugmentProbability.Add("1");
        }

        for (int i = 0; i < explodeAugment; i++)
        {
            AugmentProbability.Add("2");
        }
    }

    void GetAugment()
    {
        var augment =Random.Range(0, AugmentProbability.Count);

        selectedAugment = AugmentProbability[augment];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
