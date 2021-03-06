﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class BoardManager : MonoBehaviour
    {
        [Serializable]
        public class Count
        {
            public int Minimum;
            public int Maximum;

            public Count(int min, int max)
            {
                Minimum = min;
                Maximum = max;
            }
        }

        #region Unity Inspector

        public int Columns = 20;
        public int Rows = 16;
        public Count WallCount = new Count(15, 29);
        public Count FoodCount = new Count(1, 5);
        public GameObject Exit;
        public GameObject[] FloorTiles;
        public GameObject[] WallTiles;
        public GameObject[] FoodTiles;
        public GameObject[] EnemyTiles;
        public GameObject[] OuterwallTiles;

        #endregion




        void InitializeList()
        {
            _gridPositions.Clear();

            for (var x = 1; x < Columns - 1; ++x) 
            {
                for (var y = 1; y < Rows - 1; ++y) 
                {
                    _gridPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }

        void BoardSetup()
        {
            _boardHolder = new GameObject("Board").transform;

            for (var x = -1; x < Columns + 1; ++x)
            {
                for (var y = -1; y < Rows + 1; ++y)
                {
                    var toInstantiate = FloorTiles[Random.Range (0, FloorTiles.Length)];
                    if (x == -1 || x == Columns || y == -1 || y == Rows)
                    {
                        toInstantiate = OuterwallTiles[Random.Range (0, OuterwallTiles.Length)];
                    }

                    var instance = Instantiate(toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
                    Debug.Assert(instance != null, "instance != null");
                    instance.transform.SetParent(_boardHolder);
                }
            }
        }

        Vector3 RandomPositions()
        {
            var randomIndex = Random.Range(0, _gridPositions.Count);
            var randomPositions = _gridPositions [randomIndex];
            _gridPositions.RemoveAt(randomIndex);
            return randomPositions;
        }

        void LayoutObjectAtRandom(IList<GameObject> tileArray, int minimum, int maximum)
        {
            var objectCount = Random.Range(minimum, maximum + 1);
            for (var i = 0; i < objectCount; ++i)
            {
                var position = RandomPositions();
                var tileCoise = tileArray [Random.Range (0, tileArray.Count)];
                Instantiate (tileCoise, position, Quaternion.identity);
            }
        }

        #region Public Methods

        public void ShowTooltip(Vector3 tooltipPosition, MonoBehaviour item)
        {
            GameManager.Instance.Tooltip.SetActive(true);
        }

        public void SetupScene(int level)
        {
            BoardSetup();
            InitializeList();
            LayoutObjectAtRandom(WallTiles, WallCount.Minimum, WallCount.Maximum);
            LayoutObjectAtRandom(FoodTiles, FoodCount.Minimum, FoodCount.Maximum);
            var enemyCount = (int)Math.Log(level, 2f) * level;
            LayoutObjectAtRandom(EnemyTiles, enemyCount, enemyCount);
            Instantiate(Exit, new Vector3(Columns - 1, Rows - 1, 0f), Quaternion.identity);
        }

        #endregion Pyblic Methods

        #region Fields

        private Transform _boardHolder;
        private readonly List<Vector3> _gridPositions = new List<Vector3>();

        #endregion Fields
    }
}
