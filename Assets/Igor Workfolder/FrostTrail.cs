using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostTrail : MonoBehaviour
{
    public Queue<GameObject> DeactivatedFrostTrails = new Queue<GameObject>();
    [SerializeField] GameObject trail_prefab;
    public float melting_speed;
    public float size_modifier;
    public float lifetime;
    public float slowdown_effect;
    public float slowed_speed;
    public static GameObject frosttrail_manager;
    int frosttrail_timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        frosttrail_manager = this.gameObject;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        frosttrail_timer++; // For now it creates new trails all the time, but its here just in case we want to make it happen less often
        if (frosttrail_timer % 1 == 0 && MainContextMAIN.player.GetComponent<DashScriptMAIN>().IsDashing)
        {
            PutNewTrail(MainContextMAIN.player.transform);
        }
    }

    void PutNewTrail(Transform spawn_transform)
    {
        if (DeactivatedFrostTrails.Count < 1) Instantiate(trail_prefab, spawn_transform.position, Quaternion.identity);
        else
        {
            GameObject reused_frost_tile = DeactivatedFrostTrails.Dequeue();
            reused_frost_tile.SetActive(true);
            reused_frost_tile.GetComponent<FrostTrail_individual_script>().Reuse(spawn_transform);
        }
    }
}
