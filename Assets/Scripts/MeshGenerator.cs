
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

        //creating a gizmos method to make sure my code is working
        void OnDrawGizmos()
        {
            if (squareGrid != null)
            {
                for (int x = 0; x < 99; ++x)
                {
                    for (int y = 0; y < 99; ++y)
                    {
                        //this creates small squares to make sure that the array is populating correctly. 
                        //the reason the vector is create in this way is purely becaues I like the way it looks with the x,y,z axis controls of the empyty object holding this script to be centered,
                        //within the printed gizmo
                        Gizmos.color = (squareGrid.squares[x, y].topLeft.checkIfActive()) ? Color.black : Color.white;
                        Gizmos.DrawCube(squareGrid.squares[x, y].topLeft.getPosition(), Vector3.one * 0.5f);

                        Gizmos.color = (squareGrid.squares[x, y].topRight.checkIfActive()) ? Color.black : Color.white;
                        Gizmos.DrawCube(squareGrid.squares[x, y].topRight.getPosition(), Vector3.one * 0.5f);

                        Gizmos.color = (squareGrid.squares[x, y].bottomLeft.checkIfActive()) ? Color.black : Color.white;
                        Gizmos.DrawCube(squareGrid.squares[x, y].bottomLeft.getPosition(), Vector3.one * 0.5f);

                        Gizmos.color = (squareGrid.squares[x, y].bottomLeft.checkIfActive()) ? Color.black : Color.white;
                        Gizmos.DrawCube(squareGrid.squares[x, y].bottomLeft.getPosition(), Vector3.one * 0.5f);

                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(squareGrid.squares[x, y].top.getPosition(), Vector3.one * 0.1f);
                        Gizmos.DrawCube(squareGrid.squares[x, y].right.getPosition(), Vector3.one * 0.1f);
                        Gizmos.DrawCube(squareGrid.squares[x, y].bottom.getPosition(), Vector3.one * 0.1f);
                        Gizmos.DrawCube(squareGrid.squares[x, y].left.getPosition(), Vector3.one * 0.1f);

                    }
                }
            }
        }


    }
}
