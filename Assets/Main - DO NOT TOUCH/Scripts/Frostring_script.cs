using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frostring_script : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer sprite;
    float alpha = 1f;
    [SerializeField] float fading_away;
    public float scale_modifier;
    void Start()
    {
        transform.localScale += new Vector3(scale_modifier, scale_modifier, 0);
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        alpha -= fading_away;
        if (alpha < 0) Destroy(this.gameObject);
        sprite.color = new Color(1, 1, 1, alpha);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyStatusMAIN>().EnemyDeath();
            // make the enemy freeze
        }
    }
}
