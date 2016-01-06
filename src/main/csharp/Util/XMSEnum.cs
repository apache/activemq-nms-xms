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
using IBM.XMS;

namespace Apache.NMS.XMS.Util
{

#region Encoding (IBM JMS)*

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prp_jms_ibm_encoding.htm?lang=en
/// <summary>
/// How numerical data in the body of the message is represented when the XMS
/// client forwards the message to its intended destination.
/// </summary>
public enum Encoding
{
	/// <summary>
	/// Normal integer encoding.
	/// </summary>
	IntegerNormal = MQC.MQENC_INTEGER_NORMAL,

	/// <summary>
	/// Reversed integer encoding.
	/// </summary>
	IntegerReversed = MQC.MQENC_INTEGER_REVERSED,

	/// <summary>
	/// Normal packed decimal encoding.
	/// </summary>
	DecimalNormal = MQC.MQENC_DECIMAL_NORMAL,

	/// <summary>
	/// Reversed packed decimal encoding.
	/// </summary>
	DecimalReversed = MQC.MQENC_DECIMAL_REVERSED,

	/// <summary>
	/// Normal IEEE floating point encoding.
	/// </summary>
	FloatIEEENormal = MQC.MQENC_FLOAT_IEEE_NORMAL,

	/// <summary>
	/// Reversed IEEE floating point encoding.
	/// </summary>
	FloatIEEEReversed = MQC.MQENC_FLOAT_IEEE_REVERSED,

	/// <summary>
	/// z/OS® architecture floating point encoding.
	/// </summary>
	FloatS390 = MQC.MQENC_FLOAT_S390,

	/// <summary>
	/// Native machine encoding.
	/// </summary>
	Native = MQC.MQENC_NATIVE
}

#endregion

#region Message Type (IBM JMS)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prp_jms_ibm_msgtype.htm?lang=en
/// <summary>
/// Message type.
/// </summary>
public enum MessageType
{
	/// <summary>
	/// The message is one that does not require a reply.
	/// </summary>
	Datagram = MQC.MQMT_DATAGRAM,

	/// <summary>
	/// The message is one that requires a reply.
	/// </summary>
	Request = MQC.MQMT_REQUEST,

	/// <summary>
	/// The message is a reply message.
	/// </summary>
	Reply = MQC.MQMT_REPLY,

	/// <summary>
	/// The message is a report message.
	/// </summary>
	Report = MQC.MQMT_REPORT
}

#endregion

#region Report Confirm On Arrival (IBM JMS)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prp_jms_ibm_rep_coa.htm?lang=en
/// <summary>
/// Request 'confirm on arrival' report messages, specifying how much
/// application data from the original message must be included in a
/// report message.
/// </summary>
public enum ReportConfirmOnArrival
{
	/// <summary>
	/// Request 'confirm on arrival' report messages, with no application
	/// data from the original message included in a report message.
	/// </summary>
	NoData = MQC.MQRO_COA,

	/// <summary>
	/// Request 'confirm on arrival' report messages, with the first 100 bytes
	/// of application data from the original message included in a report
	/// message.
	/// </summary>
	PartialData = MQC.MQRO_COA_WITH_DATA,

	/// <summary>
	/// Request 'confirm on arrival' report messages, with all the application
	/// data from the original message included in a report message.
	/// </summary>
	FullData = MQC.MQRO_COA_WITH_FULL_DATA
}

#endregion

#region Report Confirm On Delivery (IBM JMS)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prp_jms_ibm_rep_cod.htm?lang=en
/// <summary>
/// Request 'confirm on delivery' report messages, specifying how much
/// application data from the original message must be included in a
/// report message.
/// </summary>
public enum ReportConfirmOnDelivery
{
	/// <summary>
	/// Request 'confirm on delivery' report messages, with no application
	/// data from the original message included in a report message.
	/// </summary>
	NoData = MQC.MQRO_COD,

	/// <summary>
	/// Request 'confirm on delivery' report messages, with the first 100 bytes
	/// of application data from the original message included in a report
	/// message.
	/// </summary>
	PartialData = MQC.MQRO_COD_WITH_DATA,

	/// <summary>
	/// Request 'confirm on delivery' report messages, with all the application
	/// data from the original message included in a report message.
	/// </summary>
	FullData = MQC.MQRO_COD_WITH_FULL_DATA
}

#endregion

#region Report Exception (IBM JMS)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prp_jms_ibm_rep_excpt.htm?lang=en
/// <summary>
/// Request exception report messages, specifying how much application data
/// from the original message must be included in a report message.
/// </summary>
public enum ReportExceptions
{
	/// <summary>
	/// Request exception report messages, with no application data from the
	/// original message included in a report message.
	/// </summary>
	NoData = MQC.MQRO_COD,

