using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Node
{
    //this class defines what a node is and where it is going to be located in 'space'. This is the smallest building block of our wall mesh. 
    //We will use nodes to build squares, depending on the nodes that are active will effect how the final mesh is created and shaped. 
    
    class Node
    {
        private Vector3 position;
        private int index = -1;

        public Node(Vector3 postion)
        {
            this.position = postion;

            
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }
        public int GetIndex()
        {
            return index;
        }
    }
}
