using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Generator
{
    public class MapGenaration : MonoBehaviour
    {
        //this array stores the data required to generate the cave. 
        int[,] map;

        //these integers are used to determine the length of the x and y access. 
        int arrayX = 100;
        int arrayY = 100;

        //a void funciton to create the initial array
        void mapGeneration()
        {
            map = new int[arrayX,arrayY];

        }

        //this will give us some control over the maps and give us the exact same outcome if the same seed is used. 
        string seed = null;
        bool useSeed = true;

        System.Random generateSeed()
        {
            if(useSeed)
            {
                seed = Time.time.ToString();
            }

            //turns the seed string into a hashcode intege for the pseudo rng 
            System.Random rng = new System.Random(seed.GetHashCode());
            return rng;
        }

        //This integer allows me to adjust the percentage of the map that is filled by walls
        int randomFillPercent = 30;
        //this uses the informaiton from seed to fill the map appropriately. 
        void fillMap(System.Random rng)
        {
            for (int x = 0; x < arrayX; ++x)
            {
                for (int y = 0; y < arrayY; ++y)
                {
                    //to make sure the border of the map is always walls
                    if(x == 0 || x == arrayX - 1 || y == 0 || y == arrayY-1)
                    {
                        map[x, y] = 1;
                    }else
                    map[x, y] = (rng.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }

        
        //for debugging to make sure the generator is working propertly. uncomment for debugging purposes
        /*
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
        }*/
        

        // Start is called before the first frame update
        void Start()
        {
            mapGeneration();
            System.Random rng = generateSeed();
            fillMap(rng);
            

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