	/// <summary>
	/// Request exception report messages, with the first 100 bytes of
	/// application data from the original message included in a report
	/// message.
	/// </summary>
	PartialData = MQC.MQRO_COD_WITH_DATA,

	/// <summary>
	/// Request exception report messages, with all the application data from
	/// the original message included in a report message.
	/// </summary>
	FullData = MQC.MQRO_COD_WITH_FULL_DATA
}

#endregion

#region Report Expiration (IBM JMS)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prp_jms_ibm_rep_exprn.htm?lang=en
/// <summary>
/// Request expiration report messages, specifying how much application data
/// from the original message must be included in a report message.
/// </summary>
public enum ReportExpiration
{
	/// <summary>
	/// Request expiration report messages, with no application data from the
	/// original message included in a report message.
	/// </summary>
	NoData = MQC.MQRO_EXPIRATION,

	/// <summary>
	/// Request expiration report messages, with the first 100 bytes of
	/// application data from the original message included in a report
	/// message.
	/// </summary>
	PartialData = MQC.MQRO_EXPIRATION_WITH_DATA,

	/// <summary>
	/// Request expiration report messages, with all the application data from
	/// the original message included in a report message.
	/// </summary>
	FullData = MQC.MQRO_EXPIRATION_WITH_FULL_DATA
}

#endregion

#region Report Correlation Id (IBM JMS)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prp_jms_ibm_rep_pcid.htm?lang=en
/// <summary>
/// Request that the correlation identifier of any report or reply message
/// is the same as the correlation identifier of the original message.
/// </summary>
public enum ReportCorrelationId
{
	/// <summary>
	/// Request that the correlation identifier of any report or reply message
	/// is the same as the correlation identifier of the original message.
	/// </summary>
	OriginalCorrelationId = MQC.MQRO_PASS_CORREL_ID,

	/// <summary>
	/// Request that the correlation identifier of any report or reply message
	/// is the same as the message identifier of the original message.
	/// </summary>
	OriginalMessageId = MQC.MQRO_COPY_MSG_ID_TO_CORREL_ID
}

#endregion

#region Report Message Id (IBM JMS)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prp_jms_ibm_rep_pmid.htm?lang=en
/// <summary>
/// Request that the message identifier of any report or reply message is the
/// same as the message identifier of the original message.
/// </summary>
public enum ReportMessageId
{
	/// <summary>
	/// Request that the message identifier of any report or reply message is the same as the message identifier of the original message.
	/// </summary>
	OriginalMessageId = MQC.MQRO_PASS_MSG_ID,

	/// <summary>
	/// Request that a new message identifier is generated for each report or
	/// reply message.
	/// </summary>
	NewMessageId = MQC.MQRO_NEW_MSG_ID
}

#endregion

#region Asynchronous Exceptions

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/xmsc_async_exceptions.htm?lang=en
/// <summary>
/// whether XMS informs an ExceptionListener only when a connection is broken,
/// or when any exception occurs asynchronously to an XMS API call.
/// </summary>
public enum AsynchronousExceptions
{
	/// <summary>
	/// Any exception detected asynchronously, outside the scope of a
	/// synchronous API call, and all connection broken exceptions are sent
	/// to the <c>ExceptionListener</c>.
	/// </summary>
	All = XMSC.ASYNC_EXCEPTIONS_ALL,

	/// <summary>
	/// Only exceptions indicating a broken connection are sent to the
	/// <c>ExceptionListener</c>. Any other exceptions occurring during
	/// asynchronous processing are not reported to the
	/// <c>ExceptionListener</c>, and hence the application is not informed
	/// of these exceptions.
	/// </summary>
	ConnectionBroken = XMSC.ASYNC_EXCEPTIONS_CONNECTIONBROKEN
}

#endregion

#region Connection Type

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_connection_type.htm?lang=en
/// <summary>
/// The type of messaging server to which an application connects.
/// </summary>
public enum ConnectionType
{
	/// <summary>
	/// A real-time connection to a broker.
	/// </summary>
	RTT = XMSC.CT_RTT,

	/// <summary>
	/// A connection to a WebSphere® MQ queue manager.
	/// </summary>
	WMQ = XMSC.CT_WMQ,

