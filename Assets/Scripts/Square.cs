using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{

   // this class continues to build out on nodes and ControlNodes to bring the final structure of the mesh together so that we can create the wall properly. 
    class Square
    {
        //have these variables public to make testing easier
        public ControlNode topLeft, topRight, bottomLeft, bottomRight;
        public Node top, left, right, bottom;

        public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomLeft, ControlNode bottomRight)
        {
           
            this.topLeft     = topLeft;
            this.topRight    = topRight;
            this.bottomLeft  = bottomLeft;
            this.bottomRight = bottomRight;

            top = topLeft.getRightNode();
            left = bottomLeft.getAboveNode();
            right = bottomRight.getAboveNode();
            bottom = bottomLeft.getRightNode();


        }

    }
}
