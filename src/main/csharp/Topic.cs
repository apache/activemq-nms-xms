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
	public class Topic : Apache.NMS.XMS.Destination, Apache.NMS.ITopic
	{
		#region Constructors and destructors

		/// <summary>
		/// Constructs a <c>Topic</c> object.
		/// </summary>
		/// <param name="topic">IBM XMS topic</param>
		public Topic(IBM.XMS.IDestination topic)
			: base(topic)
		{
		}

		/// <summary>
		/// Constructs a <c>Topic</c> object.
		/// </summary>
		/// <param name="topic">IBM XMS topic</param>
		/// <param name="isTemporary">Whether the topic is temporary</param>
		public Topic(IBM.XMS.IDestination topic, bool isTemporary)
			: base(topic, isTemporary)
		{
		}

		#endregion

        #region ITopic Members

		public string TopicName
		{
			get { return this.xmsDestination.Name; }
		}

		#endregion

		#region ToString

		/// <summary>
		/// Returns a String representation of this instance.
		/// </summary>
		/// <returns>String representation of this instance</returns>
		public override System.String ToString()
		{
			return "topic://" + TopicName;
		}

		#endregion
	}
}