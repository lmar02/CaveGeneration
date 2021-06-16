using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Next step is to connect the caverns. 
namespace Generator
{
    public class MapGenaration : MonoBehaviour
    {
        //this array stores the data required to generate the cave. 
        private int[,] map;
        //used as temp storage for smoothing the map, so that no weird errors or behaviors happen. 
        private int[,] tempMap;

        //these integers are used to determine the length of the x and y access of the full cave system. 
        private const int arrayX = 100;
        private const int arrayY = 100;

        //a void funciton to create the initial array
        void mapGeneration()
        {
            map = new int[arrayX, arrayY];
            tempMap = new int[arrayX, arrayY];

        }

        //this will give us some control over the maps and give us the exact same outcome if the same seed is used. 
        private string seed = "cats are great 12345";
        private bool useRandomSeed = true;

        System.Random generateSeed()
        {
            if (useRandomSeed)
            {
                seed = Time.time.ToString();
            }

            //turns the seed string into a hashcode intege for the pseudo rng 
            System.Random rng = new System.Random(seed.GetHashCode());
            return rng;
        }

        //This integer allows me to adjust the percentage of the map that is filled by walls
        private int randomFillPercent = 50;
        //this uses the informaiton from seed to fill the map appropriately. 
        void fillMap(System.Random rng)
        {
            for (int x = 0; x < arrayX; ++x)
            {
                for (int y = 0; y < arrayY; ++y)
                {
                    //to make sure the border of the map is always walls
                    if (x == 0 || x == arrayX - 1 || y == 0 || y == arrayY - 1)
                    {
                        map[x, y] = 1;
                        
                    }
                    else
                        map[x, y] = (rng.Next(0, 100) <= randomFillPercent) ? 1 : 0;
                }
            }
        }

        //I made this function so that if I need to repeat a function x amount of times, it is easier for me to do so, so I do not have to repeat myself 
        //thinking on changing this function and hardcoding the calls to the functions. Not sure yet if this modularity is needed. 
        //I want to make sure that my code is as readable as possible. 
        void repeatFunction(Action passedFunciton, Action secondFunction)
        {
            const int passes = 5;

            for (int i = 0; i < passes; ++i)
            {
                passedFunciton();
                secondFunction();
            }
        }

        //this funciton goes through the map and calls findNeighborWalls
        void transitionThroughMap()
        {
            for (int x = 0; x < arrayX; ++x)
            {
                for (int y = 0; y < arrayY; ++y)
                {
                    findNeighborWalls(x, y);
                }
            }
        }

        //this fucntion finds the neighbor walls of a specific wall
        void findNeighborWalls(int x, int y)
        {

            int neightborwalls = 0;
            //these ints is used to limit which x and y neighbors are looked at
            const int xi = 1;
            const int yi = 1;


            for (int lx = -1; lx <= xi; ++lx)
            {
                for (int hy = -1; hy <= yi; ++hy)
                {
                    if (x + lx == -1 || y + hy == -1 || x + lx == arrayX || y + hy == arrayY)
                    {
                        continue;
                    }
                    else if (lx == 0 && hy == 0)
                    {
                        continue;

                    }
                    else if (x == 0 || x == arrayX - 1 || y == 0 || y == arrayY - 1)
                    {
                        ++neightborwalls;
                    }
                    else if (map[x + lx, y + hy] == 1)
                    {
                        ++neightborwalls;
                    }
                }
            }
        
    

            smooth(neightborwalls, x, y);

        }

        //this function checks to see how many neighbors were produced and determines what maps needs to be adjusted too. 
        //this const int is going to be used to allow for adjustements of the smooth function. 
        const int determinationNumber = 4;
        void smooth(int neighbors, int x, int y)
        {
             
            if (x == 0 || x == arrayX - 1 || y == 0 || y == arrayY - 1)
            {
                tempMap[x, y] = 1;
            }
            else if (neighbors <determinationNumber)
            {
                tempMap[x, y] = 0;
            }
            else if (neighbors > determinationNumber)
            {
                tempMap[x, y] = 1;
            }
            else
            {
                tempMap[x, y] = map[x, y];
            }
        }

        //transfer data from tempMap back to maps
        void tranferMapData()
        {
            for (int x = 0; x < arrayX; ++x)
            {
                for (int y = 0; y < arrayY; ++y)
                {
                    map[x, y] = tempMap[x, y];
                }
            }
        }
        

        //for debugging to make sure the generator is working propertly. uncomment for debugging purposes
        
        void OnDrawGizmos()
        {
            if(map!=null)
            {
                for (int x = 0; x < arrayX; ++x)
                {
                    for (int y = 0; y < arrayY; ++y)
                    {
                        //this creates small squares to make sure that the array is populating correctly. 
                        //the reason the vector is create in this way is purely becaues I like the way it looks with the x,y,z axis controls of the empyty object holding this script to be centered,
                        //within the printed gizmo
                        Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                        Vector3 position = new Vector3(-arrayX / 2 + x + 0.5f, -arrayY / 2+ y + 0.5f);
                        Gizmos.DrawCube(position, Vector3.one);
                    }
                }
            }
        }
        

        // Start is called before the first frame update
        void Start()
        {
            mapGeneration();
            System.Random rng = generateSeed();
            fillMap(rng);
            repeatFunction(transitionThroughMap,tranferMapData);
            
            

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                mapGeneration();
                System.Random rng = generateSeed();
                fillMap(rng);
                repeatFunction(transitionThroughMap, tranferMapData);
            }

        }
    }
}
