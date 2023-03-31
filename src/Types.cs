using System.Runtime.InteropServices;
using System.Numerics;

namespace FFF
{
	public struct Primitive
	{
		private object variant;

		private Primitive(object variant) { this.variant = variant; }

/*
#macro decl PRIMITIVE_CONSTRUCT(type)
	public Primitive(type value) : this((object)value) { }
#macro end
*/

		//#macro use PRIMITIVE_CONSTRUCT(     bool)

		//#macro use PRIMITIVE_CONSTRUCT(     byte)
		//#macro use PRIMITIVE_CONSTRUCT(    sbyte)

		//#macro use PRIMITIVE_CONSTRUCT(    short)
		//#macro use PRIMITIVE_CONSTRUCT(   ushort)
		//#macro use PRIMITIVE_CONSTRUCT(     Half)

		//#macro use PRIMITIVE_CONSTRUCT(  Tribyte)

		//#macro use PRIMITIVE_CONSTRUCT(      int)
		//#macro use PRIMITIVE_CONSTRUCT(     uint)
		//#macro use PRIMITIVE_CONSTRUCT(    float)

		//#macro use PRIMITIVE_CONSTRUCT(Pentabyte)

		//#macro use PRIMITIVE_CONSTRUCT( Hexabyte)

		//#macro use PRIMITIVE_CONSTRUCT(Heptabyte)

		//#macro use PRIMITIVE_CONSTRUCT(     long)
		//#macro use PRIMITIVE_CONSTRUCT(    ulong)
		//#macro use PRIMITIVE_CONSTRUCT(   double)

		//#macro use PRIMITIVE_CONSTRUCT( LargeInt)
		//#macro use PRIMITIVE_CONSTRUCT(   BigInt)
		//#macro use PRIMITIVE_CONSTRUCT( GreatInt)
		//#macro use PRIMITIVE_CONSTRUCT(  HugeInt)
		//#macro use PRIMITIVE_CONSTRUCT( GiantInt)

		public bool Is<T>() => typeof(T) == variant.GetType();

		public T As<T>() => Is<T>() ? (T)variant : throw new Exception(string.Concat("Cannot convert primitive variant to ", typeof(T).FullName, " the primitive type is ", variant.GetType().FullName));

/*
#macro decl PRIMITIVE_IMPLICIT_OPERATOR(type)
	public static implicit operator Primitive(type value) => new Primitive(value);
#macro end
*/

		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(     bool)
		   
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(     byte)
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(    sbyte)
		   
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(    short)
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(   ushort)
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(     Half)
		  
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(  Tribyte)
		  
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(      int)
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(     uint)
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(    float)
				
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(Pentabyte)
				
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR( Hexabyte)
				
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(Heptabyte)
				
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(     long)
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(    ulong)
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(   double)
		
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR( LargeInt)
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(   BigInt)
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR( GreatInt)
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR(  HugeInt)
		//#macro use PRIMITIVE_IMPLICIT_OPERATOR( GiantInt)
/*
#macro decl PRIMITIVE_EXPLICIT_OPERATOR(type)
	public static explicit operator type(Primitive primitive) => (type)primitive.variant;
#macro end
*/

		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(     bool)
				   
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(     byte)
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(    sbyte)
				   
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(    short)
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(   ushort)
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(     Half)
  
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(  Tribyte)
		  
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(      int)
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(     uint)
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(    float)
				
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(Pentabyte)
				
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR( Hexabyte)
				
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(Heptabyte)
				
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(     long)
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(    ulong)
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(   double)
		
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR( LargeInt)
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(   BigInt)
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR( GreatInt)
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR(  HugeInt)
		//#macro use PRIMITIVE_EXPLICIT_OPERATOR( GiantInt)
	}

/*
#macro decl CONV_PRIMITIVE_STRUCT(type, size, convType, convTypeName)
#macro import macros/ConvPrimitiveStruct.macro
#macro end

#macro decl PRIMITIVE_STRUCT(type, size)
#macro import macros/PrimitiveStruct.macro
#macro end
*/

	//#macro use CONV_PRIMITIVE_STRUCT(Tribyte  , 3,  uint,  UInt)
	//#macro use CONV_PRIMITIVE_STRUCT(Pentabyte, 5, ulong, ULong)
	//#macro use CONV_PRIMITIVE_STRUCT(Hexabyte , 6, ulong, ULong)
	//#macro use CONV_PRIMITIVE_STRUCT(Heptabyte, 7, ulong, ULong)

	//#macro use PRIMITIVE_STRUCT(LargeInt, 12)
	//#macro use PRIMITIVE_STRUCT(  BigInt, 16)
	//#macro use PRIMITIVE_STRUCT(GreatInt, 24)
	//#macro use PRIMITIVE_STRUCT( HugeInt, 32)
	//#macro use PRIMITIVE_STRUCT(GiantInt, 64)
}