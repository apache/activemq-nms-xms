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

namespace Apache.NMS.XMS
{
	public class MessageProperties : Apache.NMS.IPrimitiveMap
	{
		public IBM.XMS.IMessage xmsMessage;

		public MessageProperties(IBM.XMS.IMessage message)
		{
			this.xmsMessage = message;
		}

		#region IPrimitiveMap Members

		public void Clear()
		{
			try
			{
				this.xmsMessage.ClearProperties();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public bool Contains(object key)
		{
			return this.xmsMessage.PropertyExists(key.ToString());
		}

		public void Remove(object key)
		{
			try
			{
				// Best guess at equivalent implementation.
				this.xmsMessage.SetObjectProperty(key.ToString(), null);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public int Count
		{
			get
			{
				int count = 0;
				try
				{
					IEnumerator propertyNamesEnumerator =
						this.xmsMessage.PropertyNames;

					if(null != propertyNamesEnumerator)
					{
						while(propertyNamesEnumerator.MoveNext())
						{
							count++;
						}
					}
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}

				return count;
			}
		}

		public ICollection Keys
		{
			get
			{
				ArrayList keys = new ArrayList();

				try
				{
					foreach(string propertyName in XMSConvert.ToEnumerable(this.xmsMessage.PropertyNames))
					{
						keys.Add(propertyName);
					}
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}

				return keys;
			}
		}

		public ICollection Values
		{
			get
			{
				ArrayList values = new ArrayList();

				try
				{
					foreach(string propertyName in XMSConvert.ToEnumerable(this.xmsMessage.PropertyNames))
					{
						values.Add(this.xmsMessage.GetObjectProperty(propertyName));
					}
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}

				return values;
			}
		}

		public object this[string key]
		{
			get
			{
				try
				{
					return this.xmsMessage.GetObjectProperty(key);
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
					this.xmsMessage.SetObjectProperty(key, value);
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		public string GetString(string key)
		{
			try
			{
				return this.xmsMessage.GetStringProperty(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}

		public void SetString(string key, string value)
		{
			try
			{
				this.xmsMessage.SetStringProperty(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public bool GetBool(string key)
		{
			try
			{
				return this.xmsMessage.GetBooleanProperty(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return false;
			}
		}

		public void SetBool(string key, bool value)
		{
			try
			{
				this.xmsMessage.SetBooleanProperty(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public byte GetByte(string key)
		{
			try
			{
				return this.xmsMessage.GetByteProperty(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		public void SetByte(string key, byte value)
		{
			try
			{
				this.xmsMessage.SetByteProperty(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public char GetChar(string key)
		{
			try
			{
				return this.xmsMessage.GetCharProperty(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return (char) 0;
			}
		}

		public void SetChar(string key, char value)
		{
			try
			{
				this.xmsMessage.SetCharProperty(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public short GetShort(string key)
		{
			try
			{
				return this.xmsMessage.GetShortProperty(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		public void SetShort(string key, short value)
		{
			try
			{
				this.xmsMessage.SetShortProperty(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public int GetInt(string key)
		{
			try
			{
				return this.xmsMessage.GetIntProperty(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		public void SetInt(string key, int value)
		{
			try
			{
				this.xmsMessage.SetIntProperty(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public long GetLong(string key)
		{
			try
			{
				return this.xmsMessage.GetLongProperty(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		public void SetLong(string key, long value)
		{
			try
			{
				this.xmsMessage.SetLongProperty(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public float GetFloat(string key)
		{
			try
			{
				return this.xmsMessage.GetFloatProperty(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		public void SetFloat(string key, float value)
		{
			try
			{
				this.xmsMessage.SetFloatProperty(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public double GetDouble(string key)
		{
			try
			{
				return this.xmsMessage.GetDoubleProperty(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return 0;
			}
		}

		public void SetDouble(string key, double value)
		{
			try
			{
				this.xmsMessage.SetDoubleProperty(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public IList GetList(string key)
		{
			try
			{
				return (IList) this.xmsMessage.GetObjectProperty(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}

		public void SetList(string key, IList list)
		{
			try
			{
				this.xmsMessage.SetObjectProperty(key, list);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public void SetBytes(string key, byte[] value)
		{
			try
			{
				this.xmsMessage.SetBytesProperty(key, value);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public void SetBytes(string key, byte[] value, int offset, int length)
		{
			try
			{
				byte[] byteSection = new byte[length];

				for(int srcIndex = offset, destIndex = 0; srcIndex < (offset + length); srcIndex++, destIndex++)
				{
					byteSection[destIndex] = value[srcIndex];
				}

				this.xmsMessage.SetBytesProperty(key, byteSection);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		public byte[] GetBytes(string key)
		{
			try
			{
				return (byte[]) this.xmsMessage.GetBytesProperty(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}

		public IDictionary GetDictionary(string key)
		{
			try
			{
				return (IDictionary) this.xmsMessage.GetObjectProperty(key);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}

		public void SetDictionary(string key, IDictionary dictionary)
		{
			try
			{
				this.xmsMessage.SetObjectProperty(key, dictionary);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion
	}
}
