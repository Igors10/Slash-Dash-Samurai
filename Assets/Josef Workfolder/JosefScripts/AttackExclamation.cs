using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackExclamation : MonoBehaviour
{

    private GameObject playerRef;
    // Start is called before the first frame update
    void Start()
    {


        transform.SetParent(null);

        StartCoroutine(Disapear());
    }

    IEnumerator Disapear()
    {
        yield return new WaitForSeconds(.3f);
        Destroy(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
