using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnManager : MonoBehaviour {

    public GameObject singleBlockPrefab;
    public GameObject rowBlockPrefab, specialRowPrefab;
    public GameObject sensorPrefab;
    public GameObject dotPrefab;
    public GameObject specialBlockPrefab;
    public GameObject doubleWallPrefab;
    public GameObject singleWallPrefab;
    private GameObject sensor;

    private float _currentOdds = 1.0f;
    private float _wallOdds = 0.0f;
    private bool _canSpawn = true;
    private float[] _yPositions = new float[] {5.75f,7.0f,8.25f,9.5f,10.75f,12.0f,13.25f,14.5f,15.75f,17.0f,18.25f,19.50f,20.75f,22.0f,23.25f,24.50f,25.75f,27.0f,28.25f,29.50f};
    private float[] _xPositions = new float[] { -2.24f, -1.125f, 0, 1.125f, 2.24f };
    private float[] _yWallPositions = new float[] {6.375f,7.625f,8.875f,10.125f,11.375f,12.625f,13.875f,15.125f,16.375f,17.625f,18.875f,20.125f,21.375f,22.625f,23.875f,25.125f,26.375f,27.625f,28.875f};
    private float[] _xWallPositions = new float[] {-1.65f,-0.6f,0.6f,1.65f};
    private int _currentYPos;
    private bool _tempFirstTime = true;
    private bool _nextIsSpecial = false;
    private bool _isPoweredUp = false;
    private int _lastXWallPosition = -1;

    private void LateUpdate()
    {

        if (_canSpawn && GameManager.Instance.IsGameOver() == false)
        {
            _canSpawn = false;
            _currentYPos = 0;
            int rand1 = Mathf.RoundToInt(Random.Range(2.0f, 6.0f));
            int rand2;
            if (GameManager.Instance.GetPoweredUp())
            {
                rand2 = 21;
            } else
            {
                rand2 = Mathf.RoundToInt(Random.Range(2.0f, 6.0f));
            }
            if (GameManager.Instance.IsFirstTime())
            {
                GameManager.Instance.SetFirstTime(false);
                rand1 = 0;
            }
            for(int i = 0; i < 20; i++)
            {
                if (i == rand1)
                {
                    SpawnBlockRow();
                    _currentOdds = 0;
                    _currentYPos++;
                }
                else if (i == rand1-1)
                {
                    _currentYPos++;
                }
                else if(i == rand2)
                {
                    _nextIsSpecial = true;
                    SingleBlockSpawnRoutine();
                    _currentYPos++;
                }
                else
                {
                    SingleBlockSpawnRoutine();
                    _currentYPos++;
                }


            }
            sensor = Instantiate(sensorPrefab, new Vector2(0,29.50f), Quaternion.identity);
        } else if (GameManager.Instance.IsGameOver() == false)
        {
            if (sensor != null && sensor.transform.position.y <= 4.5f)
            {
                Destroy(sensor.gameObject);
                _canSpawn = true;
            }
            
        }


    }

    private void SingleBlockSpawnRoutine()
    {
        float randDot = Random.Range(0.0f,1.0f);

        float rand = Random.Range(0.0f, 1.0f);
        ArrayList usedXPos = new ArrayList();
        ArrayList usedWallXPos = new ArrayList();
        if (rand <= _currentOdds)
        {
            int randLine = Mathf.RoundToInt(Random.Range(1.0f,2.0f));
            int lastSpawnedXPosition = -2;
            for (int i = 0; i<randLine; i++)
            {
                int randX = Mathf.RoundToInt(Random.Range(0.0f, 4.0f));
                while (randX == lastSpawnedXPosition || randX == lastSpawnedXPosition-1 || randX == lastSpawnedXPosition + 1)
                {
                    randX = Mathf.RoundToInt(Random.Range(0.0f, 4.0f));
                    
                }
                if (_nextIsSpecial)
                {
                    Instantiate(specialBlockPrefab, new Vector2(_xPositions[randX], GameManager.Instance.headNode.position.y + _yPositions[_currentYPos]), Quaternion.identity);
                    _nextIsSpecial = false;
                }
                else
                {
                    Instantiate(singleBlockPrefab, new Vector2(_xPositions[randX], GameManager.Instance.headNode.position.y + _yPositions[_currentYPos]), Quaternion.identity);
                }
                lastSpawnedXPosition = randX;
                usedXPos.Add(randX);
            }
            _currentOdds = 0.0f;
            
        }
        else
        {
            float numbersOfWalls = Random.Range(0,1.0f);
            float chanceOfWallSpawn = Random.Range(0,1.0f);

            if (chanceOfWallSpawn <= 0.65f)
            {
                if (numbersOfWalls >= 0.4f)
                {
                    if (numbersOfWalls >= 0.6f && _lastXWallPosition>-1)
                    {
                        Instantiate(singleWallPrefab, new Vector2(_xWallPositions[_lastXWallPosition], _yPositions[_currentYPos]), Quaternion.identity);
                        _lastXWallPosition = -1;
                    } else
                    {
                        int randWall = Mathf.RoundToInt(Random.Range(0, 4));
                        Instantiate(singleWallPrefab, new Vector2(_xWallPositions[randWall], _yPositions[_currentYPos]), Quaternion.identity);
                        _lastXWallPosition = randWall;
                    }
                    
                    }
                else if (numbersOfWalls >= 0.1f)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int randWall = Mathf.RoundToInt(Random.Range(0, 3));
                        while (usedWallXPos.Contains(randWall))
                        {
                            randWall = Mathf.RoundToInt(Random.Range(0, 3));
                        }
                        usedWallXPos.Add(randWall);
                        Instantiate(singleWallPrefab, new Vector2(_xWallPositions[randWall], _yPositions[_currentYPos]), Quaternion.identity);

                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int randWall = Mathf.RoundToInt(Random.Range(0, 3));
                        while (usedWallXPos.Contains(randWall))
                        {
                            randWall = Mathf.RoundToInt(Random.Range(0, 3));
                        }
                        usedWallXPos.Add(randWall);
                        Instantiate(singleWallPrefab, new Vector2(_xWallPositions[randWall], _yPositions[_currentYPos]), Quaternion.identity);

                    }
                }
            }

            if (_currentOdds == 0.0f)
            {
                _currentOdds = 0.5f;
            }
            else if (_currentOdds == 0.5f)
            {
                _currentOdds = 0.75f;
            }
            else if (_currentOdds == 0.75f)
            {
                _currentOdds = 1.0f;
            }
        }
        int dotsToSpawn = 0;

        if (randDot <= 0.3f)
        {
            if (randDot <= 0.14f)
            {
                dotsToSpawn = 2;
            } 
            else if(randDot<=0.09f)
            {
                dotsToSpawn = 3;
            } else
            {
                dotsToSpawn = 1;
            }
        }
        for (int i = 0; i < dotsToSpawn; i++)
        {
            int dotRandX = Mathf.RoundToInt(Random.Range(0.0f, 4.0f));
            while (usedXPos.Contains(dotRandX))
            {
                dotRandX = Mathf.RoundToInt(Random.Range(0.0f, 4.0f));
            }
            Instantiate(dotPrefab, new Vector2(_xPositions[dotRandX], GameManager.Instance.headNode.position.y+_yPositions[_currentYPos]), Quaternion.identity);
            usedXPos.Add(dotRandX);
        }

    }

    private void SpawnBlockRow()
    {
        if (_tempFirstTime)
        {
            Instantiate(specialRowPrefab, new Vector2(0, GameManager.Instance.headNode.position.y + _yPositions[_currentYPos]), Quaternion.identity);
            _tempFirstTime = false;
        } else
        {
            Instantiate(rowBlockPrefab, new Vector2(0, GameManager.Instance.headNode.position.y + _yPositions[_currentYPos]), Quaternion.identity);
        }
        
        
    }

    public void Restart()
    {
        _tempFirstTime = true;
        _canSpawn = true;
    }

}
