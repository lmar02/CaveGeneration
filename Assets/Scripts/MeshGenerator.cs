
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Node
{
    public class MeshGenerator : MonoBehaviour
    {

        private GridOfSquares squareGrid;

        public void MeshGeneration(int[,] mapArray, float size)
        {
            squareGrid = new GridOfSquares(mapArray, size);

            // initialize the list variables
            verticePoints = new List<Vector3>();
            triangles = new List<int>();

            //steps through the square array in squareGrid and calles createTriablesfrom points.

            for (int y = 0; y < squareGrid.GetSizeOfGridY(); ++y)
            {
                for (int x = 0; x < squareGrid.GetSizeOfGridX(); ++x)
                {
                    CreateTrianglesFromPoints(squareGrid.GetSquare(x,y));
                }
            }


            //this takes all the data that was created and uses it to caluclate the mesh of the walls. 
            Mesh wallMesh = new Mesh();
            wallMesh = GetComponent<MeshFilter>().mesh;

            wallMesh.vertices = verticePoints.ToArray();
            wallMesh.triangles = triangles.ToArray();
           // wallMesh.RecalculateNormals();



        }


        // this uses the int configuration in squares to determine exactly what arragement of nodes and control nodes are active. Then based on that it creates the proper meshes for the wall. 
        //Uses the node and control node information to create triangles from those points. 
        private void CreateTrianglesFromPoints(Square square)
        {
            switch (square.GetConfiguration())
            {
                case 0: 
                    break;
                //One Point active in the square, all the possible solutions:
                case 1:
                    CreateMeshFromPoints(square.GetBottomNode(), square.GetBottomLeft(), square.GetLeftNode());
                    break;
                case 2:
                    CreateMeshFromPoints(square.GetRightNode(), square.GetBottomRight(), square.GetBottomNode());
                    break;
                case 4:
                    CreateMeshFromPoints(square.GetTopNode(), square.GetTopRight(), square.GetRightNode());
                    break;
                case 8:
                    CreateMeshFromPoints(square.GetLeftNode(), square.GetTopNode(), square.GetLeftNode());
                    break;

                // two points active in a square, all possible solutions:
                case 3:
                    CreateMeshFromPoints(square.GetRightNode(), square.GetBottomRight(), square.GetBottomLeft(), square.GetLeftNode());
                    break;
                case 6:
                    CreateMeshFromPoints(square.GetTopNode(), square.GetTopRight(), square.GetBottomRight(), square.GetBottomNode());
                    break;
                case 9:
                    CreateMeshFromPoints(square.GetTopLeft(), square.GetTopNode(), square.GetBottomNode(), square.GetBottomLeft());
                    break;
                case 12:
                    CreateMeshFromPoints(square.GetTopLeft(), square.GetTopRight(), square.GetRightNode(), square.GetLeftNode());
                    break;
                    //Diagonal active points
                case 5:
                    CreateMeshFromPoints(square.GetTopNode(), square.GetTopRight(), square.GetRightNode(), square.GetBottomNode(), square.GetBottomLeft(), square.GetLeftNode());
                    break;
                case 10:
                    CreateMeshFromPoints(square.GetTopLeft(), square.GetTopNode(), square.GetRightNode(), square.GetBottomRight(), square.GetBottomNode(), square.GetLeftNode());
                    break;

                // Three points acitve in the square

                case 7:
                    CreateMeshFromPoints(square.GetTopNode(), square.GetTopRight(), square.GetBottomRight(), square.GetBottomLeft(), square.GetLeftNode());
                    break;
                case 11:
                    CreateMeshFromPoints(square.GetTopLeft(), square.GetTopNode(), square.GetRightNode(), square.GetBottomRight(), square.GetBottomLeft());
                    break;
                case 13:
                    CreateMeshFromPoints(square.GetTopLeft(), square.GetTopRight(), square.GetRightNode(), square.GetBottomNode(), square.GetBottomLeft());
                    break;
                case 14:
                    CreateMeshFromPoints(square.GetTopLeft(), square.GetTopRight(), square.GetBottomRight(), square.GetBottomNode(), square.GetLeftNode());
                    break;

                // 4 points active in the square

                case 15:
                    CreateMeshFromPoints(square.GetTopLeft(), square.GetTopRight(), square.GetBottomRight(), square.GetBottomLeft());
                    break;



            }


        }

        //Used to store the vectors of each point of the active noes of a given triangle, 
       private List<Vector3> verticePoints;
       private List<int> triangles;
        //this function has params to make sure that it accepts controlnodes as well as nodes
        private void CreateMeshFromPoints(params Node[] nodes)
        {
            SetVerticePoints(nodes);

            if(nodes.Length >=3)
            {
                CreateTriangles(nodes[0], nodes[1], nodes[2]);
            }
            if(nodes.Length >= 4)
            {
                CreateTriangles(nodes[0], nodes[2], nodes[3]);
            }
            if(nodes.Length >= 5)
            {
                CreateTriangles(nodes[0], nodes[3], nodes[4]);
            }
            if(nodes.Length >= 6)
            {
                CreateTriangles(nodes[0], nodes[4], nodes[5]);
            }
        }

        void SetVerticePoints(Node[] nodes)
        {
            for(int iterator = 0; iterator <nodes.Length; ++iterator)
            { 
                if( nodes[iterator].GetIndex() == -1)
                {
                    nodes[iterator].SetIndex(verticePoints.Count);
                    verticePoints.Add(nodes[iterator].GetPosition());
                }
            }
        }

        private void CreateTriangles(Node a, Node b, Node c)
        {
            triangles.Add(a.GetIndex());
            triangles.Add(b.GetIndex());
            triangles.Add(c.GetIndex());
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
        /*
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
                        //If I need to activate this again remember to change the code, since I added the new funcitons. 
                        Gizmos.color = (squareGrid.GetSquare(x, y).GetTopLeft().CheckIfActive()) ? Color.black : Color.white;
                        Gizmos.DrawCube(squareGrid.GetSquare(x, y).GetTopLeft().GetPosition(), Vector3.one * 0.5f);

                        Gizmos.color = (squareGrid.GetSquare(x, y).GetTopRight().CheckIfActive()) ? Color.black : Color.white;
                        Gizmos.DrawCube(squareGrid.GetSquare(x, y).GetTopRight().GetPosition(), Vector3.one * 0.5f);

                        Gizmos.color = (squareGrid.GetSquare(x, y).GetBottomLeft().CheckIfActive()) ? Color.black : Color.white;
                        Gizmos.DrawCube(squareGrid.GetSquare(x, y).GetBottomLeft().GetPosition(), Vector3.one * 0.5f);

                        Gizmos.color = (squareGrid.GetSquare(x, y).GetBottomLeft().CheckIfActive()) ? Color.black : Color.white;
                        Gizmos.DrawCube(squareGrid.GetSquare(x, y).GetBottomLeft().GetPosition(), Vector3.one * 0.5f);

                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(squareGrid.GetSquare(x, y).GetTopNode().GetPosition(), Vector3.one * 0.1f);
                        Gizmos.DrawCube(squareGrid.GetSquare(x, y).GetRightNode().GetPosition(), Vector3.one * 0.1f);
                        Gizmos.DrawCube(squareGrid.GetSquare(x, y).GetBottomNode().GetPosition(), Vector3.one * 0.1f);
                        Gizmos.DrawCube(squareGrid.GetSquare(x, y).GetLeftNode().GetPosition(), Vector3.one * 0.1f);

                    }
                }
            }
        }*/


    }
}