	/// <summary>
	/// A connection to a WebSphere service integration bus.
	/// </summary>
	WPM = XMSC.CT_WPM
}

#endregion

#region Delivery Mode

/// <summary>
/// The delivery mode of messages sent to the destination.
/// </summary>
public enum DeliveryMode
{
	/// <summary>
	/// A message sent to the destination is nonpersistent. The default
	/// delivery mode of the message producer, or any delivery mode specified
	/// on the Send call, is ignored. If the destination is a WebSphere MQ
	/// queue, the value of the queue attribute <c>DefPersistence</c> is also
	/// ignored.
	/// </summary>
	NotPersistent = XMSC.DELIVERY_NOT_PERSISTENT,

	/// <summary>
	/// A message sent to the destination is persistent. The default
	/// delivery mode of the message producer, or any delivery mode specified
	/// on the Send call, is ignored. If the destination is a WebSphere MQ
	/// queue, the value of the queue attribute <c>DefPersistence</c> is also
	/// ignored.
	/// </summary>
	Persistent = XMSC.DELIVERY_PERSISTENT,

	/// <summary>
	/// A message sent to the destination has the delivery mode specified on
	/// the Send call. If the Send call specifies no delivery mode, the default
	/// delivery mode of the message producer is used instead. If the
	/// destination is a WebSphere MQ queue, the value of the queue attribute
	/// <c>DefPersistence</c> is ignored.
	/// </summary>
	AsApplication = XMSC.DELIVERY_AS_APP,

	/// <summary>
	/// If the destination is a WebSphere MQ queue, a message put on the queue
	/// has the delivery mode specified by the value of the queue attribute
	/// <c>DefPersistence</c>. The default delivery mode of the message
	/// producer, or any delivery mode specified on the Send call, is ignored.
	/// </summary>
	AsDestination = XMSC.DELIVERY_AS_DEST
}

#endregion

#region Priority

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_priority.htm?lang=en
/// <summary>
/// The priority of messages sent to the destination.
/// </summary>
public enum Priority
{
	/// <summary>
	/// Lowest message priority.
	/// </summary>
	Lowest = 0,

	/// <summary>
	/// Between Low and Lowest message priority.
	/// </summary>
	VeryLow = 1,

	/// <summary>
	/// Low message priority.
	/// </summary>
	Low = 2,

	/// <summary>
	/// Normal message priority.
	/// </summary>
	Normal = 5,

	/// <summary>
	/// Between High and Normal message priority.
	/// </summary>
	AboveNormal = 6,

	/// <summary>
	/// High message priority.
	/// </summary>
	High = 7,

	/// <summary>
	/// Between Highest and High message priority.
	/// </summary>
	VeryHigh = 8,

	/// <summary>
	/// Highest message priority.
	/// </summary>
	Highest = 9,

	/// <summary>
	/// A message sent to the destination has the priority specified on the
	/// Send call. If the Send call specifies no priority, the default
	/// priority of the message producer is used instead. If the destination
	/// is a WebSphere MQ queue, the value of the queue attribute
	/// <c>DefPriority</c> is ignored.
	/// </summary>
	AsApplication = XMSC.PRIORITY_AS_APP,

	/// <summary>
	/// If the destination is a WebSphere MQ queue, a message put on the
	/// queue has the priority specified by the value of the queue attribute
	/// <c>DefPriority</c>. The default priority of the message producer,
	/// or any priority specified on the Send call, is ignored.
	/// </summary>
	AsDestination = XMSC.PRIORITY_AS_DEST
}

#endregion

#region Connection Protocol (RTT)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_rtt_conn_prot.htm?lang=en
/// <summary>
/// The communications protocol used for a real-time connection to a broker.
/// </summary>
public enum RTTConnectionProtocol
{
	/// <summary>
	/// Real-time connection to a broker over TCP/IP.
	/// <c>ConnectionFactory</c> object.
	/// </summary>
	TCP = XMSC.RTT_CP_TCP
}

#endregion

#region Multicast (RTT)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_rtt_multicast.htm?lang=en
/// <summary>
/// The multicast setting for a connection factory or destination. Only a
/// destination that is a topic can have this property.
/// An application uses this property to enable multicast in association with
/// a real-time connection to a broker and, if multicast is enabled, to
/// specify the precise way in which multicast is used to deliver messages
/// from the broker to a message consumer. The property has no effect on how
/// a message producer sends messages to the broker.
/// </summary>
public enum Multicast
{
	/// <summary>
	/// Messages are not delivered to a message consumer using WebSphere® MQ
	/// Multicast Transport. This value is the default value for a
	/// <c>ConnectionFactory</c> object.
	/// </summary>
	Disabled = XMSC.RTT_MULTICAST_DISABLED,

