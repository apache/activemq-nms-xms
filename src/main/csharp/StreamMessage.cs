/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using Apache.NMS;
using Apache.NMS.Util;
using Apache.NMS.XMS.Util;
using IBM.XMS;

namespace Apache.NMS.XMS
{
    /// <summary>
    /// A StreamMessage object is used to send a stream of primitive types in the 
    /// .NET programming language. It is filled and read sequentially. It inherits 
    /// from the Message interface and adds a stream message body.
    /// </summary>
	class StreamMessage : Apache.NMS.XMS.Message, Apache.NMS.IStreamMessage
	{
		#region Constructors and access to internal stream message

		/// <summary>
		/// Internal IBM XMS stream message.
		/// </summary>
		public IBM.XMS.IStreamMessage xmsStreamMessage
		{
			get { return (IBM.XMS.IStreamMessage)this.xmsMessage; }
			set { this.xmsMessage = value; }
		}

		/// <summary>
		/// Constructs a <c>StreamMessage</c> object.
		/// </summary>
		/// <param name="message">XMS stream message.</param>
		public StreamMessage(IBM.XMS.IStreamMessage message)
			: base(message)
		{
		}

		#endregion

		#region IStreamMessage Members

		#region Reset method

		/// <summary>
		/// Resets the contents of the stream message body.
		/// </summary>
		public void Reset()
		{
			try
			{
				this.xmsStreamMessage.Reset();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region Read methods

		/// <summary>
		/// Reads a boolean from the stream message.
		/// </summary>
		/// <returns>A <see cref="System.Boolean"/></returns>
		public bool ReadBoolean()
		{
			try
			{
				return this.xmsStreamMessage.ReadBoolean();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return false;
			}
		}

		/// <summary>
		/// Reads a byte from the stream message.
		/// </summary>
		/// <returns>A <see cref="System.Byte"/></returns>
		public byte ReadByte()
		{
			try
			{
				return this.xmsStreamMessage.ReadByte();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		/// <summary>
		/// Reads a byte array from the stream message.
		/// </summary>
		/// <param name="value">A <see cref="System.Byte"/> array</param>
		/// <returns>The total number of bytes read into the buffer, or -1 if
		/// there is no more data because the end of the byte field has been
		/// reached</returns>
		public int ReadBytes(byte[] value)
		{
			try
			{
				return this.xmsStreamMessage.ReadBytes(value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		/// <summary>
		/// Reads a character from the stream message.
		/// </summary>
		/// <returns>A <see cref="System.Char"/></returns>
		public char ReadChar()
		{
			try
			{
				return this.xmsStreamMessage.ReadChar();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return (char) 0;
			}
		}

		/// <summary>
		/// Reads a 16 bits (short) integer number from the stream message.
		/// </summary>
		/// <returns>A <see cref="System.Int16"/></returns>
		public short ReadInt16()
		{
			try
			{
				return this.xmsStreamMessage.ReadShort();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		/// <summary>
		/// Reads a 32 bits (int) integer number from the stream message.
		/// </summary>
		/// <returns>A <see cref="System.Int32"/></returns>
		public int ReadInt32()
		{
			try
			{
				return this.xmsStreamMessage.ReadInt();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		/// <summary>
		/// Reads a 64 bits (long) integer number from the stream message.
		/// </summary>
		/// <returns>A <see cref="System.Int64"/></returns>
		public long ReadInt64()
		{
			try
			{
				return this.xmsStreamMessage.ReadLong();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		/// <summary>
		/// Reads a single precision floating point number from the stream
		/// message.
		/// </summary>
		/// <returns>A <see cref="System.Single"/></returns>
		public float ReadSingle()
		{
			try
			{
				return this.xmsStreamMessage.ReadFloat();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		/// <summary>
		/// Reads a double precision floating point number from the stream
		/// message.
		/// </summary>
		/// <returns>A <see cref="System.Double"/></returns>
		public double ReadDouble()
		{
			try
			{
				return this.xmsStreamMessage.ReadDouble();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		/// <summary>
		/// Reads a character string from the stream message.
		/// </summary>
		/// <returns>A <see cref="System.String"/></returns>
		public string ReadString()
		{
			try
			{
				return this.xmsStreamMessage.ReadString();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}

		/// <summary>
		/// Reads an object from the stream message.
		/// </summary>
		/// <returns>A <see cref="System.Object"/></returns>
		public object ReadObject()
		{
			try
			{
				return this.xmsStreamMessage.ReadObject();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		#endregion

		#region Write methods

		/// <summary>
		/// Writes a boolean to the stream message.
		/// </summary>
		/// <param name="value">A <see cref="System.Boolean"/></param>
		public void WriteBoolean(bool value)
		{
			try
			{
				this.xmsStreamMessage.WriteBoolean(value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Writes a byte to the stream message.
		/// </summary>
		/// <param name="value">A <see cref="System.Byte"/></param>
		public void WriteByte(byte value)
		{
			try
			{
				this.xmsStreamMessage.WriteByte(value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Writes a byte array to the stream message.
		/// </summary>
		/// <param name="value">A <see cref="System.Byte"/> array</param>
		public void WriteBytes(byte[] value)
		{
			try
			{
				this.xmsStreamMessage.WriteBytes(value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Writes a portion of a byte array as a byte array field to the
		/// stream message.
		/// </summary>
		/// <param name="value">A <see cref="System.Byte"/> array</param>
		/// <param name="offset">A <see cref="System.Int32"/> value that
		/// indicates the point in the buffer to begin writing to the stream
		/// message.</param>
		/// <param name="length">A <see cref="System.Int32"/> value that
		/// indicates how many bytes in the buffer to write to the stream
		/// message.</param>
		public void WriteBytes(byte[] value, int offset, int length)
		{
			try
			{
				this.xmsStreamMessage.WriteBytes(value, offset, length);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Writes a character to the stream message.
		/// </summary>
		/// <param name="value">A <see cref="System.Char"/></param>
		public void WriteChar(char value)
		{
			try
			{
				this.xmsStreamMessage.WriteChar(value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Writes a 16 bts (short) integer to the stream message.
		/// </summary>
		/// <param name="value">A <see cref="System.Int16"/></param>
		public void WriteInt16(short value)
		{
			try
			{
				this.xmsStreamMessage.WriteShort(value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Writes a 32 bts (int) integer to the stream message.
		/// </summary>
		/// <param name="value">A <see cref="System.Int32"/></param>
		public void WriteInt32(int value)
		{
			try
			{
				this.xmsStreamMessage.WriteInt(value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Writes a 64 bts (long) integer to the stream message.
		/// </summary>
		/// <param name="value">A <see cref="System.Int64"/></param>
		public void WriteInt64(long value)
		{
			try
			{
				this.xmsStreamMessage.WriteLong(value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Writes a single precision floating point number to the stream
		/// message.
		/// </summary>
		/// <param name="value">A <see cref="System.Single"/></param>
		public void WriteSingle(float value)
		{
			try
			{
				this.xmsStreamMessage.WriteFloat(value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Writes a double precision floating point number to the stream
		/// message.
		/// </summary>
		/// <param name="value">A <see cref="System.Double"/></param>
		public void WriteDouble(double value)
		{
			try
			{
				this.xmsStreamMessage.WriteDouble(value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Writes a character string to the stream
		/// message.
		/// </summary>
		/// <param name="value">A <see cref="System.String"/></param>
		public void WriteString(string value)
		{
			try
			{
				this.xmsStreamMessage.WriteString(value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Writes an object to the stream
		/// message.
		/// </summary>
		/// <param name="value">A <see cref="System.Object"/></param>
		public void WriteObject(object value)
		{
			try
			{
				this.xmsStreamMessage.WriteObject(value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#endregion
	}
}
