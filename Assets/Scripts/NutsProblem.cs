using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class NutsProblem : MonoBehaviour
{

    [SerializeField] bool isWithNuts = true;
    [SerializeField] bool isMinOrMax = false; // true = maximization | false = minimization
    [SerializeField] string startNode = "a";
    [SerializeField] string goalNode = "c";
    [SerializeField] int nutGoal = 5;

    Node a, b, c, d, e;
    Node startNodeToUse;
    Node goalNodeToUse;
    Route bestRoute;
    string output;


    // Start is called before the first frame update
    void Start()
    {
        InitializeValues();
        TranslateUserInput();

        if (isWithNuts)
        {
            Debug.Log(ExhaustiveSearchWithNuts(startNodeToUse, nutGoal, isMinOrMax));
        }
        else
        {
            Debug.Log(ExhaustiveSearch(startNodeToUse, goalNodeToUse, isMinOrMax)); ;
        }
        
    }

    // Function to search for best path
    /*
        user input:
        startNode   | initial point,
        goalNode    | end goal
    */
    /*
        parameter:
        minOrMax      | false = minimization, true = maximization
    */
    string ExhaustiveSearch(Node startNode, Node goalNode,bool minOrMax = false,  int totalCost = 0, Route route = null)
    {
        // If route not set, initialize new route
        if (route == null)
        {
            route = new Route();
            if (minOrMax)
            {
                route.minimumCost = -999;
            }
            else
            {
                route.minimumCost = 999;
            }
            
            route.fullRoute = new string[5];
            route.fullRoute[0] = startNode.nodeName;
            bestRoute = new Route();
            bestRoute = route;
        }

        // If goal reached, check if cost is better and return the best route
        if (startNode.nodeName == goalNode.nodeName)
        {
            if (minOrMax)
            {
                if (totalCost > bestRoute.minimumCost)
                {
                    output = "";
                    bestRoute = new Route();
                    bestRoute.minimumCost = totalCost;
                    bestRoute.fullRoute = route.fullRoute;
                    for (int i = 0; i < bestRoute.fullRoute.Length; i++)
                    {
                        output = output + bestRoute.fullRoute[i];
                    }
                    output = output + " | cost = " + totalCost;
                }
            }
            else
            {
                if (totalCost < bestRoute.minimumCost)
                {
                    output = "";
                    bestRoute = new Route();
                    bestRoute.minimumCost = totalCost;
                    bestRoute.fullRoute = route.fullRoute;

                    for (int i = 0; i < bestRoute.fullRoute.Length; i++)
                    {
                        output = output + bestRoute.fullRoute[i];
                    }
                    output = output + " | cost = " + totalCost;
                }
            }
            route.fullRoute[findNextRouteElement(route) - 1] = null;
            return output;
        }

       
        // For all the connected nodes,
        foreach (Connection connection in startNode.connections)
        {
            // If Unexplored
            if (!route.fullRoute.Contains(connection.connectedNode.nodeName))
            {
                //OutputRoute(route);

                // Search new Node
                totalCost += connection.cost;
                int nextNode = findNextRouteElement(route);
                route.fullRoute[nextNode] = connection.connectedNode.nodeName;
                ExhaustiveSearch(connection.connectedNode, goalNode, minOrMax, totalCost, route);
                totalCost -= connection.cost;
            }
        }
        route.fullRoute[findNextRouteElement(route) - 1] = null;

        return output;
    }

    string ExhaustiveSearchWithNuts(Node startNode, int goalNuts, bool minOrMax = false, int totalCost = 0, int totalNuts = 0, Route route = null)
    {
        // If route not set, initialize new route
        if (route == null)
        {
            route = new Route();
            if (minOrMax)
            {
                route.minimumCost = -999;
            }
            else
            {
                route.minimumCost = 999;
            }

            route.fullRoute = new string[5];
            route.fullRoute[0] = startNode.nodeName;
            bestRoute = new Route();
            bestRoute = route;
        }

        // If goal reached, check if cost is better and return the best route
        if (totalNuts == goalNuts)
        {
            if (minOrMax)
            {
                if (totalCost > bestRoute.minimumCost)
                {
                    output = "";
                    bestRoute = new Route();
                    bestRoute.minimumCost = totalCost;
                    bestRoute.fullRoute = route.fullRoute;
                    for (int i = 0; i < bestRoute.fullRoute.Length; i++)
                    {
                        output = output + bestRoute.fullRoute[i];
                    }
                    output = output + " | cost = " + totalCost;
                }
            }
            else
            {
                if (totalCost < bestRoute.minimumCost)
                {
                    output = "";
                    bestRoute = new Route();
                    bestRoute.minimumCost = totalCost;
                    bestRoute.fullRoute = route.fullRoute;

                    for (int i = 0; i < bestRoute.fullRoute.Length; i++)
                    {
                        output = output + bestRoute.fullRoute[i];
                    }
                    output = output + " | cost = " + totalCost;
                }
            }
            route.fullRoute[findNextRouteElement(route) - 1] = null;
            return output;
        }


        // For all the connected nodes,
        foreach (Connection connection in startNode.connections)
        {
            // If Unexplored
            if (!route.fullRoute.Contains(connection.connectedNode.nodeName))
            {
                //OutputRoute(route);

                // Search new Node
                totalCost += connection.cost;
                totalNuts += connection.connectedNode.nuts;
                int nextNode = findNextRouteElement(route);
                route.fullRoute[nextNode] = connection.connectedNode.nodeName;
                ExhaustiveSearchWithNuts(connection.connectedNode, goalNuts, minOrMax, totalCost,totalNuts, route);
                totalCost -= connection.cost;
                totalNuts -= connection.connectedNode.nuts;
            }
        }
        route.fullRoute[findNextRouteElement(route) - 1] = null;

        return output;
    }

    // Find next element in the route string to use
    int findNextRouteElement(Route currRoute)
    {
        for (int i = 0; i< currRoute.fullRoute.Length; i++)
        {
            if (currRoute.fullRoute[i] == null)
            {
                return i;
            }
        }
        return 5;
    }

    // Translate Serialized fields to their respective nodes
    void TranslateUserInput()
    {
        switch (startNode)
        {
            case "a":
                startNodeToUse = a;
                break;
            case "b":
                startNodeToUse = b;
                break;
            case "c":
                startNodeToUse = c;
                break;
            case "d":
                startNodeToUse = d;
                break;
            case "e":
                startNodeToUse = e;
                break;
            default:
                startNodeToUse = a;
                break;
        }

        switch (goalNode)
        {
            case "a":
                goalNodeToUse = a;
                break;
            case "b":
                goalNodeToUse = b;
                break;
            case "c":
                goalNodeToUse = c;
                break;
            case "d":
                goalNodeToUse = d;
                break;
            case "e":
                goalNodeToUse = e;
                break;
            default:
                goalNodeToUse = a;
                break;
        }
    }

    // Initialize all the Nodes
    void InitializeValues()
    {
        a = new Node();
        b = new Node();
        c = new Node();
        d = new Node();
        e = new Node();

        bestRoute = new Route();
        output = "";

        // Node A
        a.nodeName = "A";
        a.nuts = 0;
        a.connections = new Connection[3];
        a.connections[0] = new Connection(b, 6);
        a.connections[1] = new Connection(d, 7);
        a.connections[2] = new Connection(e, 3);

        // Node B
        b.nodeName = "B";
        b.nuts = 2;
        b.connections = new Connection[2];
        b.connections[0] = new Connection(a, 6);
        b.connections[1] = new Connection(c, 8);

        // Node C
        c.nodeName = "C";
        c.nuts = 2;
        c.connections = new Connection[2];
        c.connections[0] = new Connection(b, 8);
        c.connections[1] = new Connection(d, 6);

        // Node D
        d.nodeName = "D";
        d.nuts = 2;
        d.connections = new Connection[3];
        d.connections[0] = new Connection(a, 7);
        d.connections[1] = new Connection(c, 6);
        d.connections[2] = new Connection(e, -2);

        // Node E
        e.nodeName = "E";
        e.nuts = 3;
        e.connections = new Connection[2];
        e.connections[0] = new Connection(a, 3);
        e.connections[1] = new Connection(d, -2);
    }


}