	/// <summary>
	/// Messages are delivered to a message consumer according to the multicast
	/// setting for the connection factory associated with the message consumer.
	/// The multicast setting for the connection factory is noted at the time
	/// that the connection is created. This value is valid only for a
	/// <c>Destination</c> object, and is the default value for a
	/// <c>Destination</c> object.
	/// </summary>
	AsConnectionFactory = XMSC.RTT_MULTICAST_ASCF,

	/// <summary>
	/// If the topic is configured for multicast in the broker, messages are
	/// delivered to a message consumer using WebSphere MQ Multicast Transport.
	/// A reliable quality of service is used if the topic is configured for
	/// reliable multicast.
	/// </summary>
	Enabled = XMSC.RTT_MULTICAST_ENABLED,

	/// <summary>
	/// If the topic is configured for reliable multicast in the broker,
	/// messages are delivered to a message consumer using WebSphere MQ
	/// Multicast Transport with a reliable quality of service. If the topic
	/// is not configured for reliable multicast, you cannot create a message
	/// consumer for the topic.
	/// </summary>
	Reliable = XMSC.RTT_MULTICAST_RELIABLE,

	/// <summary>
	/// If the topic is configured for multicast in the broker, messages are
	/// delivered to a message consumer using WebSphere MQ Multicast Transport.
	/// A reliable quality of service is not used even if the topic is
	/// configured for reliable multicast.
	/// </summary>
	NotReliable = XMSC.RTT_MULTICAST_NOT_RELIABLE
}

#endregion

#region Broker Version (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_wmq_brkr_version.htm?lang=en
/// <summary>
/// The type of broker used by the application for a connection or for the
/// destination. Only a destination that is a topic can have this property.
/// </summary>
public enum BrokerVersion
{
	/// <summary>
	/// The application is using a WebSphere® MQ Publish/Subscribe broker.
	/// </summary>
	Version1 = XMSC.WMQ_BROKER_V1,

	/// <summary>
	/// The application is using a broker of IBM® Integration Bus.
	/// </summary>
	Version2 = XMSC.WMQ_BROKER_V2,

	/// <summary>
	/// After the broker is migrated, set this property so that RFH2 headers
	/// are no longer used. After migration, this property is no longer
	/// relevant.
	/// </summary>
	Unspecified = XMSC.WMQ_BROKER_UNSPECIFIED
}

#endregion

#region Reconnect Options (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_rtt_client_reconnect_options.htm?lang=en
/// <summary>
/// Client reconnect options for new connections created by a factory.
/// </summary>
public enum ReconnectOptions
{
	/// <summary>
	/// Use the value specified in the <c>mqclient.ini</c> file. Set the value
	/// by using the DefRecon property within the Channels stanza.
	/// </summary>
	AsDefault = XMSC.WMQ_CLIENT_RECONNECT_AS_DEF,

	/// <summary>
	/// Reconnect to any of the queue managers specified in the connection name
	/// list.
	/// </summary>
	AnyQueueManager = XMSC.WMQ_CLIENT_RECONNECT,

	/// <summary>
	/// Reconnects to the same queue manager that it is originally connected to.
	/// It returns <c>MQRC.RECONNECT_QMID_MISMATCH</c> if the queue manager it
	/// tries to connect to (specified in the connection name list) has a
	/// different QMID to the queue manager originally connected to.
	/// </summary>
	SameQueueManager = XMSC.WMQ_CLIENT_RECONNECT_Q_MGR,

	/// <summary>
	/// Reconnection is disabled.
	/// </summary>
	Disabled = XMSC.WMQ_CLIENT_RECONNECT_DISABLED
}

#endregion

#region Connection Mode (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_wmq_conn_mode.htm?lang=en
/// <summary>
/// The mode by which an application connects to a queue manager.
/// </summary>
public enum ConnectionMode
{
	/// <summary>
	/// A connection to a queue manager in bindings mode, for optimal performance. This value is the default value for C/C++.
	/// </summary>
	Bindings = XMSC.WMQ_CM_BINDINGS,

	/// <summary>
	/// A connection to a queue manager in client mode, to ensure a fully managed stack. This value is the default value for .NET.
	/// </summary>
	Client = XMSC.WMQ_CM_CLIENT,

