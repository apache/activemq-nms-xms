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
using System.Collections.Specialized;
using Apache.NMS.XMS.Util;
using Apache.NMS.Policies;
using Apache.NMS.Util;
using IBM.XMS;

namespace Apache.NMS.XMS
{
	/// <summary>
	/// A Factory that can establish NMS connections to IBM MQ.
	/// </summary>
	/// <remarks>
	/// XMS connection factories can either be created from definitions
	/// administered in a repository ("administered object"), or created
	/// from an <c>XMSFactoryFactory</c>.
	///
	/// Addressable repositories for administered objects are:
	/// - file system context
	///   (URL format: "file://Path").
	/// - LDAP context
	///   (URL format: "ldap:[Hostname][:Port]["/"[DistinguishedName]]").
	/// - WSS context:
	///   (URL format: "http://Url", "cosnaming://Url" or "wsvc://Url").
	///
	/// A non-administered object is instanciated for a specific protocol:
	/// - WebSphere Application Server Service Integration Bus
	///   (protocol prefix: <c>"wpm:"</c>; XMS key: <c>XMSC.CT_WPM</c>).
	/// - IBM Integration Bus using WebSphere MQ Real-Time Transport
	///   (protocol prefix: <c>"rtt:"</c>; XMS key: <c>XMSC.CT_RTT</c>).
	/// - WebSphere MQ queue manager
	///   (protocol prefix: <c>"wmq:"</c>; XMS key: <c>XMSC.CT_WMQ</c>).
	/// </remarks>
	public class ConnectionFactory : Apache.NMS.IConnectionFactory
	{
		public IBM.XMS.IConnectionFactory xmsConnectionFactory = null;

		private Uri brokerUri = null;
		private IRedeliveryPolicy redeliveryPolicy = new RedeliveryPolicy();

		#region Constructors

		/// <summary>
		/// Constructs a <c>ConnectionFactory</c> object using default values.
		/// </summary>
		public ConnectionFactory()
			: this(new Uri("xms:wmq:"))
		{
		}

