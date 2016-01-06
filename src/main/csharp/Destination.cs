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
	public class Destination : IDestination
	{
		public IBM.XMS.IDestination xmsDestination;

		#region Constructors

		/// <summary>
		/// Constructs a destination object.
		/// </summary>
		/// <param name="destination">IBM XMS destination.</param>
		public Destination(IBM.XMS.IDestination destination)
		{
			this.xmsDestination = destination;
		}

		/// <summary>
		/// Constructs a destination object specifying if the destination is
		/// temporary.
		/// </summary>
		/// <param name="destination">IBM XMS destination.</param>
		/// <param name="isTemporary">Whether the destination is temporary.
		/// </param>
		public Destination(IBM.XMS.IDestination destination, bool isTemporary)
		{
			this.xmsDestination = destination;
			this.isTemporary = isTemporary;
		}

		#endregion

		#region IDestination implementation

		/// <summary>
		/// Destination type.
		/// </summary>
		public DestinationType DestinationType
		{
			get
			{
				return XMSConvert.ToDestinationType(
					this.xmsDestination.TypeId,
					this.isTemporary);
			}
		}

		/// <summary>
		/// Checks if destination is a topic.
		/// </summary>
		public bool IsTopic
		{
			get
			{
				return (this.xmsDestination.TypeId
					== IBM.XMS.DestinationType.Topic);
			}
		}

		/// <summary>
		/// Checks if destination is a queue.
		/// </summary>
		public bool IsQueue
		{
			get
			{
				return (this.xmsDestination.TypeId
					== IBM.XMS.DestinationType.Queue);
			}
		}

		private readonly bool isTemporary;
		/// <summary>
		/// Checks if destination is temporary.
		/// </summary>
		public bool IsTemporary
		{
			get { return this.isTemporary; }
		}

		#endregion

		#region XMS IDestination properties

		// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/props_dest.htm?lang=en

		#region Common properties

		/// <summary>
		/// Destination name.
		/// </summary>
		public string Name
		{
			get { return this.xmsDestination.Name; }
		}

		/// <summary>
		/// The delivery mode of messages sent to the destination.
		/// </summary>
		public Int32 XMSDeliveryMode
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.DELIVERY_MODE); }
			set { this.xmsDestination.SetIntProperty(XMSC.DELIVERY_MODE, value); }
		}

		/// <summary>
		/// The delivery mode of messages sent to the destination.
		/// </summary>
		public Apache.NMS.XMS.Util.DeliveryMode DeliveryMode
		{
			get { return XMSConvert.ToDeliveryMode(this.XMSDeliveryMode); }
			set { this.XMSDeliveryMode = XMSConvert.ToXMSDeliveryMode(value); }
		}

		/// <summary>
		/// The priority of messages sent to the destination.
		/// </summary>
		public Int32 Priority
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.PRIORITY); }
			set { this.xmsDestination.SetIntProperty(XMSC.PRIORITY, value); }
		}

		/// <summary>
		/// The time to live in milliseconds for messages sent to the
		/// destination.
		/// </summary>
		public Int32 TimeToLive
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.TIME_TO_LIVE); }
			set { this.xmsDestination.SetIntProperty(XMSC.TIME_TO_LIVE, value); }
		}

		#endregion

		#region RTT-specific properties

		/// <summary>
		/// The multicast setting for the destination.
		/// </summary>
		[UriAttribute("rtt.XMSMulticast")]
		public Int32 XMSMulticast
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.RTT_MULTICAST); }
			set { this.xmsDestination.SetIntProperty(XMSC.RTT_MULTICAST, value); }
		}

		/// <summary>
		/// The multicast setting for the destination.
		/// </summary>
		[UriAttribute("rtt.Multicast")]
		public Multicast Multicast
		{
			get { return XMSConvert.ToMulticast(this.XMSMulticast); }
			set { this.XMSMulticast = XMSConvert.ToXMSMulticast(value); }
		}

		#endregion

		#region WMQ-specific properties

		/// <summary>
		/// The type of broker used by the application for the destination.
		/// </summary>
		[UriAttribute("wmq.XMSBrokerVersion")]
		public Int32 XMSBrokerVersion
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.WMQ_BROKER_VERSION); }
			set { this.xmsDestination.SetIntProperty(XMSC.WMQ_BROKER_VERSION, value); }
		}

		/// <summary>
		/// The type of broker used by the application for the destination.
		/// </summary>
		[UriAttribute("wmq.BrokerVersion")]
		public BrokerVersion BrokerVersion
		{
			get { return XMSConvert.ToBrokerVersion(this.XMSBrokerVersion); }
			set { this.XMSBrokerVersion = XMSConvert.ToXMSBrokerVersion(value); }
		}

		/// <summary>
		/// The identifier (CCSID) of the coded character set, or code page,
		/// that the strings of character data in the body of a message are in
		/// when the XMS client forwards the message to the destination.
		/// </summary>
		[UriAttribute("wmq.DestinationCCSID")]
		public Int32 DestinationCCSID
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.WMQ_CCSID); }
			set { this.xmsDestination.SetIntProperty(XMSC.WMQ_CCSID, value); }
		}

		/// <summary>
		/// The name of the subscriber queue for a durable subscriber that is
		/// receiving messages from the destination.
		/// </summary>
		[UriAttribute("wmq.SubscriberQueueName")]
		public string SubscriberQueueName
		{
			get { return this.xmsDestination.GetStringProperty(XMSC.WMQ_DUR_SUBQ); }
			set { this.xmsDestination.SetStringProperty(XMSC.WMQ_DUR_SUBQ, value); }
		}

		/// <summary>
		/// How numerical data in the body of a message is represented when
		/// the XMS client forwards the message to the destination.
		/// </summary>
		[UriAttribute("wmq.XMSEncoding")]
		public Int32 XMSEncoding
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.WMQ_ENCODING); }
			set { this.xmsDestination.SetIntProperty(XMSC.WMQ_ENCODING, value); }
		}

		/// <summary>
		/// How numerical data in the body of a message is represented when
		/// the XMS client forwards the message to the destination.
		/// </summary>
		[UriAttribute("wmq.Encoding")]
		public Encoding Encoding
		{
			get { return XMSConvert.ToEncoding(this.XMSEncoding); }
			set { this.XMSEncoding = XMSConvert.ToXMSEncoding(value); }
		}

		/// <summary>
		/// Whether calls to certain methods fail if the queue manager to which
		/// the application is connected is in a quiescing state.
		/// </summary>
		[UriAttribute("wmq.XMSFailIfQuiesce")]
		public Int32 XMSFailIfQuiesce
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.WMQ_FAIL_IF_QUIESCE); }
			set { this.xmsDestination.SetIntProperty(XMSC.WMQ_FAIL_IF_QUIESCE, value); }
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
		/// This property determines whether an XMS application processes the
		/// <c>MQRFH2</c> of a WebSphere MQ message as part of the message
		/// payload (that is, as part of the message body).
		/// </summary>
		[UriAttribute("wmq.XMSMessageBody")]
		public Int32 XMSMessageBody
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.WMQ_MESSAGE_BODY); }
			set { this.xmsDestination.SetIntProperty(XMSC.WMQ_MESSAGE_BODY, value); }
		}

		/// <summary>
		/// This property determines whether an XMS application processes the
		/// <c>MQRFH2</c> of a WebSphere MQ message as part of the message
		/// payload (that is, as part of the message body).
		/// </summary>
		[UriAttribute("wmq.MessageBody")]
		public MessageBody MessageBody
		{
			get { return XMSConvert.ToMessageBody(this.XMSMessageBody); }
			set { this.XMSMessageBody = XMSConvert.ToXMSMessageBody(value); }
		}

		/// <summary>
		/// Determines what level of message context is to be set by the XMS
		/// application. The application must be running with appropriate
		/// context authority for this property to take effect.
		/// </summary>
		[UriAttribute("wmq.XMSMessageContext")]
		public Int32 XMSMessageContext
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.WMQ_MQMD_MESSAGE_CONTEXT); }
			set { this.xmsDestination.SetIntProperty(XMSC.WMQ_MQMD_MESSAGE_CONTEXT, value); }
		}

		/// <summary>
		/// Determines what level of message context is to be set by the XMS
		/// application. The application must be running with appropriate
		/// context authority for this property to take effect.
		/// </summary>
		[UriAttribute("wmq.MessageContext")]
		public MessageContext MessageContext
		{
			get { return XMSConvert.ToMessageContext(this.XMSMessageContext); }
			set { this.XMSMessageContext = XMSConvert.ToXMSMessageContext(value); }
		}

		/// <summary>
		/// This property determines whether an XMS application can extract
		/// the values of MQMD fields or not.
		/// </summary>
		[UriAttribute("wmq.XMSMQMDReadEnabled")]
		public Int32 XMSMQMDReadEnabled
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.WMQ_MQMD_READ_ENABLED); }
			set { this.xmsDestination.SetIntProperty(XMSC.WMQ_MQMD_READ_ENABLED, value); }
		}

		/// <summary>
		/// This property determines whether an XMS application can extract
		/// the values of MQMD fields or not.
		/// </summary>
		[UriAttribute("wmq.MQMDReadEnabled")]
		public bool MQMDReadEnabled
		{
			get { return XMSConvert.ToMQMDReadEnabled(this.XMSMQMDReadEnabled); }
			set { this.XMSMQMDReadEnabled = XMSConvert.ToXMSMQMDReadEnabled(value); }
		}

		/// <summary>
		/// This property determines whether an XMS application can set
		/// the values of MQMD fields or not.
		/// </summary>
		[UriAttribute("wmq.XMSMQMDWriteEnabled")]
		public Int32 XMSMQMDWriteEnabled
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.WMQ_MQMD_WRITE_ENABLED); }
			set { this.xmsDestination.SetIntProperty(XMSC.WMQ_MQMD_WRITE_ENABLED, value); }
		}

		/// <summary>
		/// This property determines whether an XMS application can set
		/// the values of MQMD fields or not.
		/// </summary>
		[UriAttribute("wmq.MQMDWriteEnabled")]
		public bool MQMDWriteEnabled
		{
			get { return XMSConvert.ToMQMDWriteEnabled(this.XMSMQMDWriteEnabled); }
			set { this.XMSMQMDWriteEnabled = XMSConvert.ToXMSMQMDWriteEnabled(value); }
		}

		/// <summary>
		/// This property determines whether message consumers and queue
		/// browsers are allowed to use read ahead to get non-persistent,
		/// non-transactional messages from this destination into an internal
		/// buffer before receiving them.
		/// </summary>
		[UriAttribute("wmq.XMSReadAheadAllowed")]
		public Int32 XMSReadAheadAllowed
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.WMQ_READ_AHEAD_ALLOWED); }
			set { this.xmsDestination.SetIntProperty(XMSC.WMQ_READ_AHEAD_ALLOWED, value); }
		}

		/// <summary>
		/// This property determines whether message consumers and queue
		/// browsers are allowed to use read ahead to get non-persistent,
		/// non-transactional messages from this destination into an internal
		/// buffer before receiving them.
		/// </summary>
		[UriAttribute("wmq.ReadAheadAllowed")]
		public ReadAheadAllowed ReadAheadAllowed
		{
			get { return XMSConvert.ToReadAheadAllowed(this.XMSReadAheadAllowed); }
			set { this.XMSReadAheadAllowed = XMSConvert.ToXMSReadAheadAllowed(value); }
		}


		/// <summary>
		/// This property determines, for messages being delivered to an
		/// asynchronous message listener, what happens to messages in the
		/// internal read ahead buffer when the message consumer is closed.
		/// </summary>
		[UriAttribute("wmq.XMSReadAheadClosePolicy")]
		public Int32 XMSReadAheadClosePolicy
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.WMQ_READ_AHEAD_CLOSE_POLICY); }
			set { this.xmsDestination.SetIntProperty(XMSC.WMQ_READ_AHEAD_CLOSE_POLICY, value); }
		}

		/// <summary>
		/// This property determines, for messages being delivered to an
		/// asynchronous message listener, what happens to messages in the
		/// internal read ahead buffer when the message consumer is closed.
		/// </summary>
		[UriAttribute("wmq.ReadAheadClosePolicy")]
		public ReadAheadClosePolicy ReadAheadClosePolicy
		{
			get { return XMSConvert.ToReadAheadClosePolicy(this.XMSReadAheadClosePolicy); }
			set { this.XMSReadAheadClosePolicy = XMSConvert.ToXMSReadAheadClosePolicy(value); }
		}

		/// <summary>
		/// Destination property that sets the target CCSID for queue manager
		/// message conversion. The value is ignored unless
		/// <c>XMSC.WMQ_RECEIVE_CONVERSION</c> is set to
		/// <c>WMQ_RECEIVE_CONVERSION_QMGR</c>.
		/// </summary>
		[UriAttribute("wmq.ReceiveCCSID")]
		public Int32 ReceiveCCSID
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.WMQ_RECEIVE_CCSID); }
			set { this.xmsDestination.SetIntProperty(XMSC.WMQ_RECEIVE_CCSID, value); }
		}

		/// <summary>
		/// Destination property that determines whether data conversion is
		/// going to be performed by the queue manager.
		/// </summary>
		[UriAttribute("wmq.XMSReceiveConversion")]
		public Int32 XMSReceiveConversion
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.WMQ_RECEIVE_CONVERSION); }
			set { this.xmsDestination.SetIntProperty(XMSC.WMQ_RECEIVE_CONVERSION, value); }
		}

		/// <summary>
		/// Destination property that determines whether data conversion is
		/// going to be performed by the queue manager.
		/// </summary>
		[UriAttribute("wmq.ReceiveConversion")]
		public ReceiveConversion ReceiveConversion
		{
			get { return XMSConvert.ToReceiveConversion(this.XMSReceiveConversion); }
			set { this.XMSReceiveConversion = XMSConvert.ToXMSReceiveConversion(value); }
		}

		/// <summary>
		/// Whether messages sent to the destination contain an <c>MQRFH2</c>
		/// header.
		/// </summary>
		[UriAttribute("wmq.XMSTargetClient")]
		public Int32 XMSTargetClient
		{
			get { return this.xmsDestination.GetIntProperty(XMSC.WMQ_TARGET_CLIENT); }
			set { this.xmsDestination.SetIntProperty(XMSC.WMQ_TARGET_CLIENT, value); }
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
		/// When creating temporary topics, XMS generates a topic string of
		/// the form "TEMP/TEMPTOPICPREFIX/unique_id", or if this property
		/// contains the default value, then this string, "TEMP/unique_id",
		/// is generated. Specifying a non-empty value allows specific model
		/// queues to be defined for creating the managed queues for subscribers
		/// to temporary topics created under this connection.
		/// </summary>
		[UriAttribute("wmq.TemporaryTopicPrefix")]
		public string WMQTemporaryTopicPrefix
		{
			get { return this.xmsDestination.GetStringProperty(XMSC.WMQ_TEMP_TOPIC_PREFIX); }
			set { this.xmsDestination.SetStringProperty(XMSC.WMQ_TEMP_TOPIC_PREFIX, value); }
		}

		#endregion

		#region WPM-specific properties

		/// <summary>
		/// The name of the service integration bus in which the destination
		/// exists.
		/// </summary>
		[UriAttribute("wpm.BusName")]
		public string BusName
		{
			get { return this.xmsDestination.GetStringProperty(XMSC.WPM_BUS_NAME); }
			set { this.xmsDestination.SetStringProperty(XMSC.WPM_BUS_NAME, value); }
		}


		/// <summary>
		/// The name of the topic space that contains the topic.
		/// </summary>
		[UriAttribute("wpm.TopicSpace")]
		public string TopicSpace
		{
			get { return this.xmsDestination.GetStringProperty(XMSC.WPM_TOPIC_SPACE); }
			set { this.xmsDestination.SetStringProperty(XMSC.WPM_TOPIC_SPACE, value); }
		}

		#endregion

		#endregion

		#region IDisposable implementation

		public void Dispose()
		{
		}

		#endregion
	}
}