	/// <summary>
	/// A connection to a queue manager which forces an unmanaged client stack.
	/// </summary>
	ClientUnmanaged = XMSC.WMQ_CM_CLIENT_UNMANAGED
}

#endregion

#region Encoding (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_wmq_encoding.htm?lang=en
// cf. "Encoding (IBM JMS)"

#endregion

#region Fail If Quiesce (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_wmq_fail_if_quiesce.htm?lang=en
/// <summary>
/// Whether calls to certain methods fail if the queue manager to which the
/// application is connected is in a quiescing state.
/// </summary>
public enum FailIfQuiesce
{
	/// <summary>
	/// Calls to certain methods fail if the queue manager is in a quiescing
	/// state. When the application detects that the queue manager is
	/// quiescing, the application can complete its immediate task and close
	/// the connection, allowing the queue manager to stop.
	/// </summary>
	Yes = XMSC.WMQ_FIQ_YES,

	/// <summary>
	/// No method calls fail because the queue manager is in a quiescing
	/// state. If you specify this value, the application cannot detect that
	/// the queue manager is quiescing. The application might continue to
	/// perform operations against the queue manager and therefore prevent
	/// the queue manager from stopping.
	/// </summary>
	No = XMSC.WMQ_FIQ_NO
}

#endregion

#region Message Body (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/xmsc_wmq_message_body.htm?lang=en
/// <summary>
/// Whether an XMS application processes the <c>MQRFH2</c> of a WebSphere®
/// MQ message as part of the message payload (that is, as part of the
/// message body).
/// </summary>
public enum MessageBody
{
	/// <summary>
	/// Receive: The inbound XMS message type and body are determined by the
	/// contents of the <c>MQRFH2</c> (if present) or the <c>MQMD</c> (if there
	/// is no <c>MQRFH2</c>) in the received WebSphere MQ message.
	/// Send: The outbound XMS message body contains a prepended and
	/// auto-generated <c>MQRFH2</c> header based on XMS Message properties and
	/// header fields.
	/// </summary>
	JMS = XMSC.WMQ_MESSAGE_BODY_JMS,

	/// <summary>
	/// Receive: The inbound XMS message type is always <c>ByteMessage</c>,
	/// irrespective of the contents of received WebSphere MQ message or the
	/// format field of the received <c>MQMD</c>. The XMS message body is the
	/// unaltered message data returned by the underlying messaging provider
	/// API call. The character set and encoding of the data in the message
	/// body is determined by the <c>CodedCharSetId</c> and <c>Encoding</c>
	/// fields of the <c>MQMD</c>. The format of the data in the message body
	/// is determined by the <c>Format</c> field of the <c>MQMD</c>.
	/// Send: The outbound XMS message body contains the application payload
	/// as-is; and no auto-generated WebSphere MQ header is added to the body.
	/// </summary>
	MQ = XMSC.WMQ_MESSAGE_BODY_MQ,

	/// <summary>
	/// Receive: The XMS client determines a suitable value for this property.
	/// On receive path, this value is the <c>WMQ_MESSAGE_BODY_JMS</c> property
	/// value.
	/// Send: The XMS client determines a suitable value for this property. On
	/// send path, this value is the <c>XMSC_WMQ_TARGET_CLIENT</c> property
	/// value.
	/// </summary>
	Unspecified = XMSC.WMQ_MESSAGE_BODY_UNSPECIFIED
}

#endregion

#region Message Context (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/xmsc_wmq_mqmd_message_context.htm?lang=en
/// <summary>
/// Determines what level of message context is to be set by the XMS
/// application. The application must be running with appropriate context
/// authority for this property to take effect.
/// </summary>
public enum MessageContext
{
	/// <summary>
	/// For outbound messages, the <c>MQOPEN</c> API call and the <c>MQPMO</c>
	/// structure specifies no explicit message context options.
	/// </summary>
	Default = XMSC.WMQ_MDCTX_DEFAULT,

	/// <summary>
	/// The <c>MQOPEN</c> API call specifies the message context option 
	/// <c>MQOO_SET_IDENTITY_CONTEXT</c> and the <c>MQPMO</c> structure
	/// specifies <c>MQPMO_SET_IDENTITY_CONTEXT</c>.
	/// </summary>
	SetIdentity = XMSC.WMQ_MDCTX_SET_IDENTITY_CONTEXT,

	/// <summary>
	/// The <c>MQOPEN</c> API call specifies the message context option
	/// <c>MQOO_SET_ALL_CONTEXT</c> and the <c>MQPMO</c> structure
	/// specifies <c>MQPMO_SET_ALL_CONTEXT</c>. 
	/// </summary>
	SetAll = XMSC.WMQ_MDCTX_SET_ALL_CONTEXT,
}

#endregion

#region MQMD Read Enabled (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/xmsc_wmq_mqmd_read_enabled.htm?lang=en
/// <summary>
/// Whether an XMS application can extract the values of MQMD fields or not.
/// </summary>
public enum MQMDReadEnabled
{
	/// <summary>
	/// When sending messages, the <c>JMS_IBM_MQMD*</c> properties on a sent
	/// message are not updated to reflect the updated field values in the
	/// <c>MQMD</c>.
	/// When receiving messages, none of the <c>JMS_IBM_MQMD*</c> properties
	/// are available on a received message, even if some or all of them are
	/// set by the sender.
	/// </summary>
	No = 0, //XMSC.WMQ_READ_ENABLED_NO,

