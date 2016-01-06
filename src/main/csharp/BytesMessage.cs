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
	class BytesMessage : Apache.NMS.XMS.Message, Apache.NMS.IBytesMessage
	{
		public IBM.XMS.IBytesMessage xmsBytesMessage
		{
			get { return (IBM.XMS.IBytesMessage)this.xmsMessage; }
			set { this.xmsMessage = value; }
		}

		public BytesMessage(IBM.XMS.IBytesMessage message)
			: base(message)
		{
		}

		#region IBytesMessage Members

		public byte[] Content
		{
			get
			{
				int contentLength = (int) this.xmsBytesMessage.BodyLength;
				byte[] msgContent = new byte[contentLength];

				this.xmsBytesMessage.Reset();
				this.xmsBytesMessage.ReadBytes(msgContent, contentLength);
				return msgContent;
			}

			set
			{
				this.ReadOnlyBody = false;
				this.xmsBytesMessage.ClearBody();
				this.xmsBytesMessage.WriteBytes(value, 0, value.Length);
			}
		}

		public long BodyLength
		{
			get { return this.xmsBytesMessage.BodyLength; }
		}

		public bool ReadBoolean()
		{
			return this.xmsBytesMessage.ReadBoolean();
		}

		public byte ReadByte()
		{
			return (byte) this.xmsBytesMessage.ReadByte();
		}

		public int ReadBytes(byte[] value, int length)
		{
			return this.xmsBytesMessage.ReadBytes(value, length);
		}

		public int ReadBytes(byte[] value)
		{
			return this.xmsBytesMessage.ReadBytes(value);
		}

		public char ReadChar()
		{
			return this.xmsBytesMessage.ReadChar();
		}

		public double ReadDouble()
		{
			return this.xmsBytesMessage.ReadDouble();
		}

		public short ReadInt16()
		{
			return this.xmsBytesMessage.ReadShort();
		}

		public int ReadInt32()
		{
			return this.xmsBytesMessage.ReadInt();
		}

		public long ReadInt64()
		{
			return this.xmsBytesMessage.ReadLong();
		}

		public float ReadSingle()
		{
			return this.xmsBytesMessage.ReadFloat();
		}

		public string ReadString()
		{
			return this.xmsBytesMessage.ReadUTF();
		}

		public void Reset()
		{
			this.xmsBytesMessage.Reset();
		}

		public void WriteBoolean(bool value)
		{
			this.xmsBytesMessage.WriteBoolean(value);
		}

		public void WriteByte(byte value)
		{
			this.xmsBytesMessage.WriteByte(value);
		}

		public void WriteBytes(byte[] value, int offset, int length)
		{
			this.xmsBytesMessage.WriteBytes(value, offset, length);
		}

		public void WriteBytes(byte[] value)
		{
			this.xmsBytesMessage.WriteBytes(value);
		}

		public void WriteChar(char value)
		{
			this.xmsBytesMessage.WriteChar(value);
		}

		public void WriteDouble(double value)
		{
			this.xmsBytesMessage.WriteDouble(value);
		}

		public void WriteInt16(short value)
		{
			this.xmsBytesMessage.WriteShort(value);
		}

		public void WriteInt32(int value)
		{
			this.xmsBytesMessage.WriteInt(value);
		}

		public void WriteInt64(long value)
		{
			this.xmsBytesMessage.WriteLong(value);
		}

		public void WriteObject(object value)
		{
			this.xmsBytesMessage.WriteObject(value);
		}

		public void WriteSingle(float value)
		{
			this.xmsBytesMessage.WriteFloat(value);
		}

		public void WriteString(string value)
		{
			this.xmsBytesMessage.WriteUTF(value);
		}

		#endregion
	}
}
