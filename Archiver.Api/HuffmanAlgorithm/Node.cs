namespace Archiver.Api.HuffmanAlgorithm;

public class Node
{
    public readonly byte Symbol;
    public readonly int Freq;
    public readonly Node? Bit0;
    public readonly Node? Bit1;

    public Node(byte symbol, int freq)
    {
        Symbol = symbol;
        Freq = freq;
    }

    public Node(Node bit0, Node bit1, int freq)
    {
        Bit0 = bit0;
        Bit1 = bit1;
        Freq = freq;
    }
}