	/// <summary>
	/// When sending messages, all of the <c>JMS_IBM_MQMD*</c> properties on
	/// a sent message are updated to reflect the updated field values in the
	/// <c>MQMD</c>, including those properties that the sender did not set
	/// explicitly.
	/// When receiving messages, all of the <c>JMS_IBM_MQMD*</c> properties
	/// are available on a received message, including those properties that
	/// the sender did not set explicitly. 
	/// </summary>
	Yes = 1 //XMSC.WMQ_READ_ENABLED_YES
}

#endregion

#region MQMD Write Enabled (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/xmsc_wmq_mqmd_write_enabled.htm?lang=en
/// <summary>
/// Whether an XMS application can write the values of MQMD fields or not.
/// </summary>
public enum MQMDWriteEnabled
{
	/// <summary>
	/// All <c>JMS_IBM_MQMD*</c> properties are ignored and their values are
	/// not copied into the underlying <c>MQMD</c> structure.
	/// </summary>
	No = 0, //XMSC.WMQ_WRITE_ENABLED_NO,

	/// <summary>
	/// <c>JMS_IBM_MQMD*</c> properties are processed. Their values are copied
	/// into the underlying <c>MQMD</c> structure.
	/// </summary>
	Yes = 1 //XMSC.WMQ_WRITE_ENABLED_YES
}

#endregion

#region Asynchronous Puts Allowed (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/xmsc_wmq_put_async_allowed.htm?lang=en
/// <summary>
/// Whether message producers are allowed to use asynchronous puts to send
/// messages to this destination.
/// </summary>
public enum AsynchronousPutsAllowed
{
	/// <summary>
	/// Determine whether asynchronous puts are allowed by referring to the
	/// queue or topic definition.
	/// </summary>
	AsDestination = XMSC.WMQ_PUT_ASYNC_ALLOWED_AS_DEST,

	/// <summary>
	/// Determine whether asynchronous puts are allowed by referring to the
	/// queue definition.
	/// </summary>
	AsQueueDefinition = XMSC.WMQ_PUT_ASYNC_ALLOWED_AS_Q_DEF,

	/// <summary>
	/// Determine whether asynchronous puts are allowed by referring to the
	/// topic definition.
	/// </summary>
	AsTopicDefinition = XMSC.WMQ_PUT_ASYNC_ALLOWED_AS_TOPIC_DEF,

	/// <summary>
	/// Asynchronous puts are not allowed.
	/// </summary>
	Disabled = XMSC.WMQ_PUT_ASYNC_ALLOWED_DISABLED,

	/// <summary>
	/// Asynchronous puts are allowed.
	/// </summary>
	Enabled = XMSC.WMQ_PUT_ASYNC_ALLOWED_ENABLED
}

#endregion

#region Read Ahead Allowed (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/xmsc_wmq_read_ahead_allowed.htm?lang=en
/// <summary>
/// Whether message consumers and queue browsers are allowed to use read ahead
/// to get non-persistent, non-transactional messages from this destination
/// into an internal buffer before receiving them.
/// </summary>
public enum ReadAheadAllowed
{
	/// <summary>
	/// Determine whether read ahead is allowed by referring to the queue
	/// definition.
	/// </summary>
	AsQueueDefinition = XMSC.WMQ_READ_AHEAD_ALLOWED_AS_Q_DEF,

	/// <summary>
	/// Determine whether read ahead is allowed by referring to the topic
	/// definition.
	/// </summary>
	AsTopicDefinition = XMSC.WMQ_READ_AHEAD_ALLOWED_AS_TOPIC_DEF,

	/// <summary>
	/// Determine whether read ahead is allowed by referring to the queue
	/// or topic definition.
	/// </summary>
	AsDestinationDefinition = XMSC.WMQ_READ_AHEAD_ALLOWED_AS_DEST,

	/// <summary>
	/// Read ahead is not allowed while consuming or browsing messages.
	/// </summary>
	Disabled = XMSC.WMQ_READ_AHEAD_ALLOWED_DISABLED,

