using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hook : MonoBehaviour
{

    public Transform hookedTransform;

    private Camera mainCamera;
    private Collider2D coll;

    private int length;
    private int strength;
    private int fishCount;

    private bool canMove = false;

    private List<Fish> hookedFishes;

    private Tweener cameraTween;

    protected Joystick joystick;

    protected Rigidbody2D rigid;

    public float speed;

    [SerializeField]
    private AudioClip fishCatch;

    

    void Awake()
    {
        mainCamera = Camera.main;
        coll = GetComponent<Collider2D>();
        hookedFishes = new List<Fish>();
        joystick = FindObjectOfType<Joystick>();
        rigid = GetComponent<Rigidbody2D>();




    }

    void Start()
    {
        
    }

   
    void Update()
    {

        if (canMove)
        {
            rigid.velocity = new Vector3(joystick.Horizontal * speed, 0, 0);
        }
        else
        {
            rigid.velocity = new Vector3(0, 0, 0);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.15f, 4.0f), transform.position.y,transform.position.z);

    }

    public void startFishing()
    {
        
        

        transform.position = new Vector3(0, -3.12f, transform.position.z);
        length = IdleManager.instance.length - 20;
        strength = IdleManager.instance.strength;
        fishCount = 0;
        float time = (-length) * 0.1f;

        cameraTween = mainCamera.transform.DOMoveY(length, 1 + time * 0.25f, false).OnUpdate(delegate
          {
              if (mainCamera.transform.position.y <= -11)
              {
                  transform.SetParent(mainCamera.transform);
              }
          }).OnComplete(delegate
          {
              coll.enabled = true;
              cameraTween = mainCamera.transform.DOMoveY(0, time * 5, false).OnUpdate(delegate
              {
                  if (mainCamera.transform.position.y >= -25f)
                  {
                      stopFishing();
                  }
              });
          });

        GameManager.instance.changeScreen(Screens.GAME);
        coll.enabled = true;
        canMove = true;
        hookedFishes.Clear();

    }

    private void stopFishing()
    {
       
        canMove = false;
        cameraTween.Kill(false);
        cameraTween = mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y >= -11)
            {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -6);
            }
        }).OnComplete(delegate
        {
            transform.position = Vector2.down * 6;
            coll.enabled = true;
            int num = 0;
            for(int i = 0;i<hookedFishes.Count;i++)
            {
                hookedFishes[i].transform.SetParent(null);
                hookedFishes[i].resetFish();
                num += hookedFishes[i].Type.price;
            }
            IdleManager.instance.totalGain = num;
            GameManager.instance.changeScreen(Screens.END);
        });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if(other.CompareTag("Fish") && fishCount != strength)
        {
            AudioSource.PlayClipAtPoint(fishCatch, transform.position);
            fishCount++;
            hookedFishes.Add(other.GetComponent<Fish>());
            other.GetComponent<Fish>().hooked();
            other.transform.SetParent(transform);
            other.transform.position = hookedTransform.position;
            other.transform.rotation = hookedTransform.rotation;
            other.transform.localScale = Vector3.one;
            

            other.transform.DOShakeRotation(5, Vector3.forward * 45, 10, 90).SetLoops(1, LoopType.Yoyo).OnComplete(delegate
            {
                other.transform.rotation = Quaternion.identity;
            });
            if (fishCount == strength)
                stopFishing();
        }

    }


}
