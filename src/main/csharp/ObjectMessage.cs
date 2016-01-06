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
	/// Represents an object message which contains a serializable .NET object.
	/// </summary>
	class ObjectMessage : Apache.NMS.XMS.Message, Apache.NMS.IObjectMessage
	{
		#region Constructors and access to internal map message

		/// <summary>
		/// Internal IBM XMS object message.
		/// </summary>
		public IBM.XMS.IObjectMessage xmsObjectMessage
		{
			get { return (IBM.XMS.IObjectMessage)this.xmsMessage; }
			set { this.xmsMessage = value; }
		}

		/// <summary>
		/// Constructs a <c>MapMessage</c> object.
		/// </summary>
		/// <param name="message">XMS map message.</param>
		public ObjectMessage(IBM.XMS.IObjectMessage message)
			: base(message)
		{
		}

		#endregion

		#region IObjectMessage Members

		/// <summary>
		/// Object message body.
		/// </summary>
		public object Body
		{
			get { return this.xmsObjectMessage.Object; }
			set { this.xmsObjectMessage.Object = value; }
		}

		#endregion
	}
}