	/// <summary>
	/// Read ahead is allowed.
	/// </summary>
	Enabled = XMSC.WMQ_READ_AHEAD_ALLOWED_ENABLED
}

#endregion

#region Read Ahead Close Policy (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/xmsc_wmq_read_ahead_close_policy.htm?lang=en
/// <summary>
/// This property determines, for messages being delivered to an asynchronous
/// message listener, what happens to messages in the internal read ahead buffer
/// when the message consumer is closed.
/// </summary>
public enum ReadAheadClosePolicy
{
	/// <summary>
	/// Only the current message listener invocation completes before returning,
	/// potentially leaving messages in the internal read ahead buffer, which
	/// are then discarded.
	/// </summary>
	DeliverCurrent = XMSC.WMQ_READ_AHEAD_DELIVERCURRENT,

	/// <summary>
	/// All messages in the internal read ahead buffer are delivered to the
	/// application message listener before returning. 
	/// </summary>
	DeliverAll = XMSC.WMQ_READ_AHEAD_DELIVERALL
}

#endregion

#region Message Selection (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_wmq_mes_selection.htm?lang=en
/// <summary>
/// Determines whether message selection is done by the XMS client or by
/// the broker.
/// </summary>
public enum MessageSelection
{
	/// <summary>
	/// Message selection is done by the XMS client.
	/// </summary>
	Client = XMSC.WMQ_MSEL_CLIENT,

	/// <summary>
	/// Message selection is done by the broker.
	/// </summary>
	Broker = XMSC.WMQ_MSEL_BROKER
}

#endregion

#region Receive Conversion (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/xmsc_wmq_receive_conversion.htm?lang=en
/// <summary>
/// Whether data conversion is going to be performed by the queue manager.
/// </summary>
public enum ReceiveConversion
{
	/// <summary>
	/// Perform data conversion on the XMS client only. Conversion is always
	/// done using codepage 1208.
	/// </summary>
	Client = XMSC.WMQ_RECEIVE_CONVERSION_CLIENT_MSG,

	/// <summary>
	/// Perform data conversion on the queue manager before sending a message
	/// to the XMS client. 
	/// </summary>
	QueueManager = XMSC.WMQ_RECEIVE_CONVERSION_QMGR
}

#endregion

#region Share Socket Allowed (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/xmsc_wmq_share_conv_allowed.htm?lang=en
/// <summary>
/// Whether a client connection can share its socket with other top-level XMS
/// connections from the same process to the same queue manager, if the channel
/// definitions match.
/// </summary>
public enum ShareSocketAllowed
{
	/// <summary>
	/// Connections do not share a socket.
	/// </summary>
	False = XMSC.WMQ_SHARE_CONV_ALLOWED_NO,

	/// <summary>
	/// Connections share a socket.
	/// </summary>
	True = XMSC.WMQ_SHARE_CONV_ALLOWED_YES
}

#endregion

#region Target Client (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_wmq_target_client.htm?lang=en
/// <summary>
/// Whether messages sent to the destination contain an <c>MQRFH2</c> header.
/// </summary>
public enum TargetClient
{
	/// <summary>
	/// Messages sent to the destination contain an <c>MQRFH2</c> header.
	/// Specify this value if the application is sending the messages to
	/// another XMS application, a WebSphere® JMS application, or a native
	/// WebSphere MQ application that is designed to handle an <c>MQRFH2</c>
	/// header.
	/// </summary>
	JMS = XMSC.WMQ_TARGET_DEST_JMS,

	/// <summary>
	/// Messages sent to the destination do not contain an <c>MQRFH2</c>
	/// header. Specify this value if the application is sending the messages
	/// to a native WebSphere MQ application that is not designed to handle
	/// an <c>MQRFH2</c> header.
	/// </summary>
	MQ = XMSC.WMQ_TARGET_DEST_MQ
}

#endregion

#region Wildcard Format (WMQ)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/xmsc_wmq_wildcard_format.htm?lang=en
/// <summary>
/// This property determines which version of wildcard syntax is to be used.
/// </summary>
public enum WildcardFormat
{
	/// <summary>
	/// Recognizes the topic level wildcards only i.e. '#' and '+' are treated
	/// as wildcards.
	/// </summary>
	TopicOnly = XMSC.WMQ_WILDCARD_TOPIC_ONLY,

	/// <summary>
	/// Recognizes the character wildcards only i.e. '*' and '?' are treated
	/// as wildcards.
	/// </summary>
	CharacterOnly = XMSC.WMQ_WILDCARD_CHAR_ONLY
}

#endregion

#region Connection Protocol (WPM)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_wpm_conn_prot.htm?lang=en
/// <summary>
/// The communications protocol used for the connection to the messaging
/// engine.
/// </summary>
public enum WPMConnectionProtocol
{
	/// <summary>
	/// The connection uses HTTP over TCP/IP.
	/// </summary>
	HTTP = XMSC.WPM_CP_HTTP,

