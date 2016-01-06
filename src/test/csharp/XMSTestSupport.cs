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
using System.Xml;
using Apache.NMS.Test;
using Apache.NMS.Util;
using NUnit.Framework;

using Apache.NMS.XMS;

namespace Apache.NMS.XMS.Test
{
	/// <summary>
	/// Useful class for test cases support.
	/// </summary>
	public class XMSTestSupport : NMSTestSupport
	{
		/// <summary>
		/// Gets the environment variable name for the configuration file path.
		/// </summary>
		/// <returns>Environment variable name</returns>
		public override string GetConfigEnvVarName()
		{
			return "XMSTESTCONFIGPATH";
		}

		/// <summary>
		/// Gets the default name for the configuration filename.
		/// </summary>
		/// <returns>Default name of the configuration filename</returns>
		public override string GetDefaultConfigFileName()
		{
			return "xmsprovider-test.config";
		}

		/// <summary>
		/// Gets a new client id.
		/// </summary>
		/// <returns>Client id</returns>
		public override string GetTestClientId()
		{
			return base.GetTestClientId();
			//return null;
		}

		/// <summary>
		/// Create a new connection to the broker.
		/// </summary>
		/// <param name="newClientId">Client ID of the new connection.</param>
		/// <returns>New connection</returns>
		public override IConnection CreateConnection(string newClientId)
		{
			IConnection newConnection;

			// IBM.XMS throws an exception if attempting to set the ClientId
			// on the Connection after it was created.
			// In a multithreaded environment, it would probably be best to
			// create as many factories as client ids, rather than change the
			// client id on the factory before creating a connection. Plus it
			// wouldn't be a good idea to protect the code section through a
			// semaphore, since connections creation takes a long time to be
			// performed.
			if(newClientId != null)
			{
				((Apache.NMS.XMS.ConnectionFactory)Factory).ClientId = newClientId;
			}

			if(this.userName == null)
			{
				newConnection = Factory.CreateConnection();
			}
			else
			{
				newConnection = Factory.CreateConnection(userName, passWord);
			}

			Assert.IsNotNull(newConnection, "Connection not created");

			return newConnection;
		}

		ISession cleanupSession = null;
		/// <summary>
		/// Gets a clear destination. This will try to delete an existing
		/// destination and re-create it.
		/// </summary>
		/// <param name="session">Session</param>
		/// <param name="destinationURI">Destination URI</param>
		/// <returns>Clear destination</returns>
		public override IDestination GetClearDestination(ISession session,
			string destinationURI)
		{
			IDestination destination = SessionUtil.GetDestination(session, destinationURI);

			// Destination exists. Can't use the given session to clean up:
			// once used synchronously, IBM XMS doesn't allow the session to be
			// used asynchronously. Therefore, we create a specific synchronous
			// session to perform cleanups.
			if(cleanupSession == null)
			{
				IConnection cleanupConnection = CreateConnection("Cleanup");
				cleanupSession = cleanupConnection.CreateSession();
			}

			IDestination cleanupDestination = SessionUtil.GetDestination(cleanupSession, destinationURI);

			using(IMessageConsumer consumer = cleanupSession.CreateConsumer(destination))
			{
				while(consumer.Receive(TimeSpan.FromMilliseconds(750)) != null)
				{
				}
			}

			return destination;
		}

	}
}
