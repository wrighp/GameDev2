using System.Runtime.Serialization;
using UnityEngine;

public class Color32SerializationSurrogate :ISerializationSurrogate
{
	#region ISerializationSurrogate implementation

	public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
	{
		Color32 c = (Color32) obj;
		info.AddValue("v", new byte[]{c.r,c.g,c.b,c.a}, typeof(byte[]));
	}

	public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
	{
		Color32 c = (Color32) obj;
		byte[] values = (byte[])info.GetValue("v",typeof(byte[]));
		c.r = values[0]; c.g = values[1]; c.b = values [2]; c.a = values[3];
		return (c);
	}

	#endregion
}