	/// <summary>
	/// The connection uses TCP/IP.
	/// </summary>
	TCP = XMSC.WPM_CP_TCP
}

#endregion

#region Connection Proximity (WPM)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_wpm_conn_prox.htm?lang=en
/// <summary>
/// The connection proximity setting for the connection. This property
/// determines how close the messaging engine that the application connects
/// to must be to the bootstrap server.
/// </summary>
public enum ConnectionProximity
{
	/// <summary>
	/// Bus.
	/// </summary>
	Bus,           // XMSC.WPM_CONNECTION_PROXIMITY_BUS     = "Bus"

	/// <summary>
	/// Cluster.
	/// </summary>
	Cluster,       // XMSC.WPM_CONNECTION_PROXIMITY_CLUSTER = "Cluster"

	/// <summary>
	/// Host.
	/// </summary>
	Host,          // XMSC.WPM_CONNECTION_PROXIMITY_HOST    = "Host"

	/// <summary>
	/// Server.
	/// </summary>
	Server         // XMSC.WPM_CONNECTION_PROXIMITY_SERVER  = "Server"
}

#endregion

#region Mapping (WPM)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_wpm_non_pers_m.htm?lang=en
// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_wpm_pers_m.htm?lang=en
/// <summary>
/// The reliability level of messages that are sent using the connection.
/// </summary>
public enum Mapping
{
	/// <summary>
	/// Determined by the default reliability level specified for the queue
	/// or topic space in the service integration bus.
	/// </summary>
	AsDestination = XMSC.WPM_MAPPING_AS_DESTINATION,

	/// <summary>
	/// Best effort nonpersistent.
	/// </summary>
	BestEffortNonPersistent = XMSC.WPM_MAPPING_BEST_EFFORT_NON_PERSISTENT,

	/// <summary>
	/// Express nonpersistent.
	/// </summary>
	ExpressNonPersistent = XMSC.WPM_MAPPING_EXPRESS_NON_PERSISTENT,

	/// <summary>
	/// Reliable nonpersistent.
	/// </summary>
	ReliableNonPersistent = XMSC.WPM_MAPPING_RELIABLE_NON_PERSISTENT,

	/// <summary>
	/// Reliable persistent.
	/// </summary>
	ReliablePersistent = XMSC.WPM_MAPPING_RELIABLE_PERSISTENT,

	/// <summary>
	/// Assured persistent.
	/// </summary>
	AssuredPersistent = XMSC.WPM_MAPPING_ASSURED_PERSISTENT
}

#endregion

#region Target Significance (WPM)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_wpm_target_signf.htm?lang=en
/// <summary>
/// The significance of the target group of messaging engines.
/// </summary>
public enum TargetSignificance
{
	/// <summary>
	/// A messaging engine in the target group is selected if one is available.
	/// Otherwise, a messaging engine outside the target group is selected,
	/// provided it is in the same service integration bus.
	/// </summary>
	Preferred, // XMSC.WPM_TARGET_SIGNIFICANCE_PREFERRED = "Preferred"

	/// <summary>
	/// The selected messaging engine must be in the target group. If a
	/// messaging engine in the target group is not available, the connection
	/// process fails.
	/// </summary>
	Required   // XMSC.WPM_TARGET_SIGNIFICANCE_REQUIRED  = "Required"
}

#endregion

#region Target Type (WPM)

// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/prx_wpm_target_type.htm?lang=en
/// <summary>
/// The type of the target group of messaging engines. This property
/// determines the nature of the target group identified by the
/// <c>XMSC_WPM_TARGET_GROUP</c> property.
/// </summary>
public enum TargetType
{
	/// <summary>
	/// The name of the target group is the name of a bus member. The target
	/// group is all the messaging engines in the bus member.
	/// </summary>
	BusMember,      // XMSC.WPM_TARGET_TYPE_BUSMEMBER = "BusMember"

	/// <summary>
	/// The name of the target group is the name of a user-defined group of
	/// messaging engines. The target group is all the messaging engines that
	/// are registered with the user-defined group.
	/// </summary>
	Custom,         // XMSC.WPM_TARGET_TYPE_CUSTOM = "Custom"

	/// <summary>
	/// The name of the target group is the name of a messaging engine. The
	/// target group is the specified messaging engine.
	/// </summary>
	MessagingEngine // XMSC.WPM_TARGET_TYPE_ME = "ME"
}

#endregion

}