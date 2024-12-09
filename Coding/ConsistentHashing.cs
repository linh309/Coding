using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding;

internal class ConsistentHashing
{
    private readonly SortedDictionary<long, string> _ring = new SortedDictionary<long, string>();
    private readonly int _replicas; // Virtual nodes for better distribution

    public ConsistentHashing(int replicas = 100)
    {
        _replicas = replicas;
    }

    private long Hash(string key)
    {
        using (var sha1 = System.Security.Cryptography.SHA1.Create())
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(key);
            var hashBytes = sha1.ComputeHash(bytes);
            return BitConverter.ToUInt32(hashBytes, 0); // Use first 4 bytes for a 32-bit hash
        }
    }

    public void AddNode(string node)
    {
        // Add multiple replicas for better distribution
        for (int i = 0; i < _replicas; i++)
        {
            long hash = Hash(node + i); // Unique hash for each replica
            _ring[hash] = node;
        }
    }

    public (long index, string node) GetNode(string key)
    {
        if (_ring.Count == 0)
        {
            throw new InvalidOperationException("No nodes available in the ring.");
        }

        long hash = Hash(key);
        foreach (var entry in _ring.Keys)
        {
            if (entry >= hash)
            {
                return (entry, _ring[entry]);
            }
        }

        var firstEntry = _ring.Keys.FirstOrDefault();
        return (firstEntry, _ring[firstEntry]);
    }

    public void DisplayRing()
    {
        var dictCount = new Dictionary<string, int>();
        foreach (var pair in _ring)
        {
            Console.WriteLine($"Hash: {pair.Key}, Node: {pair.Value}");
            if (dictCount.ContainsKey(pair.Value))
            {
                dictCount[pair.Value] += 1;
            }
            else
            {
                dictCount[pair.Value] = 1;
            }
        }

        foreach (var item in dictCount)
        {
            Console.WriteLine($"Node: {item.Value}. Count: {item.Key}");
        }

    }
}
