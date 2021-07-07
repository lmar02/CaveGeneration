
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

            for (int x = 0; x < squareGrid.GetSizeOfGridX(); ++x)
            {
                for (int y = 0; y < squareGrid.GetSizeOfGridY(); ++y)
                {
                    CreateTrianglesFromPoints(squareGrid.GetSquare(x,y));
                }
            }


            //this takes all the data that was created and uses it to caluclate the mesh of the walls. 
            Mesh wallMesh = GetComponent<MeshFilter>().mesh;

            wallMesh.vertices = verticePoints.ToArray();
            wallMesh.triangles = triangles.ToArray();
            wallMesh.RecalculateNormals();



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
                    CreateMeshFromPoints(square.GetLeftNode(), square.GetBottomNode(), square.GetBottomLeft());
                    break;
                case 2:
                    CreateMeshFromPoints(square.GetBottomRight(), square.GetBottomNode(), square.GetRightNode());
                    break;
                case 4:
                    CreateMeshFromPoints(square.GetTopRight(), square.GetRightNode(), square.GetTopNode());
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

        private void SetVerticePoints(Node[] nodes)
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


        //variable / struct used in CreateTriables() and StoreTrianglesInDictionary()
        private struct TriangleData
        {
            public int indexofA;
            public int indexofB;
            public int indexofC;

            public TriangleData(int a, int b, int c)
            {
                indexofA = a;
                indexofB = b;
                indexofC = c;
            }

            public bool CheckIndexs(int index)
            {
                return (index == indexofA || index == indexofB || index == indexofC);
            }



        }

        Dictionary<int, List<TriangleData>> dictionaryOfTriagles = new Dictionary<int, List<TriangleData>>();

        private void CreateTriangles(Node a, Node b, Node c)
        {
            //stores each nodes index in list Triangles.
            triangles.Add(a.GetIndex());
            triangles.Add(b.GetIndex());            
            triangles.Add(c.GetIndex());

            //creates a object of struct Triangle data to store the triangles indexes. 
            TriangleData triangle = new TriangleData(a.GetIndex(), b.GetIndex(), c.GetIndex());
            
            //stores each index of the given triangle. 
            StoreTriangleInDictionary(triangle.indexofA, triangle);
            StoreTriangleInDictionary(triangle.indexofB, triangle);
            StoreTriangleInDictionary(triangle.indexofB, triangle);
        }


        //this function takes a key, indexKey and a Triangle, then it checks to make sure that the IndexKey, if there is already on provided it stores the
        //new triagle under that key in the triList, otherwise it addes a new key and a new list of triangleData to the dictionary. 
        private void StoreTriangleInDictionary(int IndexKey, TriangleData triangle)
        {
            // first checking to see if this indexKey is in the dictionary, if it is it adds this traigle to that indexKey
            if (dictionaryOfTriagles.ContainsKey(IndexKey))
            {
                dictionaryOfTriagles[IndexKey].Add(triangle);
            }
            else
            {
                // this creates a list of triangledata, that is then added to the dictionary.
                List<TriangleData> triList = new List<TriangleData>();
                triList.Add(triangle);

                dictionaryOfTriagles.Add(IndexKey, triList);
            }
        }

        //this takes two indexs, and determines if the line that these indexs creates is shared by any triangles. 
        //if this line is shared between multiple triangles than it return false. 
        //otherwise it returns true if there is only one triangle. 
        bool IsLineTheEdge(int indexA, int indexB)
        {
            List<TriangleData> listOfTrianglesWithindexA = dictionaryOfTriagles[indexA];
            int sharedTriangleCount = 0;

            for(int i =0; i < listOfTrianglesWithindexA.Count; ++i)
            {
                if(listOfTrianglesWithindexA[i].CheckIndexs(indexB))
                {
                    ++sharedTriangleCount;
                }
                if (sharedTriangleCount > 1)
                {
                    break;
                }
            }
            return sharedTriangleCount == 1;
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
