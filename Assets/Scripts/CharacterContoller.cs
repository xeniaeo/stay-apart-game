using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterContoller : MonoBehaviour
{   
    public bool selected = false;
    public int targetItemID = -1;

    private float _controlSpeed = 4.5f;
    private float _startPosX;
    private float _startPosY;
    private float _refPosX;
    private float _refPosY;
    private float _randomSpeed;
    private Vector3 _randomDirection;
    private bool _isLeaving = false;
    private float _leaveSpeed = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        _startPosX = this.transform.position.x;
        _refPosX = _startPosX;
        _startPosY = this.transform.position.y;
        _refPosY = _startPosY;
        Randomize();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected && Input.GetKey(KeyCode.W))
        {
            if (this.transform.position.y > 2.5f)
                this.transform.Translate(Vector3.zero * _controlSpeed * Time.deltaTime, Space.World);
            else
                this.transform.Translate(Vector3.up * _controlSpeed * Time.deltaTime, Space.World);

        }
        else if (selected && Input.GetKey(KeyCode.A))
        {
            if (this.transform.position.x < -8.0f)
                this.transform.Translate(Vector3.zero * _controlSpeed * Time.deltaTime, Space.World);
            else
                this.transform.Translate(Vector3.left * _controlSpeed * Time.deltaTime, Space.World);
        }
        else if (selected && Input.GetKey(KeyCode.S))
        {
            if (this.transform.position.y < -3.0f)
                this.transform.Translate(Vector3.zero * _controlSpeed * Time.deltaTime, Space.World);
            else
                this.transform.Translate(Vector3.down * _controlSpeed * Time.deltaTime, Space.World);
        }
        else if (selected && Input.GetKey(KeyCode.D))
        {
            if (this.transform.position.x > 8.0f)
                this.transform.Translate(Vector3.zero * _controlSpeed * Time.deltaTime, Space.World);
            else
                this.transform.Translate(Vector3.right * _controlSpeed * Time.deltaTime, Space.World);
        }

        if (!_isLeaving)
        {
            // Move character randomly
            this.transform.Translate(_randomDirection * _randomSpeed * Time.deltaTime, Space.World);

            // Limit character movement in small area
            if (Mathf.Abs(this.transform.position.x - _refPosX) > 1.0f || Mathf.Abs(this.transform.position.y - _refPosY) > 1.0f)
            {
                Randomize();
            }

            // When character is about to move out of the screen, make it move opposite direction
            if (Mathf.Abs(this.transform.position.x) > 8.0f || Mathf.Abs(this.transform.position.y) > 2.5f)
            {
                MoveOppositeFromScreenEdge();
            }

            // Pick up item
            if (selected && Input.GetKey(KeyCode.Space))
            {
                this.transform.GetChild(0).GetComponent<CollisionDetection>().PickUpItem(targetItemID);
            }
        }
        else
        {
            if (this.transform.position.x >= 5.0f)
                this.transform.Translate(Vector3.right * _leaveSpeed * Time.deltaTime, Space.World);
            else if (this.transform.position.x <= -5.0f)
                this.transform.Translate(Vector3.left * _leaveSpeed * Time.deltaTime, Space.World);
            else if (this.transform.position.y >= 0.0f)
                this.transform.Translate(Vector3.up * _leaveSpeed * Time.deltaTime, Space.World);
            else
                this.transform.Translate(Vector3.down * _leaveSpeed * Time.deltaTime, Space.World);
        }
    }

    private void DrawRadiusCircle()
    {
        CircleCollider2D childCollider = this.transform.GetChild(0).GetComponent<CircleCollider2D>();
        //Handles.color = Color.red;
        //Handles.DrawWireDisc(this.transform.position, new Vector3(0, 1, 0), childCollider.radius);
    }

    private void Randomize()
    {
        _refPosX = this.transform.position.x;
        _refPosY = this.transform.position.y;
        _randomSpeed = Random.Range(0.3f, 1.0f);
        _randomDirection = new Vector3(Random.value, Random.value, Random.value);
    }

    private void MoveOppositeFromScreenEdge()
    {
        _refPosX = this.transform.position.x;
        _refPosY = this.transform.position.y;

        if (this.transform.position.x < -8.0f)
        {
            _randomSpeed = Random.Range(0.3f, 1.0f);
            _randomDirection = new Vector3(Random.Range(0.0f, 1.0f), Random.value, Random.value);
        }
        if (this.transform.position.x > 8.0f)
        {
            _randomSpeed = Random.Range(-1.0f, -0.3f);
            _randomDirection = new Vector3(Random.Range(0.0f, 1.0f), Random.value, Random.value);
        }
        if (this.transform.position.y < -3.0f)
        {
            _randomSpeed = Random.Range(0.3f, 1.0f);
            _randomDirection = new Vector3(Random.value, Random.Range(0.0f, 1.0f), Random.value);
        }
        if (this.transform.position.y > 2.6f)
        {
            _randomSpeed = Random.Range(-1.0f, -0.3f);
            _randomDirection = new Vector3(Random.value, Random.Range(0.0f, 1.0f), Random.value);
        }
    }

    private void GetScreenSize()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.transform.position.z);
        Debug.Log("screenCenter " + screenCenter);

        Vector3 screenHeight = new Vector3(Screen.width / 2, Screen.height, Camera.main.transform.position.z);
        Debug.Log("screenHeight " + screenHeight);

        Vector3 screenWidth = new Vector3(Screen.width, Screen.height / 2, Camera.main.transform.position.z);
        Debug.Log("screenWidth " + screenWidth);
    }

    public void AssignItemToCharacter(int itemID)
    {
        this.transform.Find("ItemNeeded/ItemSpriteHolder").GetComponent<SpriteRenderer>().sprite = ItemManager.Instance.itemSprites[itemID];
        targetItemID = itemID;
    }

    public void RetrieveItem(bool correctItem)
    {
        if (correctItem)
        {
            targetItemID = -1;
            _isLeaving = true;
            GameManager.Instance.itemRetrievedCount++;
            GameManager.Instance.AddScore(100);
            UIManager.Instance.PostToScoreUpdate("+100", 1.0f);
            AudioManager.Instance.PlayPickUpSound();
        }
        else
        {
            StartCoroutine(ShowExclamationMark());
        }
    }

    IEnumerator ShowExclamationMark()
    {
        this.transform.Find("SelectedIcon").GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        this.transform.Find("SelectedIcon").GetComponent<SpriteRenderer>().enabled = false;
    }
}
