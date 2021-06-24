
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Node
{
    public class MeshGenerator : MonoBehaviour
    {

        private GridOfSquares squareGrid;

        public void meshGenerationFunction(int[,] mapArray, float size)
        {
            squareGrid = new GridOfSquares(mapArray, size);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
