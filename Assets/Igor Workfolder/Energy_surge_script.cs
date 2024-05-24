using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy_surge_script : MonoBehaviour
{
    [SerializeField] int lifetime;
    [SerializeField] ParticleSystem blast_VFX;
    [SerializeField] ParticleSystem VFX;
    public float scale_modifier = 1;
    int current_lifetime;
    // Start is called before the first frame update
    void Start()
    {
        IncreaseSize();
        current_lifetime = lifetime;
    }

    public void IncreaseSize()
    {
        transform.localScale += new Vector3(scale_modifier, scale_modifier, 0);
        blast_VFX.startSpeed += scale_modifier * 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        current_lifetime--;
        if (current_lifetime < 1) this.gameObject.SetActive(false);
    }

    public void ActivateSurge()
    {
        AudioManagerMAIN.instance.PlaySFX("EnergySurge");
        transform.position = MainContextMAIN.player.transform.position;
        VFX.Play();
        current_lifetime = lifetime;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyStatusMAIN>().EnemyDeath();
        }
    }
}