		/// <summary>
		/// Constructs a <c>ConnectionFactory</c> object.
		/// </summary>
		/// <param name="brokerUri">Factory URI.</param>
		public ConnectionFactory(Uri brokerUri)
		{
			try
			{
				// BrokerUri will construct the xmsConnectionFactory
				this.BrokerUri = brokerUri;
			}
			catch(Exception ex)
			{
				Apache.NMS.Tracer.DebugFormat(
					"Exception instantiating IBM.XMS.ConnectionFactory: {0}",
					ex.Message);
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
			// In case WrapAndThrowNMSException masks the exception
			if(this.xmsConnectionFactory == null)
			{
				throw new Apache.NMS.NMSException(
					"Error instantiating XMS connection factory object.");
			}
		}

		/// <summary>
		/// Constructs the internal <c>ConnectionFactory</c> object from the
		/// parameters specified in the input URI.
		/// </summary>
		/// <param name="factoryUri">Factory URI.</param>
		/// <returns>URI stripped out from overridable property values.
		/// </returns>
		private Uri CreateConnectionFactoryFromURI(Uri factoryUri)
		{
			try
			{
				// Remove "xms:" scheme prefix
				string originalString = factoryUri.OriginalString;
				factoryUri = new Uri(originalString.Substring(
					originalString.IndexOf(":") + 1));

				// Parse URI properties
				StringDictionary properties = URISupport.ParseQuery(
					factoryUri.Query);

				// The URI scheme specifies either the repository of
				// administered objects where the ConnectionFactory object
				// is defined, or the target connection type for
				// non-administered objects.
				switch(factoryUri.Scheme.ToLower())
				{
				case "rtt":
				case "wmq":
				case "wpm":
					CreateNonAdministeredConnectionFactory(
						factoryUri, properties);
					break;
				case "http":
				case "ldap":
				case "cosnaming":
				case "wsvc":
				default:
					CreateAdministeredConnectionFactory(
						factoryUri, properties);
					break;
				}

				// Configure the instanciated connection factory
				ConfigureConnectionFactory(factoryUri, properties);

				return new Uri("xms:" + factoryUri.Scheme);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}

		/// <summary>
		/// Creates a connection factory from the object definition retrieved
		/// from a repository of administered objects.
		/// </summary>
		/// <param name="factoryUri">Factory URI.</param>
		/// <param name="properties">URI properties.</param>
		private void CreateAdministeredConnectionFactory(
			Uri factoryUri, StringDictionary properties)
		{
			// The initial context URL is the non-query part of the factory URI
			string icUrl = factoryUri.OriginalString.Substring(0,
				factoryUri.OriginalString.IndexOf('?'));

			// Extract other InitialContext property values from URI
			string icPrefix = "ic.";
			StringDictionary icProperties = URISupport.ExtractProperties(
				properties, icPrefix);

			// Initialize environment Hashtable
			Hashtable environment = new Hashtable();
			environment[XMSC.IC_URL] = icUrl;
			foreach(DictionaryEntry de in icProperties)
			{
				string key = ((string)de.Key).Substring(icPrefix.Length);
				//TODO: convert "environment." attribute types.
				environment[key] = de.Value;
			}

			// Create an InitialContext
			InitialContext ic = new InitialContext(environment);

			// Extract administered object name
			string objectNameKey = "object.Name";
			if(!properties.ContainsKey(objectNameKey))
			{
				throw new NMSException(string.Format(
					"URI attribute {0} must be specified.", objectNameKey));
			}
            string objectName = properties[objectNameKey];
			properties.Remove(objectNameKey);

			// Lookup for object
			this.xmsConnectionFactory =
				(IBM.XMS.IConnectionFactory)ic.Lookup(objectName);
		}

		/// <summary>
		/// Creates a non-administered connection factory.
		/// </summary>
		/// <param name="factoryUri">Factory URI.</param>
		/// <param name="properties">URI properties.</param>
		private void CreateNonAdministeredConnectionFactory(
			Uri factoryUri, StringDictionary properties)
		{
			// Get an XMS factory factory for the requested protocol
			IBM.XMS.XMSFactoryFactory xmsFactoryFactory = null;
			string scheme = factoryUri.Scheme.ToLower();
			switch(scheme)
			{
			case "rtt":
				xmsFactoryFactory =
					IBM.XMS.XMSFactoryFactory.GetInstance(IBM.XMS.XMSC.CT_RTT);

				if(!string.IsNullOrEmpty(factoryUri.Host))
				{
					this.RTTHostName = factoryUri.Host;
				}

				if(factoryUri.Port != -1)
				{
					this.RTTPort = factoryUri.Port;
				}
				break;

			case "wmq":
				xmsFactoryFactory =
					IBM.XMS.XMSFactoryFactory.GetInstance(IBM.XMS.XMSC.CT_WMQ);
				break;

			case "wpm":
				xmsFactoryFactory =
					IBM.XMS.XMSFactoryFactory.GetInstance(IBM.XMS.XMSC.CT_WPM);
				break;
			}

			// Create the connection factory
			this.xmsConnectionFactory =
				xmsFactoryFactory.CreateConnectionFactory();

			// For RTT and WMQ protocols, set the host name and port if
			// they are specified
			switch(scheme)
			{
				case "rtt":
					if(!string.IsNullOrEmpty(factoryUri.Host))
					{
						this.RTTHostName = factoryUri.Host;
					}

					if(factoryUri.Port != -1)
					{
						this.RTTPort = factoryUri.Port;
					}
					break;

				case "wmq":
					if(!string.IsNullOrEmpty(factoryUri.Host))
					{
						this.WMQHostName = factoryUri.Host;
					}

					if(factoryUri.Port != -1)
					{
						this.WMQPort = factoryUri.Port;
					}
					break;
			}
		}

		/// <summary>
		/// Configures the connection factory.
		/// </summary>
		/// <param name="factoryUri">Factory URI.</param>
		/// <param name="properties">URI properties.</param>
		private void ConfigureConnectionFactory(
			Uri factoryUri, StringDictionary properties)
		{
			if(properties != null && properties.Count > 0)
			{
				IntrospectionSupport.SetProperties(this, properties);
			}
		}

		#endregion

		#region Redelivery Policy

		/// <summary>
		/// Get/or set the redelivery policy that new IConnection objects are
		/// assigned upon creation.
		/// </summary>
		public IRedeliveryPolicy RedeliveryPolicy
		{
			get { return this.redeliveryPolicy; }
			set
			{
				if(value != null)
				{
					this.redeliveryPolicy = value;
				}
			}
		}

		#endregion

		#region Properties (configure via URL parameters)

		// http://www-01.ibm.com/support/knowledgecenter/?lang=en#!/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/props_connf.htm

		#region Connection Factory Properties common to all protocols

		/// <summary>
		/// The client identifier for a connection.
		/// </summary>
		[UriAttribute("factory.ClientId")]
		public string ClientId
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.CLIENT_ID); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.CLIENT_ID, value); }
		}

		/// <summary>
		/// The type of messaging server to which an application connects.
		/// </summary>
		[UriAttribute("factory.XMSConnectionType")]
		public Int32 XMSConnectionType
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.CONNECTION_TYPE); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.CONNECTION_TYPE, value); }
		}

		/// <summary>
		/// The type of messaging server to which an application connects.
		/// </summary>
		[UriAttribute("factory.ConnectionType")]
		public ConnectionType ConnectionType
		{
			get { return XMSConvert.ToConnectionType(this.XMSConnectionType); }
			set { this.XMSConnectionType = XMSConvert.ToXMSConnectionType(value); }
		}

		/// <summary>
		/// A user identifier that can be used to authenticate the application
		/// when it attempts to connect to a messaging server.
		/// </summary>
		[UriAttribute("factory.UserId")]
		public string UserId
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.USERID); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.USERID, value); }
		}

		/// <summary>
		/// A password that can be used to authenticate the application when it
		/// attempts to connect to a messaging server.
		/// </summary>
		[UriAttribute("factory.XMSPassword")]
		public byte[] XMSPassword
		{
			get { return this.xmsConnectionFactory.GetBytesProperty(XMSC.PASSWORD); }
			set { this.xmsConnectionFactory.SetBytesProperty(XMSC.PASSWORD, value); }
		}

		/// <summary>
		/// A password that can be used to authenticate the application when it
		/// attempts to connect to a messaging server, specified as a string.
		/// </summary>
		[UriAttribute("factory.Password")]
		public string Password
		{
			get { return Convert.ToBase64String(this.XMSPassword); }
			set { this.XMSPassword = Convert.FromBase64String(value); }
		}

		/// <summary>
		/// This property determines whether XMS informs an ExceptionListener
		/// only when a connection is broken, or when any exception occurs
		/// asynchronously to an XMS API call.
		/// </summary>
		/// <remarks>
		/// This property applies to all Connections created from this
		/// ConnectionFactory that have an ExceptionListener registered.
		/// </remarks>
		[UriAttribute("factory.XMSAsynchronousExceptions")]
		public Int32 XMSAsynchronousExceptions
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.ASYNC_EXCEPTIONS); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.ASYNC_EXCEPTIONS, value); }
		}

		/// <summary>
		/// This property determines whether XMS informs an
		/// <c>ExceptionListener</c> only when a connection is broken, or when
		/// any exception occurs asynchronously to an XMS API call.
		/// </summary>
		[UriAttribute("factory.AsynchronousExceptions")]
		public AsynchronousExceptions AsynchronousExceptions
		{
			get { return XMSConvert.ToAsynchronousExceptions(this.XMSAsynchronousExceptions); }
			set { this.XMSAsynchronousExceptions = XMSConvert.ToXMSAsynchronousExceptions(value); }
		}

		#endregion

		#region RTT-specific properties

		/// <summary>
		/// The communications protocol used for a real-time connection to a broker.
		/// </summary>
		[UriAttribute("rtt.XMSConnectionProtocol")]
		public Int32 XMSConnectionProtocol
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.RTT_CONNECTION_PROTOCOL); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.RTT_CONNECTION_PROTOCOL, value); }
		}

		/// <summary>
		/// The communications protocol used for a real-time connection to a broker.
		/// </summary>
		[UriAttribute("rtt.ConnectionProtocol")]
		public RTTConnectionProtocol ConnectionProtocol
		{
			get { return XMSConvert.ToRTTConnectionProtocol(this.XMSConnectionProtocol); }
			set { this.XMSConnectionProtocol = XMSConvert.ToXMSRTTConnectionProtocol(value); }
		}

		/// <summary>
		/// The host name or IP address of the system on which a broker runs.
		/// </summary>
		[UriAttribute("rtt.HostName")]
		public string RTTHostName
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.RTT_HOST_NAME); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.RTT_HOST_NAME, value); }
		}

		/// <summary>
		/// The number of the port on which a broker listens for incoming requests.
		/// </summary>
		[UriAttribute("rtt.Port")]
		public Int32 RTTPort
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.RTT_PORT); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.RTT_PORT, value); }
		}

		/// <summary>
		/// The host name or IP address of the local network interface to be
		/// used for a real-time connection to a broker.
		/// </summary>
		[UriAttribute("rtt.LocalAddress")]
		public string RTTLocalAddress
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.RTT_LOCAL_ADDRESS); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.RTT_LOCAL_ADDRESS, value); }
		}

		/// <summary>
		/// The multicast setting for the connection factory.
		/// </summary>
		[UriAttribute("rtt.XMSMulticast")]
		public Int32 XMSMulticast
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.RTT_MULTICAST); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.RTT_MULTICAST, value); }
		}

		/// <summary>
		/// The multicast setting for a connection factory or destination.
		/// </summary>
		[UriAttribute("rtt.Multicast")]
		public Multicast Multicast
		{
			get { return XMSConvert.ToMulticast(this.XMSMulticast); }
			set { this.XMSMulticast = XMSConvert.ToXMSMulticast(value); }
		}

		/// <summary>
		/// The time interval, in milliseconds, after which XMS .NET checks the
		/// connection to a Real Time messaging server to detect any activity.
		/// </summary>
		[UriAttribute("rtt.BrokerPingInterval")]
		public Int32 BrokerPingInterval
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.RTT_BROKER_PING_INTERVAL); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.RTT_BROKER_PING_INTERVAL, value); }
		}

		#endregion

		#region WMQ-specific properties

		/// <summary>
		/// The host name or IP address of the system on which a queue manager
		/// runs.
		/// </summary>
		[UriAttribute("wmq.HostName")]
		public string WMQHostName
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_HOST_NAME); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_HOST_NAME, value); }
		}

		/// <summary>
		/// The number of the port on which a queue manager listens for
		/// incoming requests.
		/// </summary>
		[UriAttribute("wmq.Port")]
		public Int32 WMQPort
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WMQ_PORT); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WMQ_PORT, value); }
		}

		/// <summary>
		/// For a connection to a queue manager, this property specifies the
		/// local network interface to be used, or the local port or range of
		/// local ports to be used, or both.
		/// </summary>
		[UriAttribute("wmq.LocalAddress")]
		public string WMQLocalAddress
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_LOCAL_ADDRESS); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_LOCAL_ADDRESS, value); }
		}

		/// <summary>
		/// The name of the queue manager to connect to.
		/// </summary>
		[UriAttribute("wmq.QueueManagerName")]
		public string QueueManagerName
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_QUEUE_MANAGER); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, value); }
		}

		/// <summary>
		/// The version, release, modification level and fix pack of the queue
		/// manager to which the application intends to connect.
		/// </summary>
		[UriAttribute("wmq.ProviderVersion")]
		public string ProviderVersion
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_PROVIDER_VERSION); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_PROVIDER_VERSION, value); }
		}

		/// <summary>
		/// The name of the channel to be used for a connection.
		/// </summary>
		[UriAttribute("wmq.ChannelName")]
		public string ChannelName
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_CHANNEL); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_CHANNEL, value); }
		}

		/// <summary>
		/// A Uniform Resource Locator (URL) that identifies the name and
		/// location of the file that contains the client channel definition
		/// table and also specifies how the file can be accessed.
		/// </summary>
		[UriAttribute("wmq.ClientChannelDefinitionTableURL")]
		public string ClientChannelDefinitionTableURL
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_CCDTURL); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_CCDTURL, value); }
		}

		/// <summary>
		/// The mode by which an application connects to a queue manager.
		/// </summary>
		[UriAttribute("wmq.XMSConnectionMode")]
		public Int32 XMSConnectionMode
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WMQ_CONNECTION_MODE); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, value); }
		}

		/// <summary>
		/// The mode by which an application connects to a queue manager,
		/// specified as a string.
		/// </summary>
		[UriAttribute("wmq.ConnectionMode")]
		public ConnectionMode ConnectionMode
		{
			get { return XMSConvert.ToConnectionMode(this.XMSConnectionMode); }
			set { this.XMSConnectionMode = XMSConvert.ToXMSConnectionMode(value); }
		}

		/// <summary>
		/// This property specifies the client reconnect options for new
		/// connections created by this factory.
		/// </summary>
		[UriAttribute("wmq.ClientReconnectOptions")]
		public string ClientReconnectOptions
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_CLIENT_RECONNECT_OPTIONS); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_CLIENT_RECONNECT_OPTIONS, value); }
		}

		/// <summary>
		/// This property specifies the duration of time, in seconds, that a
		/// client connection attempts to reconnect.
		/// </summary>
		[UriAttribute("wmq.ClientReconnectTimeout")]
		public string ClientReconnectTimeout
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_CLIENT_RECONNECT_TIMEOUT); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_CLIENT_RECONNECT_TIMEOUT, value); }
		}

		/// <summary>
		/// This property specifies the hosts to which the client attempts to
		/// reconnect to after its connection are broken.
		/// </summary>
		[UriAttribute("wmq.ConnectionNameList")]
		public string ConnectionNameList
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_CONNECTION_NAME_LIST); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_CONNECTION_NAME_LIST, value); }
		}

		/// <summary>
		/// Whether calls to certain methods fail if the queue manager to which
		/// the application is connected is in a quiescing state.
		/// </summary>
		[UriAttribute("wmq.XMSFailIfQuiesce")]
		public Int32 XMSFailIfQuiesce
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WMQ_FAIL_IF_QUIESCE); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WMQ_FAIL_IF_QUIESCE, value); }
		}

		/// <summary>
		/// Whether calls to certain methods fail if the queue manager to which
		/// the application is connected is in a quiescing state.
		/// </summary>
		[UriAttribute("wmq.FailIfQuiesce")]
		public bool FailIfQuiesce
		{
			get { return XMSConvert.ToFailIfQuiesce(this.XMSFailIfQuiesce); }
			set { this.XMSFailIfQuiesce = XMSConvert.ToXMSFailIfQuiesce(value); }
		}

		/// <summary>
		/// The type of broker used by the application for a connection or for
		/// the destination.
		/// </summary>
		[UriAttribute("wmq.XMSBrokerVersion")]
		public Int32 XMSBrokerVersion
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WMQ_BROKER_VERSION); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WMQ_BROKER_VERSION, value); }
		}

		/// <summary>
		/// The type of broker used by the application for a connection or for
		/// the destination.
		/// </summary>
		[UriAttribute("wmq.BrokerVersion")]
		public BrokerVersion BrokerVersion
		{
			get { return XMSConvert.ToBrokerVersion(this.XMSBrokerVersion); }
			set { this.XMSBrokerVersion = XMSConvert.ToXMSBrokerVersion(value); }
		}

		/// <summary>
		/// The name of the queue manager to which a broker is connected.
		/// </summary>
		[UriAttribute("wmq.BrokerQueueManagerName")]
		public string BrokerQueueManagerName
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_BROKER_QMGR); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_BROKER_QMGR, value); }
		}

		/// <summary>
		/// The name of the control queue used by a broker.
		/// </summary>
		[UriAttribute("wmq.BrokerControlQueueName")]
		public string BrokerControlQueueName
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_BROKER_CONTROLQ); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_BROKER_CONTROLQ, value); }
		}

		/// <summary>
		/// The name of the queue monitored by a broker where applications send
		/// messages that they publish.
		/// </summary>
		[UriAttribute("wmq.BrokerPublishQueueName")]
		public string BrokerPublishQueueName
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_BROKER_PUBQ); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_BROKER_PUBQ, value); }
		}

		/// <summary>
		/// The name of the subscriber queue for a nondurable message consumer.
		/// </summary>
		[UriAttribute("wmq.BrokerSubscriberQueueName")]
		public string BrokerSubscriberQueueName
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_BROKER_SUBQ); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_BROKER_SUBQ, value); }
		}

		/// <summary>
		/// Determines whether message selection is done by the XMS client or
		/// by the broker.
		/// </summary>
		[UriAttribute("wmq.XMSMessageSelection")]
		public Int32 XMSMessageSelection
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WMQ_MESSAGE_SELECTION); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WMQ_MESSAGE_SELECTION, value); }
		}

		/// <summary>
		/// Determines whether message selection is done by the XMS client or
		/// by the broker.
		/// </summary>
		[UriAttribute("wmq.MessageSelection")]
		public MessageSelection MessageSelection
		{
			get { return XMSConvert.ToMessageSelection(this.XMSMessageSelection); }
			set { this.XMSMessageSelection = XMSConvert.ToXMSMessageSelection(value); }
		}

		/// <summary>
		/// The maximum number of messages to be retrieved from a queue in one
		/// batch when using asynchronous message delivery.
		/// </summary>
		[UriAttribute("wmq.MessageBatchSize")]
		public Int32 MessageBatchSize
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WMQ_MSG_BATCH_SIZE); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WMQ_MSG_BATCH_SIZE, value); }
		}

		/// <summary>
		/// If each message listener within a session has no suitable message
		/// on its queue, this value is the maximum interval, in milliseconds,
		/// that elapses before each message listener tries again to get a
		/// message from its queue.
		/// </summary>
		[UriAttribute("wmq.PollingInterval")]
		public Int32 PollingInterval
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WMQ_POLLING_INTERVAL); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WMQ_POLLING_INTERVAL, value); }
		}

		/// <summary>
		/// The number of messages published by a publisher before the XMS
		/// client requests an acknowledgement from the broker.
		/// </summary>
		[UriAttribute("wmq.PublishAcknowledgementInterval")]
		public Int32 PublishAcknowledgementInterval
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WMQ_PUB_ACK_INTERVAL); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WMQ_PUB_ACK_INTERVAL, value); }
		}

		/// <summary>
		/// This property determines whether message producers are allowed to
		/// use asynchronous puts to send messages to this destination.
		/// </summary>
		[UriAttribute("wmq.XMSAsynchronousPutsAllowed")]
		public Int32 XMSAsynchronousPutsAllowed
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WMQ_PUT_ASYNC_ALLOWED); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WMQ_PUT_ASYNC_ALLOWED, value); }
		}

		/// <summary>
		/// This property determines whether message producers are allowed to
		/// use asynchronous puts to send messages to this destination.
		/// </summary>
		[UriAttribute("wmq.AsynchronousPutsAllowed")]
		public AsynchronousPutsAllowed AsynchronousPutsAllowed
		{
			get { return XMSConvert.ToAsynchronousPutsAllowed(this.XMSAsynchronousPutsAllowed); }
			set { this.XMSAsynchronousPutsAllowed = XMSConvert.ToXMSAsynchronousPutsAllowed(value); }
		}

		/// <summary>
		/// The identifier (CCSID) of the coded character set, or code page,
		/// in which fields of character data defined in the Message Queue
		/// Interface (MQI) are exchanged between the XMS client and the
		/// WebSphere MQ client.
		/// </summary>
		[UriAttribute("wmq.QueueManagerCCSID")]
		public string QueueManagerCCSID
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_QMGR_CCSID); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_QMGR_CCSID, value); }
		}

		/// <summary>
		/// Identifies a channel receive exit to be run.
		/// </summary>
		[UriAttribute("wmq.ReceiveExit")]
		public string ReceiveExit
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_RECEIVE_EXIT); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_RECEIVE_EXIT, value); }
		}

		/// <summary>
		/// The user data that is passed to a channel receive exit when it
		/// is called.
		/// </summary>
		[UriAttribute("wmq.ReceiveExitInit")]
		public string ReceiveExitInit
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_RECEIVE_EXIT_INIT); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_RECEIVE_EXIT_INIT, value); }
		}

		/// <summary>
		/// Identifies a channel security exit.
		/// </summary>
		[UriAttribute("wmq.SecurityExit")]
		public string SecurityExit
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_SECURITY_EXIT); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_SECURITY_EXIT, value); }
		}

		/// <summary>
		/// The user data that is passed to a channel security exit when it
		/// is called.
		/// </summary>
		[UriAttribute("wmq.SecurityExitInit")]
		public string SecurityExitInit
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_SECURITY_EXIT_INIT); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_SECURITY_EXIT_INIT, value); }
		}

		/// <summary>
		/// Identifies a channel send exit.
		/// </summary>
		[UriAttribute("wmq.SendExit")]
		public string SendExit
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_SEND_EXIT); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_SEND_EXIT, value); }
		}

		/// <summary>
		/// The user data that is passed to channel send exits when they
		/// are called.
		/// </summary>
		[UriAttribute("wmq.SendExitInit")]
		public string SendExitInit
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_SEND_EXIT_INIT); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_SEND_EXIT_INIT, value); }
		}

		/// <summary>
		/// The number of send calls to allow between checking for asynchronous
		/// put errors, within a single non-transacted XMS session.
		/// </summary>
		[UriAttribute("wmq.SendCheckCount")]
		public Int32 SendCheckCount
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WMQ_SEND_CHECK_COUNT); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WMQ_SEND_CHECK_COUNT, value); }
		}

		/// <summary>
		/// Whether a client connection can share its socket with other
		/// top-level XMS connections from the same process to the same queue
		/// manager, if the channel definitions match.
		/// </summary>
		[UriAttribute("wmq.XMSShareSocketAllowed")]
		public Int32 XMSShareSocketAllowed
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WMQ_SHARE_CONV_ALLOWED); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WMQ_SHARE_CONV_ALLOWED, value); }
		}

		/// <summary>
		/// Whether a client connection can share its socket with other
		/// top-level XMS connections from the same process to the same queue
		/// manager, if the channel definitions match.
		/// </summary>
		[UriAttribute("wmq.ShareSocketAllowed")]
		public bool ShareSocketAllowed
		{
			get { return XMSConvert.ToShareSocketAllowed(this.XMSShareSocketAllowed); }
			set { this.XMSShareSocketAllowed = XMSConvert.ToXMSShareSocketAllowed(value); }
		}

		/// <summary>
		/// The locations of the servers that hold the certificate revocation
		/// lists (CRLs) to be used on an SSL connection to a queue manager.
		/// </summary>
		[UriAttribute("wmq.SSLCertificateStores")]
		public string SSLCertificateStores
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_SSL_CERT_STORES); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_SSL_CERT_STORES, value); }
		}

		/// <summary>
		/// The name of the CipherSpec to be used on a secure connection to
		/// a queue manager.
		/// </summary>
		[UriAttribute("wmq.SSLCipherSpec")]
		public string SSLCipherSpec
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_SSL_CIPHER_SPEC); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_SSL_CIPHER_SPEC, value); }
		}

		/// <summary>
		/// The name of the CipherSuite to be used on an SSL or TLS connection
		/// to a queue manager. The protocol used in negotiating the secure
		/// connection depends on the specified CipherSuite.
		/// </summary>
		[UriAttribute("wmq.SSLCipherSuite")]
		public string SSLCipherSuite
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_SSL_CIPHER_SUITE); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_SSL_CIPHER_SUITE, value); }
		}

		/// <summary>
		/// Configuration details for the cryptographic hardware connected
		/// to the client system.
		/// </summary>
		[UriAttribute("wmq.SSLCryptoHardware")]
		public string SSLCryptoHardware
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_SSL_CRYPTO_HW); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_SSL_CRYPTO_HW, value); }
		}

		/// <summary>
		/// The value of this property determines whether an application can
		/// or cannot use non-FIPS compliant cipher suites. If this property
		/// is set to true, only FIPS algorithms are used for the client-server
		/// connection.
		/// </summary>
		[UriAttribute("wmq.SSLFipsRequired")]
		public bool SSLFipsRequired
		{
			get { return this.xmsConnectionFactory.GetBooleanProperty(XMSC.WMQ_SSL_FIPS_REQUIRED); }
			set { this.xmsConnectionFactory.SetBooleanProperty(XMSC.WMQ_SSL_FIPS_REQUIRED, value); }
		}

		/// <summary>
		/// The location of the key database file in which keys and
		/// certificates are stored.
		/// </summary>
		[UriAttribute("wmq.SSLKeyRepository")]
		public string SSLKeyRepository
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_SSL_KEY_REPOSITORY); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_SSL_KEY_REPOSITORY, value); }
		}

		/// <summary>
		/// The KeyResetCount represents the total number of unencrypted bytes
		/// sent and received within an SSL conversation before the secret key
		/// is renegotiated.
		/// </summary>
		[UriAttribute("wmq.SSLKeyResetCount")]
		public Int32 SSLKeyResetCount
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WMQ_SSL_KEY_RESETCOUNT); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WMQ_SSL_KEY_RESETCOUNT, value); }
		}

		/// <summary>
		/// The peer name to be used on an SSL connection to a queue manager.
		/// </summary>
		[UriAttribute("wmq.SSLPeerName")]
		public string SSLPeerName
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_SSL_PEER_NAME); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_SSL_PEER_NAME, value); }
		}

		/// <summary>
		/// Whether all messages must be retrieved from queues within sync point control.
		/// </summary>
		[UriAttribute("wmq.SyncpointAllGets")]
		public bool SyncpointAllGets
		{
			get { return this.xmsConnectionFactory.GetBooleanProperty(XMSC.WMQ_SYNCPOINT_ALL_GETS); }
			set { this.xmsConnectionFactory.SetBooleanProperty(XMSC.WMQ_SYNCPOINT_ALL_GETS, value); }
		}

		/// <summary>
		/// Whether messages sent to the destination contain an MQRFH2 header.
		/// </summary>
		[UriAttribute("wmq.XMSTargetClient")]
		public Int32 XMSTargetClient
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WMQ_TARGET_CLIENT); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WMQ_TARGET_CLIENT, value); }
		}

		/// <summary>
		/// Whether messages sent to the destination contain an <c>MQRFH2</c>
		/// header.
		/// </summary>
		[UriAttribute("wmq.TargetClient")]
		public TargetClient TargetClient
		{
			get { return XMSConvert.ToTargetClient(this.XMSTargetClient); }
			set { this.XMSTargetClient = XMSConvert.ToXMSTargetClient(value); }
		}

		/// <summary>
		/// The prefix used to form the name of the WebSphere MQ dynamic queue
		/// that is created when the application creates an XMS temporary queue.
		/// </summary>
		[UriAttribute("wmq.TemporaryQueuePrefix")]
		public string WMQTemporaryQueuePrefix
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_TEMP_Q_PREFIX); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_TEMP_Q_PREFIX, value); }
		}

		/// <summary>
		/// When creating temporary topics, XMS generates a topic string of
		/// the form "TEMP/TEMPTOPICPREFIX/unique_id", or if this property
		/// contains the default value, then this string, "TEMP/unique_id", is
		/// generated. Specifying a non-empty value allows specific model
		/// queues to be defined for creating the managed queues for subscribers
		/// to temporary topics created under this connection.
		/// </summary>
		[UriAttribute("wmq.TemporaryTopicPrefix")]
		public string WMQTemporaryTopicPrefix
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_TEMP_TOPIC_PREFIX); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_TEMP_TOPIC_PREFIX, value); }
		}

		/// <summary>
		/// The name of the WebSphere MQ model queue from which a dynamic
		/// queue is created when the application creates an XMS temporary
		/// queue.
		/// </summary>
		[UriAttribute("wmq.TemporaryModel")]
		public string WMQTemporaryModel
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_TEMP_TOPIC_PREFIX); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_TEMP_TOPIC_PREFIX, value); }
		}

		#endregion

		#region WPM-specific properties

		/// <summary>
		/// For a connection to a service integration bus, this property
		/// specifies the local network interface to be used, or the local
		/// port or range of local ports to be used, or both.
		/// </summary>
		[UriAttribute("wpm.LocalAddress")]
		public string WPMLocalAddress
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WPM_LOCAL_ADDRESS); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WPM_LOCAL_ADDRESS, value); }
		}

		/// <summary>
		/// The name of the service integration bus that the application
		/// connects to.
		/// </summary>
		[UriAttribute("wpm.BusName")]
		public string BusName
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WPM_BUS_NAME); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WPM_BUS_NAME, value); }
		}

		/// <summary>
		/// The connection proximity setting for the connection.
		/// </summary>
		[UriAttribute("wpm.XMSConnectionProximity")]
		public Int32 XMSConnectionProximity
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WPM_CONNECTION_PROXIMITY); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WPM_CONNECTION_PROXIMITY, value); }
		}

		/// <summary>
		/// The connection proximity setting for the connection.
		/// </summary>
		[UriAttribute("wpm.ConnectionProximity")]
		public ConnectionProximity ConnectionProximity
		{
			get { return XMSConvert.ToConnectionProximity(this.XMSConnectionProximity); }
			set { this.XMSConnectionProximity = XMSConvert.ToXMSConnectionProximity(value); }
		}

		/// <summary>
		/// The name of the messaging engine where all durable subscriptions
		/// for a connection or a destination are managed.
		/// </summary>
		[UriAttribute("wpm.DurableSubscriptionHome")]
		public string DurableSubscriptionHome
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WPM_DUR_SUB_HOME); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WPM_DUR_SUB_HOME, value); }
		}

		/// <summary>
		/// The reliability level of nonpersistent messages that are sent
		/// using the connection.
		/// </summary>
		[UriAttribute("wpm.XMSNonPersistentMap")]
		public Int32 XMSNonPersistentMap
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WPM_NON_PERSISTENT_MAP); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WPM_NON_PERSISTENT_MAP, value); }
		}

		/// <summary>
		/// The reliability level of nonpersistent messages that are sent
		/// using the connection.
		/// </summary>
		[UriAttribute("wpm.NonPersistentMap")]
		public Mapping NonPersistentMap
		{
			get { return XMSConvert.ToMapping(this.XMSNonPersistentMap); }
			set { this.XMSNonPersistentMap = XMSConvert.ToXMSMapping(value); }
		}

		/// <summary>
		/// The reliability level of persistent messages that are sent
		/// using the connection.
		/// </summary>
		[UriAttribute("wpm.XMSPersistentMap")]
		public Int32 XMSPersistentMap
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WPM_PERSISTENT_MAP); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WPM_PERSISTENT_MAP, value); }
		}

		/// <summary>
		/// The reliability level of persistent messages that are sent
		/// using the connection.
		/// </summary>
		[UriAttribute("wpm.PersistentMap")]
		public Mapping PersistentMap
		{
			get { return XMSConvert.ToMapping(this.XMSPersistentMap); }
			set { this.XMSPersistentMap = XMSConvert.ToXMSMapping(value); }
		}

		/// <summary>
		/// A sequence of one or more endpoint addresses of bootstrap servers.
		/// </summary>
		[UriAttribute("wpm.ProviderEndpoints")]
		public string ProviderEndpoints
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WPM_PROVIDER_ENDPOINTS); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WPM_PROVIDER_ENDPOINTS, value); }
		}

		/// <summary>
		/// The name of a target group of messaging engines.
		/// </summary>
		[UriAttribute("wpm.TargetGroup")]
		public string TargetGroup
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WPM_TARGET_GROUP); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WPM_TARGET_GROUP, value); }
		}

		/// <summary>
		/// The significance of the target group of messaging engines.
		/// </summary>
		[UriAttribute("wpm.XMSTargetSignificance")]
		public Int32 XMSTargetSignificance
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WPM_TARGET_SIGNIFICANCE); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WPM_TARGET_SIGNIFICANCE, value); }
		}

		/// <summary>
		/// The significance of the target group of messaging engines.
		/// </summary>
		[UriAttribute("wpm.TargetSignificance")]
		public TargetSignificance TargetSignificance
		{
			get { return XMSConvert.ToTargetSignificance(this.XMSTargetSignificance); }
			set { this.XMSTargetSignificance = XMSConvert.ToXMSTargetSignificance(value); }
		}

		/// <summary>
		/// The name of the inbound transport chain that the application must
		/// use to connect to a messaging engine.
		/// </summary>
		[UriAttribute("wpm.TargetTransportChain")]
		public string TargetTransportChain
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WPM_TARGET_TRANSPORT_CHAIN); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WPM_TARGET_TRANSPORT_CHAIN, value); }
		}

		/// <summary>
		/// The type of the target group of messaging engines.
		/// </summary>
		[UriAttribute("wpm.XMSTargetType")]
		public Int32 XMSTargetType
		{
			get { return this.xmsConnectionFactory.GetIntProperty(XMSC.WPM_TARGET_TYPE); }
			set { this.xmsConnectionFactory.SetIntProperty(XMSC.WPM_TARGET_TYPE, value); }
		}

		/// <summary>
		/// The type of the target group of messaging engines.
		/// </summary>
		[UriAttribute("wpm.TargetType")]
		public TargetType TargetType
		{
			get { return XMSConvert.ToTargetType(this.XMSTargetType); }
			set { this.XMSTargetType = XMSConvert.ToXMSTargetType(value); }
		}

		/// <summary>
		/// The prefix used to form the name of the temporary queue that is
		/// created in the service integration bus when the application creates
		/// an XMS temporary queue.
		/// </summary>
		[UriAttribute("wpm.TemporaryQueuePrefix")]
		public string WPMTemporaryQueuePrefix
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_TEMP_Q_PREFIX); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_TEMP_Q_PREFIX, value); }
		}

		/// <summary>
		/// The prefix used to form the name of a temporary topic that is
		/// created by the application.
		/// </summary>
		[UriAttribute("wpm.TemporaryTopicPrefix")]
		public string WPMTemporaryTopicPrefix
		{
			get { return this.xmsConnectionFactory.GetStringProperty(XMSC.WMQ_TEMP_TOPIC_PREFIX); }
			set { this.xmsConnectionFactory.SetStringProperty(XMSC.WMQ_TEMP_TOPIC_PREFIX, value); }
		}

		#endregion

		#region Common properties having protocol-specific keys

		/// <summary>
		/// The host name or IP address of the system on which a broker (RTT)
		/// or queue manager (WMQ) runs.
		/// </summary>
		public string HostName
		{
			get
			{
				switch(this.XMSConnectionType)
				{
					case XMSC.CT_RTT: return this.RTTHostName;
					case XMSC.CT_WMQ: return this.WMQHostName;
					case XMSC.CT_WPM: return null;
					default         : return null;
				}
			}
			set
			{
				switch(this.XMSConnectionType)
				{
					case XMSC.CT_RTT: this.RTTHostName = value; break;
					case XMSC.CT_WMQ: this.WMQHostName = value; break;
					case XMSC.CT_WPM: break;
					default         : break;
				}
			}
		}

		/// <summary>
		/// The number of the port on which a broker (RTT) or queue manager
		/// (WMQ) listens for incoming requests.
		/// </summary>
		public Int32 Port
		{
			get
			{
				switch(this.XMSConnectionType)
				{
					case IBM.XMS.XMSC.CT_RTT: return this.RTTPort;
					case IBM.XMS.XMSC.CT_WMQ: return this.WMQPort;
					case IBM.XMS.XMSC.CT_WPM: return 0;
					default         : return 0;
				}
			}
			set
			{
				switch(this.XMSConnectionType)
				{
					case IBM.XMS.XMSC.CT_RTT: this.RTTPort = value; break;
					case IBM.XMS.XMSC.CT_WMQ: this.WMQPort = value; break;
					case IBM.XMS.XMSC.CT_WPM: break;
					default         : break;
				}
			}
		}

		/// <summary>
		/// The host name or IP address of the local network interface to be
		/// used for a RTT real-time connection to a broker.
		/// For a WMQ connection to a queue manager, this property specifies
		/// the local network interface to be used, or the local port or range
		/// of local ports to be used, or both.
		/// For a WPM connection to a service integration bus, this property
		/// specifies the local network interface to be used, or the local
		/// port or range of local ports to be used, or both.
		/// </summary>
		public string LocalAddress
		{
			get
			{
				switch(this.XMSConnectionType)
				{
					case IBM.XMS.XMSC.CT_RTT: return this.RTTLocalAddress;
					case IBM.XMS.XMSC.CT_WMQ: return this.WMQLocalAddress;
					case IBM.XMS.XMSC.CT_WPM: return this.WPMLocalAddress;
					default         : return null;
				}
			}
			set
			{
				switch(this.XMSConnectionType)
				{
					case IBM.XMS.XMSC.CT_RTT: this.RTTLocalAddress = value; break;
					case IBM.XMS.XMSC.CT_WMQ: this.WMQLocalAddress = value; break;
					case IBM.XMS.XMSC.CT_WPM: this.WPMLocalAddress = value; break;
					default         : break;
				}
			}
		}

		/// <summary>
		/// The prefix used to form the name of the WebSphere MQ dynamic queue
		/// that is created (WMQ), or the name of the temporary queue that is
		/// created in the service integration bus (WPM) when the application
		/// creates an XMS temporary queue.
		/// </summary>
		public string TemporaryQueuePrefix
		{
			get
			{
				switch(this.XMSConnectionType)
				{
					case IBM.XMS.XMSC.CT_RTT: return null;
					case IBM.XMS.XMSC.CT_WMQ: return this.WMQTemporaryQueuePrefix;
					case IBM.XMS.XMSC.CT_WPM: return this.WPMTemporaryQueuePrefix;
					default         : return null;
				}
			}
			set
			{
				switch(this.XMSConnectionType)
				{
					case IBM.XMS.XMSC.CT_RTT: break;
					case IBM.XMS.XMSC.CT_WMQ: this.WMQTemporaryQueuePrefix = value; break;
					case IBM.XMS.XMSC.CT_WPM: this.WPMTemporaryQueuePrefix = value; break;
					default         : break;
				}
			}
		}

		/// <summary>
		/// The prefix used to form the name of a temporary topic that is
		/// created by the application (WMQ and WPM).
		/// </summary>
		public string TemporaryTopicPrefix
		{
			get
			{
				switch(this.XMSConnectionType)
				{
					case IBM.XMS.XMSC.CT_RTT: return null;
					case IBM.XMS.XMSC.CT_WMQ: return this.WMQTemporaryTopicPrefix;
					case IBM.XMS.XMSC.CT_WPM: return this.WPMTemporaryTopicPrefix;
					default         : return null;
				}
			}
			set
			{
				switch(this.XMSConnectionType)
				{
					case IBM.XMS.XMSC.CT_RTT: break;
					case IBM.XMS.XMSC.CT_WMQ: this.WMQTemporaryTopicPrefix = value; break;
					case IBM.XMS.XMSC.CT_WPM: this.WPMTemporaryTopicPrefix = value; break;
					default         : break;
				}
			}
		}

		#endregion

		#endregion

		#region IConnectionFactory Members

		/// <summary>
		/// Creates a new connection to IBM MQ with the default properties.
		/// </summary>
		public Apache.NMS.IConnection CreateConnection()
		{
			Apache.NMS.IConnection connection = null;
			try
			{
				connection = new Apache.NMS.XMS.Connection(
					this.xmsConnectionFactory.CreateConnection());
				ConfigureConnection(connection);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
			return connection;
		}

		/// <summary>
		/// Creates a new connection to IBM MQ using a specified user identity.
		/// </summary>
		/// <param name="userName">User name.</param>
		/// <param name="password">Password.</param>
		public Apache.NMS.IConnection CreateConnection(
					string userName, string password)
		{
			Apache.NMS.IConnection connection = null;
			try
			{
				connection = new Apache.NMS.XMS.Connection(
					this.xmsConnectionFactory.CreateConnection(
						userName, password));
				ConfigureConnection(connection);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
			return connection;
		}

		/// <summary>
		/// Configure the newly created connection.
		/// </summary>
		/// <param name="connection">Connection.</param>
		private void ConfigureConnection(IConnection connection)
		{
			connection.RedeliveryPolicy = this.redeliveryPolicy.Clone() as IRedeliveryPolicy;
			connection.ConsumerTransformer = this.consumerTransformer;
			connection.ProducerTransformer = this.producerTransformer;
		}

		/// <summary>
		/// Get or set the broker URI.
		/// </summary>
		public Uri BrokerUri
		{
			get { return this.brokerUri; }
			set { this.brokerUri = CreateConnectionFactoryFromURI(value); }
		}

		private ConsumerTransformerDelegate consumerTransformer;
		/// <summary>
		/// A Delegate that is called each time a Message is dispatched to allow the client to do
		/// any necessary transformations on the received message before it is delivered.  The
		/// ConnectionFactory sets the provided delegate instance on each Connection instance that
		/// is created from this factory, each connection in turn passes the delegate along to each
		/// Session it creates which then passes that along to the Consumers it creates.
		/// </summary>
		public ConsumerTransformerDelegate ConsumerTransformer
		{
			get { return this.consumerTransformer; }
			set { this.consumerTransformer = value; }
		}

		private ProducerTransformerDelegate producerTransformer;
		/// <summary>
		/// A delegate that is called each time a Message is sent from this Producer which allows
		/// the application to perform any needed transformations on the Message before it is sent.
		/// The ConnectionFactory sets the provided delegate instance on each Connection instance that
		/// is created from this factory, each connection in turn passes the delegate along to each
		/// Session it creates which then passes that along to the Producers it creates.
		/// </summary>
		public ProducerTransformerDelegate ProducerTransformer
		{
			get { return this.producerTransformer; }
			set { this.producerTransformer = value; }
		}

		#endregion
	}
}
