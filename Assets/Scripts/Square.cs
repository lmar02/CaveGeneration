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
        //I plan to to switch these back to private once testing is done. 
        private ControlNode topLeft, topRight, bottomLeft, bottomRight;
        private Node top, left, right, bottom;
        private int configuration = 0;


        public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft)
        {
           
            this.topLeft     = topLeft;
            this.topRight    = topRight;
            this.bottomRight = bottomRight;
            this.bottomLeft  = bottomLeft;

            top    = topLeft.GetRightNode();
            right  = bottomRight.GetAboveNode();
            bottom = bottomLeft.GetRightNode();
            left   = bottomLeft.GetAboveNode();
            

            SetConfiguration();


        }

        private void SetConfiguration()
        {
            if(topLeft.CheckIfActive())
            {
                configuration += 8;
            }
            if(topRight.CheckIfActive())
            {
                configuration += 4;
            }
            if(bottomRight.CheckIfActive())
            {
                configuration += 2;
            }
            if(bottomLeft.CheckIfActive())
            {
                configuration += 1;
            }
        }

        //functions meant so you can access the private variables of this class. 
        public int GetConfiguration()
        {
            return configuration;
        }
        public ControlNode GetTopLeft()
        {
            return topLeft;
        }
        public ControlNode GetTopRight()
        {
            return topRight;
        }
        public ControlNode GetBottomRight()
        {
            return bottomRight;
        }
        public ControlNode GetBottomLeft()
        {
            return bottomLeft;
        }
        public Node GetTopNode()
        {
            return top;
        }
        public Node GetLeftNode()
        {
            return left;
        }
        public Node GetRightNode()
        {
            return right;
        }
        public Node GetBottomNode()
        {
            return bottom;
        }
        
           

    }
}
