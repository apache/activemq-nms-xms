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
	/// Represents an NMS connection to IBM MQ.
	/// </summary>
	///
	public class Connection : Apache.NMS.IConnection
	{
		private Apache.NMS.AcknowledgementMode acknowledgementMode;
		public readonly IBM.XMS.IConnection xmsConnection;
		private IRedeliveryPolicy redeliveryPolicy;
		private ConnectionMetaData metaData = null;
		private readonly Atomic<bool> started = new Atomic<bool>(false);
		private bool closed = false;
		private bool disposed = false;
		
		#region Constructors
		
		/// <summary>
		/// Constructs a connection object.
		/// </summary>
		public Connection(IBM.XMS.IConnection xmsConnection)
		{
			this.xmsConnection = xmsConnection;
			this.xmsConnection.ExceptionListener = this.HandleXmsException;
		}
		
		/// <summary>
		/// "Destructs" or "finalizes" a connection object.
		/// </summary>
		~Connection()
		{
			Dispose(false);
		}
		
		#endregion
		
		#region IStartable Members
		
		/// <summary>
		/// Starts message delivery for this connection.
		/// </summary>
		public void Start()
		{
			if(started.CompareAndSet(false, true))
			{
				try
				{
					this.xmsConnection.Start();
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}
		
		public bool IsStarted
		{
			get { return this.started.Value; }
		}
		
		#endregion
		
		#region IStoppable Members
		
		/// <summary>
		/// Stop message delivery for this connection.
		/// </summary>
		public void Stop()
		{
			try
			{
				if(started.CompareAndSet(true, false))
				{
					this.xmsConnection.Stop();
				}
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}
		
		#endregion
		
		#region IConnection Members
		
		/// <summary>
		/// Creates a new session to work on this connection
		/// </summary>
		public Apache.NMS.ISession CreateSession()
		{
			return CreateSession(acknowledgementMode);
		}
		
		/// <summary>
		/// Creates a new session to work on this connection
		/// </summary>
		public Apache.NMS.ISession CreateSession(
			Apache.NMS.AcknowledgementMode mode)
		{
			try
			{
				bool isTransacted =
					(mode == Apache.NMS.AcknowledgementMode.Transactional);
				return XMSConvert.ToNMSSession(
					this.xmsConnection.CreateSession(
						isTransacted, XMSConvert.ToAcknowledgeMode(mode)));
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public void Close()
		{
			lock(this)
			{
				if(closed)
				{
					return;
				}
		
				try
				{
					this.xmsConnection.ExceptionListener = null;
					this.xmsConnection.Stop();
					this.xmsConnection.Close();
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
				finally
				{
					closed = true;
				}
			}
		}
		
		public void PurgeTempDestinations()
		{
		}
		
		#endregion
		
		#region IDisposable Members
		
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		protected void Dispose(bool disposing)
		{
			if(disposed)
			{
				return;
			}
		
			if(disposing)
			{
				// Dispose managed code here.
			}
		
			try
			{
				Close();
			}
			catch
			{
				// Ignore errors.
			}
		
			disposed = true;
		}
		
		#endregion
		
		#region Attributes
		
		/// <summary>
		/// The default timeout for network requests.
		/// </summary>
		public TimeSpan RequestTimeout
		{
			get { return Apache.NMS.NMSConstants.defaultRequestTimeout; }
			set { }
		}
		
		public Apache.NMS.AcknowledgementMode AcknowledgementMode
		{
			get { return acknowledgementMode; }
			set { acknowledgementMode = value; }
		}
		
		public string ClientId
		{
			get
			{
				try
				{
					return this.xmsConnection.ClientID;
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
					this.xmsConnection.ClientID = value;
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}
		
		/// <summary>
		/// Get/or set the redelivery policy for this connection.
		/// </summary>
		public IRedeliveryPolicy RedeliveryPolicy
		{
			get { return this.redeliveryPolicy; }
			set { this.redeliveryPolicy = value; }
		}
		
		/// <summary>
		/// Gets the Meta Data for the NMS Connection instance.
		/// </summary>
		public IConnectionMetaData MetaData
		{
			get { return this.metaData ?? (this.metaData = new ConnectionMetaData()); }
		}
		
		#endregion
		
		#region Properties
		
		// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/props_conn.htm?lang=en
		
		#region Common properties
		
		/// <summary>
		/// This property is used to obtain the name of the queue manager
		/// to which it is connected.
		/// </summary>
		public string ResolvedQueueManagerName
		{
			get { return this.xmsConnection.GetStringProperty(XMSC.WMQ_RESOLVED_QUEUE_MANAGER); }
			set { this.xmsConnection.SetStringProperty(XMSC.WMQ_RESOLVED_QUEUE_MANAGER, value); }
		}
		
		/// <summary>
		/// This property is populated with the ID of the queue manager
		/// after the connection.
		/// </summary>
		public string ResolvedQueueManagerId
		{
			get { return this.xmsConnection.GetStringProperty(XMSC.WMQ_RESOLVED_QUEUE_MANAGER_ID); }
			set { this.xmsConnection.SetStringProperty(XMSC.WMQ_RESOLVED_QUEUE_MANAGER_ID, value); }
		}
		
		#endregion
		
		#region WPM-specific properties
		
		/// <summary>
		/// The communications protocol used for the connection to the
		/// messaging engine. This property is read-only.
		/// </summary>
		public Int32 XMSConnectionProtocol
		{
			get { return this.xmsConnection.GetIntProperty(XMSC.WPM_CONNECTION_PROTOCOL); }
		}
		
		/// <summary>
		/// The communications protocol used for the connection to the
		/// messaging engine. This property is read-only.
		/// </summary>
		public WPMConnectionProtocol ConnectionProtocol
		{
			get { return XMSConvert.ToWPMConnectionProtocol(this.XMSConnectionProtocol); }
		}
		
		/// <summary>
		/// The host name or IP address of the system that contains the
		/// messaging engine to which the application is connected. This
		/// property is read-only.
		/// </summary>
		public string HostName
		{
			get { return this.xmsConnection.GetStringProperty(XMSC.WPM_HOST_NAME); }
		}
		
		/// <summary>
		/// The name of the messaging engine to which the application is
		/// connected. This property is read-only.
		/// </summary>
		public string MessagingEngineName
		{
			get { return this.xmsConnection.GetStringProperty(XMSC.WPM_ME_NAME); }
		}
		
		/// <summary>
		/// The number of the port listened on by the messaging engine to
		/// which the application is connected. This property is read-only.
		/// </summary>
		public Int32 Port
		{
			get { return this.xmsConnection.GetIntProperty(XMSC.WPM_PORT); }
		}
		
		#endregion
		
		#endregion
		
		#region Event Listeners, Handlers and Delegates
		
		/// <summary>
		/// A delegate that can receive transport level exceptions.
		/// </summary>
		public event ExceptionListener ExceptionListener;
		
		/// <summary>
		/// Handles XMS connection exceptions.
		/// </summary>
		private void HandleXmsException(Exception exception)
		{
			if(ExceptionListener != null)
			{
				// Return codes MQRC_RECONNECTING and MQRC_RECONNECTED
				// are not defined in XMS.
				// const int MQRC_RECONNECTING = 2544;
				// const int MQRC_RECONNECTED = 2545;
				// According to http://www-01.ibm.com/support/knowledgecenter/#!/SSFKSJ_8.0.0/com.ibm.mq.con.doc/q017800_.htm
				// Except for JMS and XMS clients, if a client application has
				// access to reconnection options, it can also create an event
				// handler to handle reconnection events.
				ExceptionListener(exception);
			}
			else
			{
				Apache.NMS.Tracer.Error(exception);
			}
		}
		
		/// <summary>
		/// An asynchronous listener that is notified when a fault tolerant
		/// connection has been interrupted.
		/// </summary>
		/// <remarks>
		/// IBM XMS does not handle disconnection / reconnection notifications.
		/// This delegate will never be called.
		/// </remarks>
		public event ConnectionInterruptedListener ConnectionInterruptedListener;
		
		private void HandleTransportInterrupted()
		{
			Tracer.Debug("Transport has been interrupted.");
		
			if(this.ConnectionInterruptedListener != null && !this.closed)
			{
				try
				{
					this.ConnectionInterruptedListener();
				}
				catch
				{
				}
			}
		}
		
		/// <summary>
		/// An asynchronous listener that is notified when a fault tolerant
		/// connection has been resumed.
		/// </summary>
		/// <remarks>
		/// IBM XMS does not handle disconnection / reconnection notifications.
		/// This delegate will never be called.
		/// </remarks>
		public event ConnectionResumedListener ConnectionResumedListener;
		
		private void HandleTransportResumed()
		{
			Tracer.Debug("Transport has resumed normal operation.");
		
			if(this.ConnectionResumedListener != null && !this.closed)
			{
				try
				{
					this.ConnectionResumedListener();
				}
				catch
				{
				}
			}
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
