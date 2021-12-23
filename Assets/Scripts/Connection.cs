using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection
{
    
    public Node connectedNode;
    public int cost;

    public Connection(Node newnode, int resource)
    {
        this.connectedNode = newnode;
        this.cost = resource;
    }
}
