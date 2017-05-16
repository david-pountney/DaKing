using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpawnCoins : MonoBehaviour
{
    public int coinsToDrop;
    public float startX = 1f;
    public float endX = 5f;
    public float timeBetweenCoinAdding;
    public float timeBetweenCoinRemoval;

    public float floor = 1;

    public GameObject coin;

    public List<Vector2> _listOfPositions;
    private PlayerAttributesLogic playerAttributes;

    private float gapX;
    private float gapY;

    private GameObject treasure;

    // Use this for initialization
    void Start()
    {
        playerAttributes = GlobalReferencesBehaviour.instance.SceneData.playerAttributes.PlayerAttributesLogic;
            
        _listOfPositions = new List<Vector2>();

        gapX = coin.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        gapY = coin.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        float rt = gapX;
        float rh = gapY;

        addToList();

        startDroppingCoins(coinsToDrop);
    }

    public void updateCoins(int amount)
    {
        if (amount < 0)
        {
            StartCoroutine(removeCoins(amount));
        }
        else
        {
            //Debug.Log(gameObject.name);
            //Debug.Log(this.name);
            StartCoroutine(dropCoins(amount));
        }
    }


    public void startDroppingCoins(int amount)
    {

        StartCoroutine(dropCoins(amount));

    }

    public void startRemovingCoins(int amount)
    {
        StartCoroutine(removeCoins(amount));
    }

    private void addToList()
    {
        float currentX = startX;

        while (currentX < endX)
        {
            _listOfPositions.Add(new Vector2(currentX, floor));
            currentX += gapX;
        }
    }

    private IEnumerator dropCoins(int amount)
    {
        GameObject clone;

        Vector2 coinPos = Vector2.zero;
        float randX;
        int randNumber = 0;
        float currentFloor = floor;
        int currentCoin = coinsToDrop;

        treasure = GameObject.Find("TreasureChest");

        if (amount != 0)
        {
            for (int i = 0; i < amount; ++i)
            {
                if (_listOfPositions.Count == 0)
                {
                    addToList();
                    currentFloor += gapY;
                }

                randNumber = UnityEngine.Random.Range(0, _listOfPositions.Count);
                randX = _listOfPositions[randNumber].x;
                _listOfPositions.RemoveAt(randNumber);

                clone = Instantiate(coin, new Vector2(transform.localPosition.x + randX, transform.localPosition.y + 10), Quaternion.identity) as GameObject;

                clone.transform.parent = treasure.transform;

                randX = UnityEngine.Random.Range(-5, 5);

                Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(randX, 0));

                yield return new WaitForSeconds(timeBetweenCoinAdding);
            }
        }
    }

    private IEnumerator removeCoins(int amount)
    {
        GameObject coin;

        int flip = Math.Abs(amount);

        for (int i = 0; i < flip; ++i)
        {
            coin = GameObject.FindGameObjectWithTag("Coin");
            DestroyObject(coin);


            yield return new WaitForSeconds(timeBetweenCoinRemoval);
        }
    }

}
