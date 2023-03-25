using System.Runtime.InteropServices;
using System.Numerics;

namespace FFF
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Tribyte
    {
    	private byte[] data;

		public Tribyte() { data = new byte[3]; }

    	public Tribyte(byte b0, byte b1, byte b2)
    	{
    		this.data = new byte[3] { b0, b1, b2 };
    	}

    	public Tribyte(uint data) : this((byte)(data & 0xFF), (byte)((data >> 8) & 0xFF), (byte)((data >> 16) & 0xFF)) { }

    	public Tribyte(byte[] data)
    	{
    		this.data = new byte[3];
    		for(int i = 0; i < 3; i++)
    			this.data[i] = data[i];
    	}

    	public byte[] GetBytes() => data;
		public uint ToUInt() 
			=> (uint)data[0] | ((uint)data[1] << 8) | ((uint)data[2] << 16);

    	public override bool Equals(object obj)
    	{
    	    if (obj == null || !(obj is Tribyte other))
    	        return false;
			
			for(int i = 0; i < 3; i++)
				if(other.data[i] != data[i])
					return false;

			return true;
    	}
	
    	public override int GetHashCode() => HashCode.Combine(data[0], data[1], data[2]);

    	public static bool operator ==(Tribyte a, Tribyte b) => a.Equals(b);
    	public static bool operator !=(Tribyte a, Tribyte b) => !a.Equals(b);

    	public static bool operator ==(Tribyte a, ulong b) => a.ToUInt() == b;
    	public static bool operator !=(Tribyte a, ulong b) => a.ToUInt() != b;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Pentabyte
    {
    	private byte[] data;

		public Pentabyte() { data = new byte[5]; }

    	public Pentabyte(byte b0, byte b1, byte b2, byte b3, byte b4)
    	{
    		this.data = new byte[5] { b0, b1, b2, b3, b4 };
    	}

    	public Pentabyte(ulong data) : this((byte)(data & 0xFF), (byte)((data >> 8) & 0xFF), (byte)((data >> 16) & 0xFF), (byte)((data >> 24) & 0xFF), (byte)((data >> 32) & 0xFF)) { }

    	public Pentabyte(byte[] data)
    	{
    		this.data = new byte[5];
    		for(int i = 0; i < 5; i++)
    			this.data[i] = data[i];
    	}

    	public byte[] GetBytes() => data;
		public ulong ToUlong() 
			=> (ulong)data[0] | ((ulong)data[1] << 8) | ((ulong)data[2] << 16) | ((ulong)data[3] << 24) | ((ulong)data[4] << 32);

    	public override bool Equals(object obj)
    	{
    	    if (obj == null || !(obj is Pentabyte other))
    	        return false;
			
			for(int i = 0; i < 5; i++)
				if(other.data[i] != data[i])
					return false;

			return true;
    	}
	
    	public override int GetHashCode() => HashCode.Combine(data[0], data[1], data[2], data[3], data[4]);

    	public static bool operator ==(Pentabyte a, Pentabyte b) => a.Equals(b);
    	public static bool operator !=(Pentabyte a, Pentabyte b) => !a.Equals(b);

    	public static bool operator ==(Pentabyte a, ulong b) => a.ToUlong() == b;
    	public static bool operator !=(Pentabyte a, ulong b) => a.ToUlong() != b;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Hexabyte
    {
    	private byte[] data;

		public Hexabyte() { data = new byte[6]; }

    	public Hexabyte(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5)
    	{
    		this.data = new byte[6] { b0, b1, b2, b3, b4, b5 };
    	}

    	public Hexabyte(ulong data) : this((byte)(data & 0xFF), (byte)((data >> 8) & 0xFF), (byte)((data >> 16) & 0xFF), (byte)((data >> 24) & 0xFF), (byte)((data >> 32) & 0xFF), (byte)((data >> 40) & 0xFF)) { }

    	public Hexabyte(byte[] data)
    	{
    		this.data = new byte[6];
    		for(int i = 0; i < 6; i++)
    			this.data[i] = data[i];
    	}

    	public byte[] GetBytes() => data;
		public ulong ToUlong() 
			=> (ulong)data[0] | ((ulong)data[1] << 8) | ((ulong)data[2] << 16) | ((ulong)data[3] << 24) | ((ulong)data[4] << 32) | ((ulong)data[5] << 40);

    	public override bool Equals(object obj)
    	{
    	    if (obj == null || !(obj is Hexabyte other))
    	        return false;
			
			for(int i = 0; i < 6; i++)
				if(other.data[i] != data[i])
					return false;

			return true;
    	}
	
    	public override int GetHashCode() => HashCode.Combine(data[0], data[1], data[2], data[3], data[4], data[5]);

    	public static bool operator ==(Hexabyte a, Hexabyte b) => a.Equals(b);
    	public static bool operator !=(Hexabyte a, Hexabyte b) => !a.Equals(b);

    	public static bool operator ==(Hexabyte a, ulong b) => a.ToUlong() == b;
    	public static bool operator !=(Hexabyte a, ulong b) => a.ToUlong() != b;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Heptabyte
    {
    	private byte[] data;

		public Heptabyte() { data = new byte[7]; }

    	public Heptabyte(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6)
    	{
    		this.data = new byte[7] { b0, b1, b2, b3, b4, b5, b6 };
    	}

    	public Heptabyte(ulong data) : this((byte)(data & 0xFF), (byte)((data >> 8) & 0xFF), (byte)((data >> 16) & 0xFF), (byte)((data >> 24) & 0xFF), (byte)((data >> 32) & 0xFF), (byte)((data >> 40) & 0xFF), (byte)((data >> 48) & 0xFF)) { }

    	public Heptabyte(byte[] data)
    	{
    		this.data = new byte[7];
    		for(int i = 0; i < 7; i++)
    			this.data[i] = data[i];
    	}

    	public byte[] GetBytes() => data;
		public ulong ToUlong() 
			=> (ulong)data[0] | ((ulong)data[1] << 8) | ((ulong)data[2] << 16) | ((ulong)data[3] << 24) | ((ulong)data[4] << 32) | ((ulong)data[5] << 40) | ((ulong)data[6] << 48);

    	public override bool Equals(object obj)
    	{
    	    if (obj == null || !(obj is Heptabyte other))
    	        return false;
			
			for(int i = 0; i < 7; i++)
				if(other.data[i] != data[i])
					return false;

			return true;
    	}
	
    	public override int GetHashCode() => HashCode.Combine(data[0], data[1], data[2], data[3], data[4], data[5], data[6]);

    	public static bool operator ==(Heptabyte a, Heptabyte b) => a.Equals(b);
    	public static bool operator !=(Heptabyte a, Heptabyte b) => !a.Equals(b);

    	public static bool operator ==(Heptabyte a, ulong b) => a.ToUlong() == b;
    	public static bool operator !=(Heptabyte a, ulong b) => a.ToUlong() != b;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LargeInt
    {
    	private byte[] data;

    	public LargeInt() { data = new byte[12]; }

    	public LargeInt(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7, byte b8, byte b9, byte b10, byte b11)
    	{
    		this.data = new byte[12] { b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11 };
    	}

    	public LargeInt(byte[] data)
    	{
    		this.data = new byte[12];
    		for(int i = 0; i < 12; i++)
    			this.data[i] = data[i];
    	}

    	public byte[] GetBytes() => data;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BigInt
    {
    	private byte[] data;

    	public BigInt() { data = new byte[16]; }

    	public BigInt(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7, byte b8, byte b9, byte b10, byte b11, byte b12, byte b13, byte b14, byte b15)
    	{
    		this.data = new byte[16] { b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15 };
    	}

    	public BigInt(byte[] data)
    	{
    		this.data = new byte[16];
    		for(int i = 0; i < 16; i++)
    			this.data[i] = data[i];
    	}

    	public byte[] GetBytes() => data;

		public static implicit operator BigInt(BigInteger data) => new BigInt(data.ToByteArray());
		public static explicit operator BigInteger(BigInt data) => new BigInteger(data.GetBytes());
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GreatInt
    {
    	private byte[] data;

		public GreatInt() { data = new byte[24]; }

    	public GreatInt(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7, byte b8, byte b9, byte b10, byte b11, byte b12, byte b13, byte b14, byte b15, byte b16, byte b17, byte b18, byte b19, byte b20, byte b21, byte b22, byte b23)
    	{
    		this.data = new byte[24] { b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15, b16, b17, b18, b19, b20, b21, b22, b23 };
    	}

    	public GreatInt(byte[] data)
    	{
    		this.data = new byte[24];
    		for(int i = 0; i < 24; i++)
    			this.data[i] = data[i];
    	}

    	public byte[] GetBytes() => data;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HugeInt
    {
    	private byte[] data;

		public HugeInt() { data = new byte[32]; }

    	public HugeInt(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7, byte b8, byte b9, byte b10, byte b11, byte b12, byte b13, byte b14, byte b15, byte b16, byte b17, byte b18, byte b19, byte b20, byte b21, byte b22, byte b23, byte b24, byte b25, byte b26, byte b27, byte b28, byte b29, byte b30, byte b31)
    	{
    		this.data = new byte[32] { b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15, b16, b17, b18, b19, b20, b21, b22, b23, b24, b25, b26, b27, b28, b29, b30, b31 };
    	}

    	public HugeInt(byte[] data)
    	{
    		this.data = new byte[32];
    		for(int i = 0; i < 32; i++)
    			this.data[i] = data[i];
    	}

    	public byte[] GetBytes() => data;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GiantInt
    {
    	private byte[] data;

		public GiantInt() { data = new byte[64]; }

    	public GiantInt(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7, byte b8, byte b9, byte b10, byte b11, byte b12, byte b13, byte b14, byte b15, byte b16, byte b17, byte b18, byte b19, byte b20, byte b21, byte b22, byte b23, byte b24, byte b25, byte b26, byte b27, byte b28, byte b29, byte b30, byte b31, byte b32, byte b33, byte b34, byte b35, byte b36, byte b37, byte b38, byte b39, byte b40, byte b41, byte b42, byte b43, byte b44, byte b45, byte b46, byte b47, byte b48, byte b49, byte b50, byte b51, byte b52, byte b53, byte b54, byte b55, byte b56, byte b57, byte b58, byte b59, byte b60, byte b61, byte b62, byte b63)
    	{
    		this.data = new byte[64] { b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15, b16, b17, b18, b19, b20, b21, b22, b23, b24, b25, b26, b27, b28, b29, b30, b31, b32, b33, b34, b35, b36, b37, b38, b39, b40, b41, b42, b43, b44, b45, b46, b47, b48, b49, b50, b51, b52, b53, b54, b55, b56, b57, b58, b59, b60, b61, b62, b63 };
    	}

    	public GiantInt(byte[] data)
    	{
    		this.data = new byte[64];
    		for(int i = 0; i < 64; i++)
    			this.data[i] = data[i];
    	}

    	public byte[] GetBytes() => data;
    }
}