using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Node
{
    class GridOfSquares
    {
        private Square[,] squares;
        private int[,] map;
        ControlNode[,] controlNodes;
        float squareSize;


        //this constructor transfers the proper data to GridOfSquares, then calls the proper functions to continue the proper creation of all required elements of hte mesh. 
        public GridOfSquares(int[,] map, float squareSize)
        {

            this.squareSize = squareSize;
            int yLength = map.GetLength(1);
            int xLength = map.GetLength(0);

           for(int y = 0; y < map.GetLength(1); ++y)
            {
                for( int x = 0; x < map.GetLength(0); ++x)
                {
                    this.map[y, x] = map[y, x];
                }
            }

            createControlNode();

        }

        //This function creates the ControlNode array. 
        public void createControlNode()
        {
            //horizontalCount and verticalCount store the length of the multidimensional map array; 
            int horizontalCount = map.GetLength(1);
            int verticalCount = map.GetLength(0);

            //this creates floats that hold the total width and length of the map
            float width = horizontalCount * squareSize;
            float length = verticalCount * squareSize;
            
            // fully initializes the control node array to the proper size
            controlNodes = new ControlNode[verticalCount, horizontalCount];

            //goes step my step through the control node array and creates the proper control points base on maps stored information. 
            for (int y = 0; y < map.GetLength(1); ++y)
            {
                for (int x = 0; x < map.GetLength(0); ++x)
                {
                   
                    Vector3 position = new Vector3(-width / 2 + y * squareSize + squareSize / 2, 0, -length / 2 + x * squareSize + squareSize / 2);

                    controlNodes[y, x] = new ControlNode(position, map[y, x] == 1, squareSize);
                }
            }

        }


    }

}
