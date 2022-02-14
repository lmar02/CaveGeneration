
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Node
{
    public class MeshGenerator : MonoBehaviour
    {

        private GridOfSquares squareGrid;

        //Used to store the vectors of each point of the active noes of a given triangle, 
        private List<Vector3> verticePoints;
        private List<int> triangles;

        public void MeshGeneration(int[,] mapArray, float size)
        {
            dictionaryOfTriagles.Clear();
            outlines.Clear();
            checkedIndex.Clear();

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

            /*

            //this takes all the data that was created and uses it to caluclate the mesh of the walls. 
            Mesh wallMesh = GetComponent<MeshFilter>().mesh;

            wallMesh.vertices = verticePoints.ToArray();
            wallMesh.triangles = triangles.ToArray();
            wallMesh.RecalculateNormals();*/

            CreateWallMesh();

        }

        //This function takes all the data in verticePoints and places it in its proper order inside wall verticies
        //it also takes tartIndex and adds 0-3 to it to show which vertex of the trianlge it is adding to the list of wallTriangles. 
        //then it takes that data and creates a mesh for the walls of the cave. 
        void CreateWallMesh()
        {
            CalculateMeshOutline();

            List<Vector3> wallVerticies = new List<Vector3>();
            List<int> walltriangles = new List<int>();
            Mesh wallMesh = new Mesh();
            float wallHeight = 5;

            foreach(List<int> outline in outlines)
            {
                for(int i = 0; i <outline.Count - 1; ++i)
                {
                    int startIndex = wallVerticies.Count;

                    wallVerticies.Add(verticePoints[outline[i]]);                            //top left
                    wallVerticies.Add(verticePoints[outline[i+1]]);                          //top right
                    wallVerticies.Add(verticePoints[outline[i]] - Vector3.up * wallHeight);    //bottom left
                    wallVerticies.Add(verticePoints[outline[i+1]] - Vector3.up * wallHeight); // bottom Right

                    walltriangles.Add(startIndex + 0);  //TopLeft
                    walltriangles.Add(startIndex + 2); //BottomLeft
                    walltriangles.Add(startIndex + 3); //BottomRight

                    walltriangles.Add(startIndex + 3); //BottomRight
                    walltriangles.Add(startIndex + 1); //TopRight
                    walltriangles.Add(startIndex + 0); //TopLeft
                }
            }

            wallMesh.vertices = wallVerticies.ToArray();
            wallMesh.triangles = walltriangles.ToArray();

            MeshFilter walls = GetComponent<MeshFilter>();
            walls.mesh = wallMesh;






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
                    /*
                    checkedIndex.Add(square.GetTopLeft().GetIndex());
                    checkedIndex.Add(square.GetTopRight().GetIndex());
                    checkedIndex.Add(square.GetBottomRight().GetIndex());
                    checkedIndex.Add(square.GetBottomLeft().GetIndex())*/
                    break;



            }


        }

        
        //this function has params to make sure that it accepts controlnodes as well as nodes
        //the main purpose of this function is to create the individual triangles needed out of the nodes provided.
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

        //sets the index of the node to its proper location in verticePoints. 
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
        //using an indexer so that I can easily cycle through the ints stored in the struct. 
        private struct TriangleData
        {
            public int indexofA;
            public int indexofB;
            public int indexofC;
            public int[] indexer;

            public TriangleData(int a, int b, int c)
            {
                indexofA = a;
                indexofB = b;
                indexofC = c;

                indexer = new int[3];
                indexer[0] = a;
                indexer[1] = b;
                indexer[2] = c;

            }

            public int this[int i]
            {
                get
                {
                    return indexer[i];
                }

            }


            public bool CheckIndexs(int index)
            {
                return (index == indexofA || index == indexofB || index == indexofC);
            }



        }


        //stores all of the lists of triable data for a specific index. 
        Dictionary<int, List<TriangleData>> dictionaryOfTriagles = new Dictionary<int, List<TriangleData>>();

        //used to peek a list of int lists that represent all the outlines of the mesh.
        List<List<int>> outlines = new List<List<int>>();

        //this is used to make sure that once we check a index/vertice that we do not check it agian. 
        HashSet<int> checkedIndex = new HashSet<int>();

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
            StoreTriangleInDictionary(triangle.indexofC, triangle);
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
        //this function goes through the verticepoints list, and connects the outline that the mesh will use. It also calls 
        //FollowOutline which takes newOutlineVertex and outlines. 
        private void CalculateMeshOutline()
        {
            for(int vertex = 0; vertex < verticePoints.Count; ++vertex)
            {
                if(!checkedIndex.Contains(vertex))
                {
                    int newOutlineVertex = GetConnectedOutline(vertex);

                    if(newOutlineVertex != -1)
                    {
                        checkedIndex.Add(vertex);

                        List<int> newOutline = new List<int>();
                        newOutline.Add(vertex);
                        outlines.Add(newOutline);
                        FollowOutline(newOutlineVertex, outlines.Count - 1);
                        outlines[outlines.Count - 1].Add(vertex);
                    }
                }
            }
        }
        
        //this is a recersive function that continues and finds the full outline of a specific section of map until it has nowhere to go. As it goes through the outline it addes it to outlines which is a list<list<int>>. It also adds it to the hash called checkedIndex
        void FollowOutline(int vertexIndex, int outlineIndex)
        {
            outlines[outlineIndex].Add(vertexIndex);
            checkedIndex.Add(vertexIndex);

            int nextVertexIndex = GetConnectedOutline(vertexIndex);

            if (nextVertexIndex != -1)
            {
                FollowOutline(nextVertexIndex, outlineIndex);
            }
        }


        //this function creates a list of triangles depending on the index key. then uses a for loop to check each triangle 
        //then it loops through all the vertexes in the triangle and checks them against vertex b. 
        // if it is the same it skips calling isLinetheEdge, if isLineTheEdge returns true than it returns indexB
        private int GetConnectedOutline(int indexKey)
        {
            List<TriangleData> trianglesContainingIndexKey = dictionaryOfTriagles[indexKey];

            for(int i = 0; i < trianglesContainingIndexKey.Count; ++i)
            {
                TriangleData triangle = trianglesContainingIndexKey[i];

                for(int j = 0; j < 3; ++j)
                {
                    int indexB = triangle[j];

                    if ( indexB != indexKey && !checkedIndex.Contains(indexB))
                    {
                        if (IsLineTheEdge(indexKey, indexB))
                        {
                            return indexB;
                        }
                    }
                }

            }
            return -1;
        }

        //this takes two indexs, and determines if the line that these indexs creates is shared by any triangles. 
        //if this line is shared between multiple triangles than it return false. 
        //otherwise it returns true if there is only one triangle. 
        private bool IsLineTheEdge(int indexA, int indexB)
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
