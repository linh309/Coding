
using Coding;

var consistentHash = new ConsistentHashing(100);

consistentHash.AddNode("NodeA");
consistentHash.AddNode("NodeB");
consistentHash.AddNode("NodeC");
consistentHash.AddNode("NodeD");

var keys = new List<string>
{
    "elsa",
    "vinfast",
    "xanh sm",
    "employee hero",
    "deputy"
};

foreach(var key in keys)
{
    var node = consistentHash.GetNode(key);
    Console.WriteLine($"Key: {key}\t\t\tNode: {node.node} - {node.index}");
}

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


