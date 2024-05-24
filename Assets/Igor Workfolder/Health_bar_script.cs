using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_bar_script : MonoBehaviour
{
    public int starting_hp;
    [SerializeField] int current_hp;
    [SerializeField] Sprite full_heart;
    [SerializeField] Sprite black_heart;
    [SerializeField] Sprite empty_heart;
    [SerializeField] Sprite white_heart;
    [SerializeField] GameObject first_heart;
    //[SerializeField]
    //[SerializeField] Vector3 hp_position;
    //[SerializeField] GameObject[] hearts;
    List<GameObject> hearts = new List<GameObject>();

    // Debug bools (buttons)
    [SerializeField] bool plus_heart;
    [SerializeField] bool minus_heart;
    [SerializeField] bool heal_heart;

    [SerializeField] int blinking_time;
    [SerializeField] float blinking_speed;

    // Start is called before the first frame update
    void Start()
    {
        current_hp = 1;

        hearts.Add(first_heart);

        for (int a = 1; a < starting_hp; a++)
        {
            PlusHeart();
        }
    }

    IEnumerator BlinkingHeart(GameObject heart, Sprite new_sprite)
    {
        for (int a = 0; a < blinking_time; a++)
        {
            heart.GetComponent<Image>().sprite = (heart.GetComponent<Image>().sprite == full_heart) ? new_sprite : full_heart;
            yield return new WaitForSeconds(blinking_speed);
        }

        heart.GetComponent<Image>().sprite = (new_sprite == black_heart) ? empty_heart : full_heart;

    }
    public void MinusHeart()
    {
        if (current_hp == 0) return;
        current_hp--;
        StartCoroutine(BlinkingHeart(hearts[hearts.Count - current_hp - 1], black_heart));
    }

    public void PlusHeart()
    {
        current_hp++;
        GameObject new_heart = Instantiate(hearts[hearts.Count - 1], hearts[hearts.Count - 1].transform.position, Quaternion.identity, this.gameObject.transform);
        hearts.Add(new_heart);
        for (int b = 0; b < hearts.Count - 1; b++)
        {
            hearts[b].transform.Translate(120, 0, 0);
        }
    }

    public void HealHeart()
    {
        if (current_hp == hearts.Count) return;
        current_hp++;
        StartCoroutine(BlinkingHeart(hearts[hearts.Count - current_hp], white_heart));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Temporary input
        if (plus_heart)
        {
            PlusHeart();
            plus_heart = false;
        }

        if (minus_heart)
        {
            MinusHeart();
            minus_heart = false;
        }

        if (heal_heart)
        {
            HealHeart();
            heal_heart = false;
        }

    }
}
