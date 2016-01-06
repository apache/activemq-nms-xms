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
	/// Represents a text based message.
	/// </summary>
	class TextMessage : Apache.NMS.XMS.Message, Apache.NMS.ITextMessage
	{
		#region Constructors and access to internal stream message

		/// <summary>
		/// Internal IBM XMS text message.
		/// </summary>
		public IBM.XMS.ITextMessage xmsTextMessage
		{
			get { return (IBM.XMS.ITextMessage)this.xmsMessage; }
			set { this.xmsMessage = value; }
		}

		/// <summary>
		/// Constructs a <c>TextMessage</c> object.
		/// </summary>
		/// <param name="message">XMS text message.</param>
		public TextMessage(IBM.XMS.ITextMessage message)
			: base(message)
		{
		}

		#endregion

		#region ITextMessage Members

		/// <summary>
		/// The text contents of the message body.
		/// </summary>
		public string Text
		{
			get
			{
				try
				{
					return this.xmsTextMessage.Text;
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
					this.xmsTextMessage.Text = value;
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		#endregion
	}
}
