using Archiver.Api.HuffmanAlgorithm.Intrefaces;

namespace Archiver.Api.HuffmanAlgorithm;

// [Obsolete("Current algorithm is outdated. Use the HuffmanV2 version")]
public class HuffmanCompressor : ICompressor
{
    public void Compress(byte[] bytes, string archFileName)
    {
        var data = CompressBytes(bytes);
        File.WriteAllBytes(archFileName, data);
    }
    
    public void Decompress(byte[] bytes, string fileName)
    {
        var data = DecompressBytes(bytes);
        File.WriteAllBytes(fileName, data);
    }

    private byte[] DecompressBytes(byte[] bytes)
    {
        ParseHeader(bytes, out int dataLength, out int startIndex, out int[] freqs);
        var root = CreateHuffmanTree(freqs);
        var data = DecompressBytes(bytes, startIndex, dataLength, root);
        return data;
    }

    private byte[] DecompressBytes(byte[] bytes, int startIndex, int dataLength, Node root)
    {
        var size = 0;
        var current = root;
        var data = new List<byte>();
        for (var i = startIndex; i < bytes.Length; i++)
            for (var j = 1; j <= 128; j <<=1)
            {
                var isZero = (bytes[i] & j) == 0;
                if (isZero)
                    current = current.Bit0;
                else
                    current = current.Bit1;

                if (current.Bit0 != null)
                    continue;
                if (size++ < dataLength)
                    data.Add(current.Symbol);
                current = root;
            }

        return data.ToArray();
    }

    private void ParseHeader(
        byte[] bytes, 
        out int dataLength, 
        out int startIndex, 
        out int[] freqs)
    {
        dataLength = bytes[0] |
                     (bytes[1] << 8) |
                     (bytes[1] << 16) |
                     (bytes[1] << 24);

        freqs = new int[256];
        for (var i = 0; i < 256; i++)
            freqs[i] = bytes[4 + i];

        startIndex = 256 + 4;
    }


    private byte[] CompressBytes(byte[] bytes)
    {
        var frequencies = CalculateFrequency(bytes);
        var root = CreateHuffmanTree(frequencies);
        var head = CreateHeader(bytes.Length, frequencies);
        var codes = CreateHuffmanCode(root);
        var bits = Compress(bytes, codes);
        
        return head.Concat(bits).ToArray();
    }

    private byte[] CreateHeader(int dataLength, int[] frequencies)
    {
        var head = new List<byte>
        {
            (byte)(dataLength & 255),
            (byte)((dataLength >> 8) & 255),
            (byte)((dataLength >> 16) & 255),
            (byte)((dataLength >> 24) & 255)
        };
            
        for (var i = 0; i < 256; i++)
            head.Add((byte)frequencies[i]);

        return head.ToArray();
    }

    private byte[] Compress(byte[] data, string[] codes)
    {
        var bytes = new List<byte>();
        byte sum = 0;
        byte bit = 1;
        foreach (var symbol in data)
            foreach (var c in codes[symbol])
            {
                if (c == '1')
                    sum |= bit;

                if (bit < 128)
                    bit <<= 1;
                else
                {
                    bytes.Add(sum);
                    sum = 0;
                    bit = 1;
                }
            }
        
        if(bit > 1)
            bytes.Add(sum);

        return bytes.ToArray();
    }   

    private int[] CalculateFrequency(byte[] data)
    {
        var freqs = new int[256];
        foreach (var e in data)
        {
            freqs[e]++;
        }

        NormalizeFrequency(freqs);
        return freqs;
    }

    private void NormalizeFrequency(int[] freqs)
    {
        var max = freqs.Max();
        if (max <= 255) return;
        for (var i = 0; i < 256; i++)
            if (freqs[i] > 0)
                freqs[i] = 1 + freqs[i] * 255 / (max + 1);
    }

    private Node CreateHuffmanTree(int[] frequencies)
    {
        var pq = new PriorityQueue<Node, int>();
        for (var i = 0; i < 256; i++)
        {
            if (frequencies[i] > 0)
                pq.Enqueue(new Node((byte)i, frequencies[i]), frequencies[i]);
        }

        while (pq.Count > 1)
        {
            var bit0 = pq.Dequeue();
            var bit1 = pq.Dequeue();
            var freq = bit0.Freq + bit1.Freq;

            var next = new Node(bit0, bit1, freq);
            pq.Enqueue(next, freq);
        }

        return pq.Dequeue();
    }
    
    private string[] CreateHuffmanCode(Node root)
    {
        var codes = new string[256];
        Traversal(root, string.Empty, codes);
        return codes;
    }

    private void Traversal(Node root, string code, string[] codes)
    {
        if (root.Bit0 is null)
            codes[root.Symbol] = code;
        else
        {
            Traversal(root.Bit0, code + "0", codes);
            Traversal(root.Bit1, code + "1", codes);
        }
    }
}
