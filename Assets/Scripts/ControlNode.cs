using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Node
{
    // This class starts to link together the nodes, so that they know where each other are. These new nodes that it creates are above it and to the right of it.
    // This forms one of the many triangles that will create the mesh. 

    class ControlNode : Node
    {
        private bool active;
        private Node above, right;

        public ControlNode(Vector3 positon, bool active, float squareSize):base(positon)
        {
            this.active = active;
            above = new Node(getPosition() + ((Vector3.forward * squareSize) / 2f));
            right = new Node(getPosition() + ((Vector3.right * squareSize) / 2));
        }

        //these to funcitons are here to access the private varriables of the class
        public Node getAboveNode()
        {
            return above;
        }
        public Node getRightNode()
        {
            return right;
        }
        public bool checkIfActive()
        {
            return active;
        }
    }
}
