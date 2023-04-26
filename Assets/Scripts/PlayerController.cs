using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movimentação")]
    [SerializeField] private float vel;
    [SerializeField] private float velRot;
    [SerializeField] private Animator anim;


    [Header("Inimigos")]
    public GameObject enemyTarget;
    [SerializeField] private List<GameObject> enemysOnTheBack;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject dropEnemyPrefab;
    [SerializeField] private Transform stackPosition;
    [SerializeField] private Rigidbody posRB;
    private Transform parentPickup;
    

    [Header("Gerenciamento")]
    public int obj = 0;
    public int limits = 3;
    public int money = 0;
    public int need = 10;
    bool drop = false;

    [Header("Info")]
    public TMP_Text moneyText;
    public TMP_Text needText;
    public TMP_Text contText;
    public Renderer myMaterial;

    // Start is called before the first frame update
    void Start()
    {
        moneyText.text = "MONEY: " + money.ToString();
        needText.text = "NEED: " + need.ToString();
        contText.text = enemysOnTheBack.Count.ToString() + "/" + limits.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Controller();
        Drop();
    }

    public void Controller()
    {
        float move = Input.GetAxis("Vertical") * vel;
        float rote = Input.GetAxis("Horizontal") * velRot;

        move *= Time.deltaTime;
        rote *= Time.deltaTime;

        transform.Rotate(0,rote,0);

        if(move > 0)
        {
            transform.Translate(0,0,move);
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }

        if(Input.GetButtonDown("Fire1"))
        {
            anim.Play("Punch");
            Damage();
        }
    }

    public void Damage()
    {
        if(enemyTarget && enemysOnTheBack.Count < limits)
        {
            enemyTarget.GetComponent<Enemy>().Golpe();
            enemyTarget = null;
            StartCoroutine(EnemyOnTheBack());
        }
    }


    IEnumerator EnemyOnTheBack()
    {   
        yield return new WaitForSeconds(2f);

        GameObject enemy = Instantiate(enemyPrefab);

        Transform enemyTransform = enemy.transform;
        enemysOnTheBack.Add(enemy);

        if(parentPickup == null)
        {
            parentPickup = enemyTransform;
            parentPickup.position = stackPosition.position;
            enemy.GetComponent<EnemyPrefab>().sj.connectedBody = posRB;
            enemy.GetComponent<EnemyPrefab>().sj.autoConfigureConnectedAnchor = false;
            contText.text = enemysOnTheBack.Count.ToString() + "/" + limits.ToString();
        }
        else
        {
            enemy.GetComponent<EnemyPrefab>().sj.connectedBody = enemysOnTheBack[obj].GetComponent<Rigidbody>();
            enemy.GetComponent<EnemyPrefab>().sj.autoConfigureConnectedAnchor = false;
            obj++;
            contText.text = enemysOnTheBack.Count.ToString() + "/" + limits.ToString();
            enemysOnTheBack[obj].transform.position = Vector3.up * 3;
            enemyTransform.position = stackPosition.position;
            Debug.Log("Opa! Aqui");
        }
    }

    public void Drop()
    {
        if(Input.GetKeyDown(KeyCode.Space) && drop)
        {
            foreach (GameObject enemy in enemysOnTheBack)
            {
                GameObject newEnemyPrefab = Instantiate(dropEnemyPrefab,enemy.transform.position,Quaternion.identity);
                newEnemyPrefab.GetComponent<Drop>().DropAddForce();
                money += 3;
                Destroy(enemy.gameObject);
                Debug.Log("Descarte");
            }

            enemysOnTheBack.Clear();
            obj = 0;
            
            moneyText.text = "MONEY: " + money.ToString();
            contText.text = enemysOnTheBack.Count.ToString() + "/" + limits.ToString();
        }
    }

    public void Upgrade()
    {
        if(money >= need)
        {
            limits += 2;
            money -= need;
            need += 2;
            moneyText.text = "MONEY: " + money.ToString();
            needText.text = "NEED: " + need.ToString();
            contText.text = enemysOnTheBack.Count.ToString() + "/" + limits.ToString();
            Debug.Log("UP! Limits now: " + limits.ToString());


            Color myCor = new Color(coloNumberConversion(Random.Range(0,255f)), coloNumberConversion(Random.Range(0,255f)), coloNumberConversion(Random.Range(0,255f)), 1f);
            myMaterial.material.SetColor("_Color", myCor);
        }
        else
        {
            Debug.Log("No Money");
        }
    }

    private float coloNumberConversion(float num) 
    {
        return num / 255.0f;
    }

    
    public void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
           enemyTarget = col.gameObject;
        }

        if(col.gameObject.CompareTag("Area"))
        {
            drop = true;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
           enemyTarget = null;
        }

        if(col.gameObject.CompareTag("Area"))
        {
            drop = false;
        }
    }
}
