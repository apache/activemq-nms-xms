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
	public class Queue : Apache.NMS.XMS.Destination, Apache.NMS.IQueue
	{
		#region Constructors and destructors

		/// <summary>
		/// Constructs a <c>Queue</c> object.
		/// </summary>
		/// <param name="queue">IBM XMS queue</param>
		public Queue(IBM.XMS.IDestination queue)
			: base(queue)
		{
		}

		/// <summary>
		/// Constructs a <c>Queue</c> object.
		/// </summary>
		/// <param name="queue">IBM XMS queue</param>
		/// <param name="isTemporary">Whether the queue is temporary</param>
		public Queue(IBM.XMS.IDestination queue, bool isTemporary)
			: base(queue, isTemporary)
		{
		}

		#endregion
        
        #region IQueue Members

		/// <summary>
		/// Queue name.
		/// </summary>
		public string QueueName
		{
			get { return this.xmsDestination.Name; }
		}

		#endregion

		#region ToString

		/// <summary>
		/// Returns a string representation of this instance.
		/// </summary>
		/// <returns>String representation of this instance</returns>
		public override System.String ToString()
		{
			return "queue://" + QueueName;
		}

		#endregion
	}
}