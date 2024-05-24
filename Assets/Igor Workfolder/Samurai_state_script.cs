using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Samurai_state_script : MonoBehaviour
{

    public int max_hp;
    [SerializeField] GameObject game_over_text;
    public GameObject hp_bar; // make it connect to hpbarscript 
    public bool dead = false;
    public int current_hp;
    [SerializeField] SpriteRenderer sprite;
    bool invisibility = false;
    [SerializeField] int invisibility_time;
    [SerializeField] bool tummy_ache;
    [SerializeField] bool lung_cancer;
    public static GameObject player;

    // Start is called before the first frame update

    private void Awake()
    {
        hp_bar.GetComponent<Health_bar_script>().starting_hp = max_hp;
    }
    void Start()
    {
        current_hp = max_hp;
        player = this.gameObject;
        //TakeDamage(1);
    }

    public void Death()
    {
        //this.gameObject.SetActive(false);
        game_over_text.SetActive(true);
        dead = true;
        sprite.color = new Color(1.0f, 1.0f, 1.0f, 0f);
        StartCoroutine(GameOverScreen());
    }

    IEnumerator GameOverScreen()
    {
        AudioManager.instance.PlaySFX("GameOver");
        yield return new WaitForSeconds(4.0f);
        SceneManager.LoadScene("GameOverScene");
    }

    IEnumerator DamageAnimation(float waitTime)
    {
        //yield return new WaitForSeconds(waitTime);
        sprite.color = new Color(1.0f, 0, 0, 1.0f);
        invisibility = true;
        yield return new WaitForSeconds(waitTime);

        for (int a = 0; a < invisibility_time; a++)
        {
            yield return new WaitForSeconds(waitTime);
            sprite.color = (a % 2 == 0) ? new Color(1.0f, 1.0f, 1.0f, 0.3f) : new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        invisibility = false;
        sprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("damage_taken");
        if (invisibility || dead) return;

        hp_bar.GetComponent<Health_bar_script>().MinusHeart();

        current_hp -= damage;
        if (current_hp <= 0) Death();
        else
        {
            AudioManager.instance.PlaySFX("PlayerDamaged");
            StartCoroutine(DamageAnimation(0.3f));
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }

        if (lung_cancer && invisibility == false)
        {
            TakeDamage(1);
        }
        else if (tummy_ache && invisibility == false)
        {
            TakeDamage(0);
        }
    }
}
