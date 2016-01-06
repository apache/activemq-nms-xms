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

namespace Apache.NMS.XMS
{
	class TemporaryTopic : Apache.NMS.XMS.Topic, Apache.NMS.ITemporaryTopic
	{
		#region Constructors and destructors

		/// <summary>
		/// Constructs a <c>TemporaryTopic</c> object.
		/// </summary>
		/// <param name="temporaryTopic">IBM XMS queue</param>
		public TemporaryTopic(IBM.XMS.IDestination temporaryTopic)
			: base(temporaryTopic, true)
		{
		}

		#endregion

		#region ITemporaryTopic Members

		/// <summary>
		/// Deletes the temporary topic.
		/// </summary>
		public void Delete()
		{
			// IBM.XMS does not provide a method for deleting a destination.
			// Should we throw an exception or ignore the request ?
			//this.xmsDestination.Delete();
		}

		#endregion

		#region ToString

		/// <summary>
		/// Returns a string representation of this instance.
		/// </summary>
		/// <returns>string representation of this instance</returns>
		public override System.String ToString()
		{
			return "temp-topic://" + TopicName;
		}

		#endregion
	}
}
