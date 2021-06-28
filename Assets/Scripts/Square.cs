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
        public Node top, left, right, bottom;
        private int configuration = 0;


        public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomLeft, ControlNode bottomRight)
        {
           
            this.topLeft     = topLeft;
            this.topRight    = topRight;
            this.bottomLeft  = bottomLeft;
            this.bottomRight = bottomRight;

            top =    topLeft.GetRightNode();
            left =   bottomLeft.GetAboveNode();
            right =  bottomRight.GetAboveNode();
            bottom = bottomLeft.GetRightNode();

            setConfiguration();


        }

        private void setConfiguration()
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
        public ControlNode GetTopLeft()
        {
            return topLeft;
        }
        public ControlNode GetTopRight()
        {
            return topRight;
        }
        public ControlNode GetbottomRight()
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
