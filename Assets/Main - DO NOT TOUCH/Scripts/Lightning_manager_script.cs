using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_manager_script : MonoBehaviour
{
    [SerializeField] GameObject[] charges;
    [SerializeField] GameObject lightning_VFX;
    [SerializeField] GameObject lightning_killzone;
    [SerializeField] float lightning_intervals;
    [SerializeField] GameObject nearby_enemy_radar;
    List<GameObject> nearby_enemies = new List<GameObject>();

    public int lightning_count;
    public float score_to_compare;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ChargeCheck(float new_score)
    {
        if (new_score <= score_to_compare)
        {
            for (int a = 0; a < charges.Length; a++)
            {
                charges[a].SetActive(false);
            }

            return;
        }

        for (int a = 0; a < charges.Length; a++)
        {
            if (charges[a].activeSelf) continue;

            charges[a].SetActive(true);
            break;
        }

        if (charges[charges.Length - 1].activeSelf) StartCoroutine(Activate());
    }

    void SummonLightning(Vector3 target)
    {
        AudioManagerMAIN.instance.PlaySFX("LightningStrike");
        GameObject lightning = Instantiate(lightning_VFX, target, Quaternion.identity);
        lightning.transform.Rotate(-90, 0, 0);
        
    }

    IEnumerator Activate()
    {
        for (int a = 0; a < charges.Length; a++)
        {
            charges[a].SetActive(false);
        }

        for (int a = 0; a < lightning_count; a++)
        {
            Debug.Log("summon_lightning");

            float random_x_offset = Random.Range(-5.0f, 5.0f);
            float random_y_offset = Random.Range(-5.0f, 5.0f);

            Vector3 samurai_position = MainContextMAIN.player.transform.position;
            Vector3 lightning_summon_position = new Vector3(samurai_position.x + random_x_offset, samurai_position.y + random_y_offset + 14, 0);
            SummonLightning(lightning_summon_position);
            yield return new WaitForSeconds(0.0f);

            GameObject lightning_hit_zone = Instantiate(lightning_killzone, new Vector3(samurai_position.x + random_x_offset, samurai_position.y + random_y_offset, 0), Quaternion.identity);

            yield return new WaitForSeconds(lightning_intervals);

            Destroy(lightning_hit_zone);
        }

        /*    Code With targeting specific enemies
        nearby_enemy_radar.SetActive(true);
        yield return new WaitForSeconds(lightning_intervals);
        nearby_enemies = nearby_enemy_radar.GetComponent<Enemy_radar_script>().nearby_foes;
        Debug.Log(nearby_enemy_radar.GetComponent<Enemy_radar_script>().nearby_foes.Count);
        
        //nearby_enemy_radar.GetComponent<Enemy_radar_script>().nearby_foes.Clear();
        nearby_enemy_radar.SetActive(false);

        for (int a = 0; a < lightning_count; a++)
        {
            Debug.Log("Lightning_summoned");
            Debug.Log(nearby_enemies.Count);
            if (nearby_enemies.Count > 1)
            {
                
                int random_enemy_number = Random.Range(0, nearby_enemies.Count);
                SummonLightning(nearby_enemies[random_enemy_number].transform);
                nearby_enemies.Remove(nearby_enemies[random_enemy_number]);
                yield return new WaitForSeconds(lightning_intervals);
            }   
        }*/
    }

    private void FixedUpdate()
    {
        transform.position = MainContextMAIN.player.transform.position;
        transform.Translate(2, 0, 0);
    }
}
