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
        //have these varibales public just for testing
        public Square[,] squares;
        public int[,] map;
        public ControlNode[,] controlNodes;
        private float squareSize;


        private int yLength;
        private int xLength;

        //this constructor transfers the all the data GridOfSquares needs to create the array that will be used to create the mesh. 
        public GridOfSquares(int[,] map, float squareSize)
        {
            
            this.squareSize = squareSize;

            yLength = map.GetLength(1);
            xLength = map.GetLength(0);

            this.map = new int[yLength, xLength];

            //this transfers the contents of maps to the private map varriable of GridOfSquares
           for(int y = 0; y < map.GetLength(1); ++y)
            {
                for( int x = 0; x < map.GetLength(0); ++x)
                {
                    this.map[y, x] = map[y, x];
                }
            }
           
            createControlNode();
            createSquareArray();

        }

        //This function creates the ControlNode array. 
        void createControlNode()
        {
            

            //this creates floats that hold the total width and length of the map
            float width = xLength * squareSize;
            float length = yLength * squareSize;
            
            // fully initializes the control node array to the proper size
            controlNodes = new ControlNode[yLength, xLength];

            //goes step my step through the control node array and creates the proper control points base on maps stored information. 
            for (int y = 0; y < map.GetLength(1); ++y)
            {
                for (int x = 0; x < map.GetLength(0); ++x)
                {
                   //creates a vector3 so that controlNode knows where it is located in the game. 
                    Vector3 position = new Vector3(-width / 2 + y * squareSize + squareSize / 2, 0, -length / 2 + x * squareSize + squareSize / 2);

                    //creates the new controlnode, passes it the data, and saves it to the control node array
                    controlNodes[y, x] = new ControlNode(position, map[y, x] == 1, squareSize);
                }
            }

        }

        //this creates the array of squares 
        void createSquareArray()
        {
            //I subtract one from lengths to make sure that we do not go outside of bounds of the other array. 
            squares = new Square[yLength-1, xLength-1];

            for (int y = 0; y < yLength - 1; ++y)
            {
                for (int x = 0; x < xLength - 1; ++x)
                {
                    //this cycles through the array of squares, creates new square objects. Then it passes to the square objects the data of different control nodes that it needs to know about.
                    //Then it saves this data all into the squares array. 
                    squares[y, x] = new Square(controlNodes[y, x + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);

                }
            }

        }


    }

}
