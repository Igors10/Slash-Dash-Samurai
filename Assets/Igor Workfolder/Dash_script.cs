using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Dash_script : MonoBehaviour
{
    public GameObject slash_effect;
    [SerializeField] bool slash_mode;
    [SerializeField] bool ranged_mode;
    int touch_time;
    [SerializeField] Collider2D ranged_button_col;
    [SerializeField] GameObject slash;
    [SerializeField] GameObject curve_slash_center;
    [SerializeField] GameObject curve_slash_border;

    //     ** Path variables **
    [SerializeField] GameObject[] target_point;
    public float vector_length_modifier;
    Vector2 target_position;
    Vector2 starting_position = new Vector2(0, 0);
    Vector3 movement_starting_position;
    [SerializeField] float short_dash_cap;
    [SerializeField] float long_dash_max;
    [SerializeField] float short_dash_max;
    float cool_t = 0;
    [SerializeField] float t = 0;

    //     ** Bezier Curve variables **

    [SerializeField] GameObject[] curve_points;
    [SerializeField] float curve_radius;
    [SerializeField] LineRenderer line;


    // -------------------------------------

    //     ** Movement variables **  
    public Vector3 move_to_position;
    [SerializeField] float move_speed;
    public float NonTimeSpeed;
    bool dashing;
    [SerializeField] bool centered_swipe;
    [SerializeField] bool curve_movement;
    [SerializeField] bool alt_movement;
    bool curve;
    Vector3 curve_control_position;
    int tap_time = 3;



    // Start is called before the first frame update
    void Start()
    {
        movement_starting_position = transform.position;
        touch_time = 10;

        if (alt_movement)
        {
            //short_dash_cap += 1;
            //long_dash_max += 2;
            short_dash_max += 1 ;
        }
    }

    public void UpdateDistance(float modifier)
    {
        short_dash_max /= vector_length_modifier;
        //short_dash_cap /= vector_length_modifier;
        long_dash_max /= vector_length_modifier;

        vector_length_modifier += modifier;

        short_dash_max *= vector_length_modifier;
        //short_dash_cap *= vector_length_modifier;
        long_dash_max *= vector_length_modifier;
    }
    void AltDashTrajectory(Vector2 target_position)
    {
        //  * Target point movement *

        if (Vector2.Distance(starting_position, new Vector2(target_position.x + starting_position.x, target_position.y + starting_position.y)) > short_dash_cap)
        {
            curve_slash_center.SetActive(false);
            slash.SetActive(true);
            curve = false;

            target_position = Vector3.ClampMagnitude(target_position, long_dash_max);

            for (int a = 0; a < target_point.Length; a++)
            {

                target_point[a].transform.position = new Vector2(transform.position.x + target_position.x * vector_length_modifier * (1.0f - 1.0f / target_point.Length * a), transform.position.y + target_position.y * vector_length_modifier * (1.0f - 1.0f / target_point.Length * a));

            }

            slash.transform.position = target_point[0].transform.position;
            slash.transform.Translate(0, 1, 0);

        }
        else
        {
            target_position = Vector3.ClampMagnitude(target_position, short_dash_max);
            // _________Setting bezier curve points___________
            curve = true;
            curve_points[0].transform.position = transform.position;
            curve_points[2].transform.position = new Vector3(transform.position.x + target_position.x, transform.position.y + target_position.y, curve_points[1].transform.position.z);
            curve_points[1].transform.position = new Vector3(transform.position.x + target_position.x / 2, transform.position.y + target_position.y / 2, curve_points[2].transform.position.z);
            curve_points[1].transform.Translate(curve_radius, 0, 0);

            curve_control_position = curve_points[1].transform.position;

            // Slash hit zone 

            //slash.transform.position = curve_points[1].transform.position;
            //slash.transform.Translate(curve_radius * -1.5f, 0, 0);

            curve_slash_center.SetActive(true);
            slash.SetActive(false);
            curve_slash_center.transform.position = new Vector3(transform.position.x + target_position.x / 2, transform.position.y + target_position.y / 2, curve_points[2].transform.position.z);
            curve_slash_center.transform.localScale = new Vector3(0.7f, Vector2.Distance(starting_position, new Vector2(target_position.x + starting_position.x, target_position.y + starting_position.y)) - 3, 1);

            // ________________________________________________

            // Bezier curve formula
            // (B(t) = (1 - t)^2 * P0 + 2(1 - t) * t * P1 + t^2 * P2)

            for (int a = 0; a < target_point.Length; a++)
            {
                float t = 1.0f - 1.0f / target_point.Length * a;
                // 1.0f / target_point.Length * a
                target_point[a].transform.position = Mathf.Pow(1 - t, 2) * curve_points[0].transform.position + 2 * (1 - t) * t * curve_control_position
                    + Mathf.Pow(t, 2) * curve_points[2].transform.position;
            }
           
        }


        // -----------------------

        //  * Rotation *

        float rotation_angle = Mathf.Atan2(target_point[0].transform.position.x - transform.position.x, target_point[0].transform.position.y - transform.position.y) * Mathf.Rad2Deg * -1;

        transform.rotation = Quaternion.AngleAxis(rotation_angle, Vector3.forward);

        // ------------------------
    }
    void DashTrajectory(Vector2 target_position)
    {
        //  * Target point movement *

        if (curve_movement && Vector2.Distance(starting_position, new Vector2(target_position.x + starting_position.x, target_position.y + starting_position.y)) > short_dash_cap)
        {
            target_position = Vector3.ClampMagnitude(target_position, long_dash_max);
            // _________Setting bezier curve points___________
            curve = true;
            curve_points[0].transform.position = transform.position;
            curve_points[2].transform.position = new Vector3(transform.position.x + target_position.x, transform.position.y + target_position.y, curve_points[1].transform.position.z);
            curve_points[1].transform.position = new Vector3(transform.position.x + target_position.x / 2, transform.position.y + target_position.y / 2, curve_points[2].transform.position.z);
            curve_points[1].transform.Translate(curve_radius, 0, 0);

            curve_control_position = curve_points[1].transform.position;

            // Slash hit zone 

            //slash.transform.position = curve_points[1].transform.position;
            //slash.transform.Translate(curve_radius * -1.5f, 0, 0);

            curve_slash_center.SetActive(true);
            slash.SetActive(false);
            curve_slash_center.transform.position = new Vector3(transform.position.x + target_position.x / 2, transform.position.y + target_position.y / 2, curve_points[2].transform.position.z);
            curve_slash_center.transform.localScale = new Vector3(0.7f, Vector2.Distance(starting_position, new Vector2(target_position.x + starting_position.x, target_position.y + starting_position.y)) - 3, 1);

            // ________________________________________________

            // Bezier curve formula
            // (B(t) = (1 - t)^2 * P0 + 2(1 - t) * t * P1 + t^2 * P2)

            for (int a = 0; a < target_point.Length; a++)
            {
                float t = 1.0f - 1.0f / target_point.Length * a;
                // 1.0f / target_point.Length * a
                target_point[a].transform.position = Mathf.Pow(1 - t, 2) * curve_points[0].transform.position + 2 * (1 - t) * t * curve_control_position
                    + Mathf.Pow(t, 2) * curve_points[2].transform.position;
            }

        }
        else
        {
            curve_slash_center.SetActive(false);
            slash.SetActive(true);
            curve = false;

            target_position = Vector3.ClampMagnitude(target_position, short_dash_max);

            for (int a = 0; a < target_point.Length; a++)
            {

                target_point[a].transform.position = new Vector2(transform.position.x + target_position.x * vector_length_modifier * (1.0f - 1.0f / target_point.Length * a), transform.position.y + target_position.y * vector_length_modifier * (1.0f - 1.0f / target_point.Length * a));

            }

            slash.transform.position = target_point[0].transform.position;
            slash.transform.Translate(0, 1, 0);
        }


        // -----------------------

        //  * Rotation *

        float rotation_angle = Mathf.Atan2(target_point[0].transform.position.x - transform.position.x, target_point[0].transform.position.y - transform.position.y) * Mathf.Rad2Deg * -1;

        transform.rotation = Quaternion.AngleAxis(rotation_angle, Vector3.forward);

        // ------------------------
    }

    void Aim()
    {

    }

    void ActivatePath(bool active)
    {
        slash.SetActive(active);

        for (int a = 0; a < target_point.Length; a++)
        {
            //target_point[a].transform.position = transform.position;
            target_point[a].SetActive(active);
        }
    }

    void BeginDash(Vector2 target_position, bool curve)
    {
        dashing = true;

        AudioManager.instance.PlaySFX("PlayerSlash");

        move_to_position = target_point[0].transform.position;

        slash.SetActive(false);
        curve_slash_center.SetActive(false);

        t = 0;
    }

    void PlaySlashAnimation()
    {
        GameObject SlashSFX;
        if (curve)
        {
            Vector3 curve_slash_position = Vector3.Lerp(starting_position, transform.position, 0.7f);
            SlashSFX = Instantiate(slash_effect, curve_slash_position, transform.rotation); ; // make a function for spawning this effect
            SlashSFX.transform.Rotate(0f, 0f, 90.0f);
        }
        else
        {
            SlashSFX = Instantiate(slash_effect, transform.position, transform.rotation); // make a function for spawning this effect
        }
    }

    void EndDash()
    {
        if (dashing) PlaySlashAnimation();
        dashing = false;
        transform.position = move_to_position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // --------------Movement-------------------

        if ((Vector3.Distance(move_to_position, transform.position) > 0.0001f) && (dashing))
        {
            t += Time.deltaTime;

            //float SlowDownCoefficient = 5.0f;
            float SlowDownCoefficient = 1.0f;
            float Distance = Vector3.Distance(movement_starting_position, move_to_position);
            float Progress = t / ((Distance * SlowDownCoefficient) / NonTimeSpeed);

            Debug.Log(Progress);

            if (true) { cool_t = Mathf.Pow(Progress, 2); }
            /*else 
                { 
                if (Progress > 0.2f) 
                    {
                    cool_t = -0.1f * (((Progress - 0.0f) * (Progress - 0.1f) * (Progress - 0.2f) * (Progress - 1.0f)) /
                        ((0.05f - 0.0f) * (0.05f - 0.1f) * (0.05f - 0.2f) * (0.05f - 1.0f))) +
                        0.5f * (((Progress - 0.0f) * (Progress - 0.1f) * (Progress - 0.05f) * (Progress - 1.0f)) /
                        ((0.2f - 0.0f) * (0.2f - 0.1f) * (0.2f - 0.05f) * (0.2f - 1.0f))) +
                        1.0f * (((Progress - 0.0f) * (Progress - 0.1f) * (Progress - 0.05f) * (Progress - 0.2f)) /
                        ((1f - 0.0f) * (1f - 0.1f) * (1.0f - 0.05f) * (1.0f - 0.2f)));
                    }  
                else
                    {
                    float a = (0.5f - 1.0f) / (Mathf.Log(0.2f / 1.0f));
                    float b = Mathf.Exp((1.0f*Mathf.Log(0.2f) - 0.5f*Mathf.Log(1.0f))/
                        (0.5f - 1.0f));
                    cool_t = a*Mathf.Log(b*Progress);
                    }
                
                }*/

            //Debug.Log(Distance/NonTimeSpeed);

            if (curve)
            {
                // Curve movement

                // (B(t) = (1 - t)^2 * P0 + 2(1 - t) * t * P1 + t^2 * P2)

                if (cool_t < 1) transform.position = Mathf.Pow(1 - cool_t, 2) * movement_starting_position + 2 * (1 - cool_t) * cool_t * curve_control_position
                    + Mathf.Pow(cool_t, 2) * move_to_position;

                
                else if (cool_t > 0.95 && cool_t < 9.51)
                {
                    HelperFunctions.CheckCollisionCurve(movement_starting_position, move_to_position, transform.position, 1.0f);


                    EndDash();
                }

                // Curve rotation
                float rotation_angle = Mathf.Atan2(curve_slash_center.transform.position.x - transform.position.x, curve_slash_center.transform.position.y - transform.position.y) * Mathf.Rad2Deg * -1;

                if (dashing) transform.rotation = Quaternion.AngleAxis(rotation_angle, Vector3.forward);
            }
            else
            {
                // Line movement
               
                transform.position = Vector3.Lerp(movement_starting_position, move_to_position, cool_t);

                if (cool_t > 0.95 && cool_t < 9.51)
                {
                    HelperFunctions.CheckCollisionLine(movement_starting_position, move_to_position, 0.5f, new Vector2(3.0f, 1.5f), target_position);
                    
                    EndDash();
                }
            }
            
        }
        

        // -----------------------------------------
        
        // _________________ ** Touch Input ** _____________________
        if (Input.touchCount > 0 && dashing == false && GetComponent<Samurai_state_script>().dead == false)
        {

            Touch touch = Input.GetTouch(0);
            Vector2 touch_position = Camera.main.ScreenToWorldPoint(touch.position);
            
            

            if (touch.phase == TouchPhase.Began)
            {
                movement_starting_position = transform.position;
                touch_time = 0;
               

                Collider2D touched_collider = Physics2D.OverlapPoint(touch_position);
                starting_position = (centered_swipe) ? transform.position : touch_position;

                if (ranged_button_col == touched_collider) ranged_mode = true;
                else
                {
                    slash_mode = true;
                    ActivatePath(true);
                }

            }
            if (touch.phase == TouchPhase.Moved)
            {
                touch_time++;
                target_position = new Vector2(starting_position.x - touch_position.x, starting_position.y - touch_position.y);
                if (slash_mode && alt_movement) AltDashTrajectory(target_position);
                else if (slash_mode) DashTrajectory(target_position);
                else if (ranged_mode) Aim();
            }
            if (touch.phase == TouchPhase.Ended)
            {
                Collider2D touched_collider = Physics2D.OverlapPoint(touch_position);

                if (ranged_button_col == touched_collider) ranged_mode = false;
                
            }
        }
        else if (slash_mode)
        {
            //Camera.main.GetComponent<Camera_movement_script>().FocusOnSamurai(); // && touch_time > 10
            slash_mode = false;
            if (touch_time > tap_time) BeginDash(target_position, false);
            ActivatePath(false);
        }
        // ______________________________________________________________________
    }

}
