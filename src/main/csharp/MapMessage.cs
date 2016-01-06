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
using System.Collections;
using Apache.NMS;
using Apache.NMS.Util;
using Apache.NMS.XMS.Util;
using IBM.XMS;

namespace Apache.NMS.XMS
{
	/// <summary>
	/// Represents a map message which contains key and value pairs which are
	/// of primitive types.
	/// </summary>
	class MapMessage : Apache.NMS.XMS.Message, Apache.NMS.IMapMessage,
		Apache.NMS.IPrimitiveMap
	{
		#region Constructors and access to internal map message

		/// <summary>
		/// Internal IBM XMS map message.
		/// </summary>
		public IBM.XMS.IMapMessage xmsMapMessage
		{
			get { return (IBM.XMS.IMapMessage)(this.xmsMessage); }
			set { this.xmsMessage = value; }
		}

		/// <summary>
		/// Constructs a <c>MapMessage</c> object.
		/// </summary>
		/// <param name="message">XMS map message.</param>
		public MapMessage(IBM.XMS.IMapMessage message)
			: base(message)
		{
		}

		#endregion

		#region IMapMessage Members

		public Apache.NMS.IPrimitiveMap Body
		{
			get { return this; }
		}

		#endregion

		#region IPrimitiveMap Members

		#region General methods

		/// <summary>
		/// Clears the contents of the message body.
		/// </summary>
		public void Clear()
		{
			try
			{
				this.ReadOnlyBody = false;
				this.xmsMapMessage.ClearBody();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Checks if the body contains the specified item.
		/// </summary>
		/// <param name="key">Item key.</param>
		public bool Contains(object key)
		{
			try
			{
				return this.xmsMapMessage.ItemExists(key.ToString());
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return false;
			}
		}

		/// <summary>
		/// Removes an item from the map message body.
		/// </summary>
		/// <param name="key">Item key.</param>
		public void Remove(object key)
		{
			try
			{
				// Best guess at equivalent implementation.
				this.xmsMapMessage.SetObject(key.ToString(), null);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Count of key/value pairs in the message body.
		/// </summary>
		public int Count
		{
			get
			{
				int count = 0;

				try
				{
					IEnumerator mapNames = this.xmsMapMessage.MapNames;
					while(mapNames.MoveNext())
					{
						count++;
					}
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}

				return count;
			}
		}

		/// <summary>
		/// The collection of keys in the mep message body.
		/// </summary>
		public ICollection Keys
		{
			get
			{
				ArrayList keys = new ArrayList();

				try
				{
					IEnumerator mapNames = this.xmsMapMessage.MapNames;
					while(mapNames.MoveNext())
					{
						keys.Add(mapNames.Current);
					}
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}

				return keys;
			}
		}

		/// <summary>
		/// The collection of values in the mep message body.
		/// </summary>
		public ICollection Values
		{
			get
			{
				ArrayList values = new ArrayList();

				try
				{
					IEnumerator mapNames = this.xmsMapMessage.MapNames;
					while(mapNames.MoveNext())
					{
						string key = (string)mapNames.Current;
						values.Add(this.xmsMapMessage.GetObject(key));
					}
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}

				return values;
			}
		}

		/// <summary>
		/// Accesses an item by its key.
		/// </summary>
		/// <param name="key">Item key.</param>
		public object this[string key]
		{
			get
			{
				try
				{
					return this.xmsMapMessage.GetObject(key);
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
					return null;
				}
			}
			set
			{
				try
				{
					this.xmsMapMessage.SetObject(key, value);
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		#endregion

		#region String items

		/// <summary>
		/// Gets the value of a <c>string</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <returns>Item value.</returns>
		public string GetString(string key)
		{
			try
			{
				return this.xmsMapMessage.GetString(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}

		/// <summary>
		/// Sets the value of a <c>string</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="value">Item value.</param>
		public void SetString(string key, string value)
		{
			try
			{
				this.xmsMapMessage.SetString(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region Boolean items

		/// <summary>
		/// Gets the value of a <c>bool</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <returns>Item value.</returns>
		public bool GetBool(string key)
		{
			try
			{
				return this.xmsMapMessage.GetBoolean(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return false;
			}
		}

		/// <summary>
		/// Sets the value of a <c>bool</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="value">Item value.</param>
		public void SetBool(string key, bool value)
		{
			try
			{
				this.xmsMapMessage.SetBoolean(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region Byte items

		/// <summary>
		/// Gets the value of a <c>byte</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <returns>Item value.</returns>
		public byte GetByte(string key)
		{
			try
			{
				return this.xmsMapMessage.GetByte(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		/// <summary>
		/// Sets the value of a <c>byte</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="value">Item value.</param>
		public void SetByte(string key, byte value)
		{
			try
			{
				this.xmsMapMessage.SetByte(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region Char items

		/// <summary>
		/// Gets the value of a <c>char</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <returns>Item value.</returns>
		public char GetChar(string key)
		{
			try
			{
				return this.xmsMapMessage.GetChar(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return (char) 0;
			}
		}

		/// <summary>
		/// Sets the value of a <c>char</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="value">Item value.</param>
		public void SetChar(string key, char value)
		{
			try
			{
				this.xmsMapMessage.SetChar(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region Short items

		/// <summary>
		/// Gets the value of a 16 bits <c>short</c> integer item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <returns>Item value.</returns>
		public short GetShort(string key)
		{
			try
			{
				return this.xmsMapMessage.GetShort(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		/// <summary>
		/// Sets the value of a 16 bits <c>short</c> integer item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="value">Item value.</param>
		public void SetShort(string key, short value)
		{
			try
			{
				this.xmsMapMessage.SetShort(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region Int items

		/// <summary>
		/// Gets the value of a 32 bits <c>int</c> integer item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <returns>Item value.</returns>
		public int GetInt(string key)
		{
			try
			{
				return this.xmsMapMessage.GetInt(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		/// <summary>
		/// Sets the value of a 32 bits <c>int</c> integer item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="value">Item value.</param>
		public void SetInt(string key, int value)
		{
			try
			{
				this.xmsMapMessage.SetInt(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region Long items

		/// <summary>
		/// Gets the value of a 64 bits <c>long</c> integer item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <returns>Item value.</returns>
		public long GetLong(string key)
		{
			try
			{
				return this.xmsMapMessage.GetLong(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		/// <summary>
		/// Sets the value of a 64 bits <c>long</c> integer item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="value">Item value.</param>
		public void SetLong(string key, long value)
		{
			try
			{
				this.xmsMapMessage.SetLong(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region Float items

		/// <summary>
		/// Gets the value of a <c>float</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <returns>Item value.</returns>
		public float GetFloat(string key)
		{
			try
			{
				return this.xmsMapMessage.GetFloat(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		/// <summary>
		/// Sets the value of a <c>float</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="value">Item value.</param>
		public void SetFloat(string key, float value)
		{
			try
			{
				this.xmsMapMessage.SetFloat(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region Double items

		/// <summary>
		/// Gets the value of a <c>double</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <returns>Item value.</returns>
		public double GetDouble(string key)
		{
			try
			{
				return this.xmsMapMessage.GetDouble(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		/// <summary>
		/// Sets the value of a <c>double</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="value">Item value.</param>
		public void SetDouble(string key, double value)
		{
			try
			{
				this.xmsMapMessage.SetDouble(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region List items

		/// <summary>
		/// Gets the value of an <c>IList</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <returns>Item value.</returns>
		public IList GetList(string key)
		{
			try
			{
				return (IList) this.xmsMapMessage.GetObject(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}

		/// <summary>
		/// Sets the value of an <c>IList</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="list">Item value.</param>
		public void SetList(string key, IList list)
		{
			try
			{
				this.xmsMapMessage.SetObject(key, list);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region Bytes array items

		/// <summary>
		/// Gets the value of a <c>byte[]</c> byte array item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <returns>Item value.</returns>
		public byte[] GetBytes(string key)
		{
			try
			{
				return this.xmsMapMessage.GetBytes(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}

		/// <summary>
		/// Sets the value of a <c>byte[]</c> byte array item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="value">Item value.</param>
		public void SetBytes(string key, byte[] value)
		{
			try
			{
				this.xmsMapMessage.SetBytes(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Sets the value of a <c>byte[]</c> byte array item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="value">Byte array from which value is extracted.</param>
		/// <param name="offset">Index of first byte to extract.</param>
		/// <param name="length">Number of bytes to extract.</param>
		public void SetBytes(string key, byte[] value, int offset, int length)
		{
			try
			{
				this.xmsMapMessage.SetBytes(key, value, offset, length);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region Dictionary items

		/// <summary>
		/// Gets the value of an <c>IDictionary</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <returns>Item value.</returns>
		public IDictionary GetDictionary(string key)
		{
			try
			{
				return (IDictionary) this.xmsMapMessage.GetObject(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}

		/// <summary>
		/// Sets the value of an <c>IDictionary</c> item.
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="dictionary">Item value.</param>
		public void SetDictionary(string key, IDictionary dictionary)
		{
			try
			{
				this.xmsMapMessage.SetObject(key, dictionary);
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
