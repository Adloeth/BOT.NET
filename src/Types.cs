using System.Runtime.InteropServices;
using System.Numerics;

namespace FFF
{
	public struct Primitive
	{
		private object variant;

		public Primitive() { this.variant = 0; }
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

		public static bool IsPrimitive(System.Type type)
		{
			/*#macro decl IS_PRIMITIVE(primType)
				|| type == typeof(primType)
			#macro end*/

			return type == typeof(bool)
			//#macro use IS_PRIMITIVE(     byte)
			//#macro use IS_PRIMITIVE(    sbyte)
			//#macro use IS_PRIMITIVE(    short)
			//#macro use IS_PRIMITIVE(   ushort)
			//#macro use IS_PRIMITIVE(     Half)
			//#macro use IS_PRIMITIVE(  Tribyte)
			//#macro use IS_PRIMITIVE(      int)
			//#macro use IS_PRIMITIVE(     uint)
			//#macro use IS_PRIMITIVE(    float)
			//#macro use IS_PRIMITIVE(Pentabyte)
			//#macro use IS_PRIMITIVE( Hexabyte)
			//#macro use IS_PRIMITIVE(Heptabyte)
			//#macro use IS_PRIMITIVE(     long)
			//#macro use IS_PRIMITIVE(    ulong)
			//#macro use IS_PRIMITIVE(   double)
			//#macro use IS_PRIMITIVE( LargeInt)
			//#macro use IS_PRIMITIVE(   BigInt)
			//#macro use IS_PRIMITIVE( GreatInt)
			//#macro use IS_PRIMITIVE(  HugeInt)
			//#macro use IS_PRIMITIVE( GiantInt)
			;
		}

		public bool IsDefault()
		{
			/*#macro decl IS_CS_PRIMITIVE_DEFAULT(primType)
				if(Is<primType>()) return As<primType>() == (primType)0;
			#macro end*/
			/*#macro decl IS_CU_PRIMITIVE_DEFAULT(primType)
				if(Is<primType>()) return As<primType>().IsDefault();
			#macro end*/

			if(Is<bool>()) return !As<bool>();
			//#macro use IS_CS_PRIMITIVE_DEFAULT(     byte)
			//#macro use IS_CS_PRIMITIVE_DEFAULT(    sbyte)
			//#macro use IS_CS_PRIMITIVE_DEFAULT(    short)
			//#macro use IS_CS_PRIMITIVE_DEFAULT(   ushort)
			//#macro use IS_CS_PRIMITIVE_DEFAULT(     Half)
			//#macro use IS_CU_PRIMITIVE_DEFAULT(  Tribyte)
			//#macro use IS_CS_PRIMITIVE_DEFAULT(      int)
			//#macro use IS_CS_PRIMITIVE_DEFAULT(     uint)
			//#macro use IS_CS_PRIMITIVE_DEFAULT(    float)
			//#macro use IS_CU_PRIMITIVE_DEFAULT(Pentabyte)
			//#macro use IS_CU_PRIMITIVE_DEFAULT( Hexabyte)
			//#macro use IS_CU_PRIMITIVE_DEFAULT(Heptabyte)
			//#macro use IS_CS_PRIMITIVE_DEFAULT(     long)
			//#macro use IS_CS_PRIMITIVE_DEFAULT(    ulong)
			//#macro use IS_CS_PRIMITIVE_DEFAULT(   double)
			//#macro use IS_CU_PRIMITIVE_DEFAULT( LargeInt)
			//#macro use IS_CU_PRIMITIVE_DEFAULT(   BigInt)
			//#macro use IS_CU_PRIMITIVE_DEFAULT( GreatInt)
			//#macro use IS_CU_PRIMITIVE_DEFAULT(  HugeInt)
			//#macro use IS_CU_PRIMITIVE_DEFAULT( GiantInt)
			throw new Exception("Invalid primitive type");
		}

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