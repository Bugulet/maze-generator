using System.Collections;
using System.Collections.Generic;

public class MarchingSquare
{
    public static readonly int[] Configurations = new int[15]
    {
        //BIT ORIENTATION
        //       1
        //     4   2
        //       3

        0b1111, //crossroad   
        0b1000,//up  
        0b0100,//right  
        0b0010,//down  
        0b0001,//left  
        0b1100,//up-right  
        0b0110,//down-right 
        0b0011,//down-left  
        0b1001,//up-left  
        0b0101,//left-right 
        0b1010,//up-down
        0b1101,//intersection up
        0b1110,//intersection right
        0b0111,//intersection down
        0b1011//intersection left
    };
}
