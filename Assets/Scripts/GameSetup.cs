using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{

    public float playAreaScale = 25f;
    public int playAreaDivisions = 3;
    public Material playAreaMaterial;
    
    public int maxPickups = 20;

    public AudioClip pickupSound;
    public Material wallMaterial;
    public Material pickupMaterial;
    public Material pickupMaterial2;

    public float score = 0;
    float topScore = 0;
    public int level = 1;
    int collected = 0;
    int totalCollected = 0;

    Grid grid;

    GameObject playArea;
    Transform playerTransform;

    List<Material> walls = new List<Material>();

    float nextFall;

    enum LastCollected
    {
        None,
        Sphere,
        Capsule
    }

    LastCollected lastCollected = LastCollected.None;
    int pickupCombo = 0;

    public GameObject textPrefab;

    public Text levelText;
    public Text scoreText;

    // Start is called before the first frame update


    public void EndGame()
    {
        SaveLoad.SaveAttempt((int) score, totalCollected);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void Start()
    {
        ScoreData saved = SaveLoad.LoadAttempt();
        topScore = saved.score;

        scoreText.text = "Score: 0 (Best: " + topScore.ToString() + ")";

        playArea = GameObject.CreatePrimitive(PrimitiveType.Cube);
        playArea.transform.localPosition = new Vector3();
        playArea.transform.localScale = new Vector3(playAreaScale, 1, playAreaScale);
        playArea.GetComponent<Renderer>().material = playAreaMaterial;

        playerTransform = GameObject.Find("PLAYER").transform;

        grid = new Grid(Vector3.zero, playAreaScale / 2, playAreaDivisions);

        GameObject wall1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall1.transform.localScale = new Vector3(playAreaScale, 50, 3);
        wall1.transform.localPosition = new Vector3(0, wall1.transform.localScale.y/2, playAreaScale/2 + wall1.transform.localScale.z / 2);

        GameObject wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall2.transform.localScale = new Vector3(playAreaScale, 50, 3);
        wall2.transform.localPosition = new Vector3(0, wall1.transform.localScale.y / 2, -playAreaScale / 2 - wall1.transform.localScale.z / 2);

        GameObject wall3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall3.transform.localScale = new Vector3(3, 50, playAreaScale);
        wall3.transform.localPosition = new Vector3(-playAreaScale / 2 - wall3.transform.localScale.x / 2, wall3.transform.localScale.y / 2, 0);

        GameObject wall4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall4.transform.localScale = new Vector3(3, 50, playAreaScale);
        wall4.transform.localPosition = new Vector3(playAreaScale / 2 + wall3.transform.localScale.x / 2, wall3.transform.localScale.y / 2, 0);

        wall1.GetComponent<Renderer>().material = wallMaterial;
        wall2.GetComponent<Renderer>().material = wallMaterial;
        wall3.GetComponent<Renderer>().material = wallMaterial;
        wall4.GetComponent<Renderer>().material = wallMaterial;

        walls.Add(wall1.GetComponent<Renderer>().material);
        walls.Add(wall2.GetComponent<Renderer>().material);
        walls.Add(wall3.GetComponent<Renderer>().material);
        walls.Add(wall4.GetComponent<Renderer>().material);


        for (int i = 0; i < 20; i++)
        {
            //StartCoroutine(SpawnRandomBlockade(5f));
            SpawnPickup();
        }

        nextFall = Time.time + 5;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Material wall in walls)
        {
            Vector3 pp = playerTransform.position;
            Vector3 v = new Vector3(pp.x, pp.y, pp.z);
            wall.SetVector("_PlayerPos", v);
        }

        if (grid.AvailableSquares() > 0 && Time.time > nextFall)
        {
            nextFall = Time.time + Random.Range(0, 12 - 2 * level);
            SpawnRandomBlockade();
        }
    }

    void AdvanceLevel()
    {
        level++;

        if (level == 6)
        {
            EndGame();
        }

        levelText.text = "Level: " + level.ToString();
        playerTransform.localScale -= new Vector3(.1f, .1f, .1f);
        playerTransform.GetComponentInChildren<TrailRenderer>().startWidth -= 0.1f;
        playerTransform.GetComponent<Player>().maxSpeed *= 1.1f;
        //Camera.main.orthographicSize -= 1;
    }

    void SpawnRandomBlockade()
    {
        GameObject blockade = GameObject.CreatePrimitive(PrimitiveType.Cube);
        blockade.transform.localScale = new Vector3(1,1,1) * playAreaScale / playAreaDivisions;
        Vector3 spawnPosition = grid.GetCenterRandomSquare(true);
        blockade.transform.position = spawnPosition + Vector3.up * 20;
        var rb = blockade.AddComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.mass = 100;        

        float distance = (blockade.transform.position - playerTransform.position).magnitude;
        Camera.main.GetComponent<CameraShake>().AddDisplacement(3.0f / distance);

    }

    Color RandomColor()
    {
        Vector3 pos = Random.insideUnitSphere;

        return new Color(pos.x,pos.y,pos.z);
    }

    void SpawnPickup()
    {

        if (grid.AvailableSquares() == 0)
        {
            return;
        }

        float rand = Random.Range(0, 100);

        GameObject pickup;

        if (rand > 50)
        {
            pickup = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            pickup.AddComponent<ScorePickup>().sphere = false;
        }
        else
        {
            pickup = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pickup.AddComponent<ScorePickup>().sphere = true;
        }

        pickup.GetComponent<Collider>().isTrigger = true;
        pickup.transform.localScale *= 0.5f;
        

        pickup.transform.position = grid.GetRandomPointRandomSquare() + Vector3.up * 1;

        Color color = Random.ColorHSV(0f, 1, 0.9f, 1, 0.7f, 1);
        Material baseMat = new Material(pickupMaterial2);
        Material outline = new Material(pickupMaterial);
        baseMat.color = color;
        outline.SetColor("_OutlineColor", color);
        outline.SetColor("_Color", color);
        List<Material> mats = new List<Material>() { baseMat, outline };

        pickup.GetComponent<Renderer>().materials = mats.ToArray();
    }

    public void ScorePickupTouched(bool sphere, Color color, bool wasPlayer, Vector3 position)
    {
        AudioSource AS = GetComponent<AudioSource>();
        if (wasPlayer)
        {
            if (lastCollected == LastCollected.None)
            {
                if (sphere)
                {
                    lastCollected = LastCollected.Sphere;
                }
                else
                {
                    lastCollected = LastCollected.Capsule;
                }
            }
            else if (lastCollected == LastCollected.Sphere)
            {
                if (sphere)
                {
                    pickupCombo = 0;
                    AS.pitch = 1;
                    lastCollected = LastCollected.Sphere;
                }
                else
                {
                    if (AS.pitch < 2)
                    {
                        AS.pitch += .1f;
                    }
                    pickupCombo++;
                    lastCollected = LastCollected.Capsule;
                }
            }
            else if (lastCollected == LastCollected.Capsule)
            {
                if (sphere)
                {
                    if (AS.pitch < 2)
                    {
                        AS.pitch += .1f;
                    }
                    pickupCombo++;
                    lastCollected = LastCollected.Sphere;
                }
                else
                {
                    pickupCombo = 0;
                    AS.pitch = 1;
                    lastCollected = LastCollected.Capsule;
                }
            }
            AS.PlayOneShot(pickupSound);

            playerTransform.GetComponentInChildren<TrailRenderer>().startColor = color;
            playerTransform.GetComponent<Renderer>().materials[0].color = color;

            collected++;
            totalCollected++;

            GameObject text = Instantiate(textPrefab, null);
            text.GetComponentInChildren<TextAppear>().SetText("x" + pickupCombo.ToString(), color);
            text.transform.position = position + Vector3.up * 1;

            score += 5 * level * (1+pickupCombo);
            scoreText.text = "Score: " + score + " (Best: " + topScore + ")";
        }

        SpawnPickup();

        if (collected == 10)
        {
            collected = 0;
            AdvanceLevel();
        }
    }
}
