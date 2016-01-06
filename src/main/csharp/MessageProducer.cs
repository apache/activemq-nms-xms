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
using System.Threading;
using Apache.NMS;
using Apache.NMS.Util;
using Apache.NMS.XMS.Util;

namespace Apache.NMS.XMS
{
	class MessageProducer : Apache.NMS.IMessageProducer
	{
		protected readonly Apache.NMS.XMS.Session nmsSession;
		public IBM.XMS.IMessageProducer xmsMessageProducer;
		private TimeSpan requestTimeout = NMSConstants.defaultRequestTimeout;
		private bool closed = false;
		private bool disposed = false;

		public MessageProducer(Apache.NMS.XMS.Session session,
			IBM.XMS.IMessageProducer producer)
		{
			this.nmsSession = session;
			this.xmsMessageProducer = producer;
			this.RequestTimeout = session.RequestTimeout;
		}

		~MessageProducer()
		{
			Dispose(false);
		}

		private Apache.NMS.XMS.Message GetXMSMessage(Apache.NMS.IMessage message)
		{
			Apache.NMS.XMS.Message msg = (Apache.NMS.XMS.Message) message;

			if(this.ProducerTransformer != null)
			{
				IMessage transformed = this.ProducerTransformer(this.nmsSession, this, message);
				if(transformed != null)
				{
					msg = (Apache.NMS.XMS.Message) transformed;
				}
			}

			return msg;
		}

		#region IMessageProducer Members

		/// <summary>
		/// Sends the message to the default destination for this producer.
		/// </summary>
		public void Send(Apache.NMS.IMessage message)
		{
			Apache.NMS.XMS.Message msg = GetXMSMessage(message);
			long timeToLive = (long) message.NMSTimeToLive.TotalMilliseconds;

			if(0 == timeToLive)
			{
				timeToLive = this.xmsMessageProducer.TimeToLive;
			}

			try
			{
				msg.OnSend();
				this.xmsMessageProducer.Send(
							msg.xmsMessage,
							this.xmsMessageProducer.DeliveryMode,
							this.xmsMessageProducer.Priority,
							timeToLive);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Sends the message to the default destination with the explicit QoS
		/// configuration.
		/// </summary>
		public void Send(Apache.NMS.IMessage message,
			MsgDeliveryMode deliveryMode, MsgPriority priority,
			TimeSpan timeToLive)
		{
			Apache.NMS.XMS.Message msg = GetXMSMessage(message);

			try
			{
				this.xmsMessageProducer.Send(
							msg.xmsMessage,
							XMSConvert.ToJMSDeliveryMode(deliveryMode),
							(int)priority,
							(long)timeToLive.TotalMilliseconds);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Sends the message to the given destination.
		/// </summary>
		public void Send(Apache.NMS.IDestination destination,
			Apache.NMS.IMessage message)
		{
			Apache.NMS.XMS.Destination dest =
				(Apache.NMS.XMS.Destination)destination;

			Apache.NMS.XMS.Message msg = GetXMSMessage(message);
			long timeToLive = (long)message.NMSTimeToLive.TotalMilliseconds;

			if(0 == timeToLive)
			{
				timeToLive = this.xmsMessageProducer.TimeToLive;
			}

			try
			{
				this.xmsMessageProducer.Send(
							dest.xmsDestination,
							msg.xmsMessage,
							this.xmsMessageProducer.DeliveryMode,
							this.xmsMessageProducer.Priority,
							timeToLive);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		/// <summary>
		/// Sends the message to the given destination with the explicit QoS
		/// configuration.
		/// </summary>
		public void Send(Apache.NMS.IDestination destination,
			Apache.NMS.IMessage message, MsgDeliveryMode deliveryMode,
			MsgPriority priority, TimeSpan timeToLive)
		{
			Apache.NMS.XMS.Destination dest =
				(Apache.NMS.XMS.Destination)destination;

			Apache.NMS.XMS.Message msg = GetXMSMessage(message);

			try
			{
				this.xmsMessageProducer.Send(
							dest.xmsDestination,
							msg.xmsMessage,
							XMSConvert.ToJMSDeliveryMode(deliveryMode),
							(int)priority,
							(long)timeToLive.TotalMilliseconds);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		private ProducerTransformerDelegate producerTransformer;
		/// <summary>
		/// A delegate that is called each time a Message is sent from this
		/// Producer which allows the application to perform any needed
		/// transformations on the Message before it is sent.
		/// </summary>
		public ProducerTransformerDelegate ProducerTransformer
		{
			get { return this.producerTransformer; }
			set { this.producerTransformer = value; }
		}

		public MsgDeliveryMode DeliveryMode
		{
			get
			{
				return XMSConvert.ToNMSMsgDeliveryMode(
					this.xmsMessageProducer.DeliveryMode);
			}
			set
			{
				try
				{
					this.xmsMessageProducer.DeliveryMode =
						XMSConvert.ToJMSDeliveryMode(value);
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		public TimeSpan TimeToLive
		{
			get
			{
				return TimeSpan.FromMilliseconds(
					this.xmsMessageProducer.TimeToLive);
			}
			set
			{
				try
				{
					this.xmsMessageProducer.TimeToLive =
						(long)value.TotalMilliseconds;
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		/// <summary>
		/// The default timeout for network requests.
		/// </summary>
		public TimeSpan RequestTimeout
		{
			get { return requestTimeout; }
			set { this.requestTimeout = value; }
		}

		public MsgPriority Priority
		{
			get { return (MsgPriority) this.xmsMessageProducer.Priority; }
			set
			{
				try
				{
					this.xmsMessageProducer.Priority = (int) value;
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		public bool DisableMessageID
		{
			get { return this.xmsMessageProducer.DisableMessageID; }
			set
			{
				try
				{
					this.xmsMessageProducer.DisableMessageID = value;
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		public bool DisableMessageTimestamp
		{
			get { return this.xmsMessageProducer.DisableMessageTimestamp; }
			set
			{
				try
				{
					this.xmsMessageProducer.DisableMessageTimestamp = value;
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		/// <summary>
		/// Creates a new message with an empty body
		/// </summary>
		public Apache.NMS.IMessage CreateMessage()
		{
			return this.nmsSession.CreateMessage();
		}

		/// <summary>
		/// Creates a new text message with an empty body.
		/// </summary>
		public Apache.NMS.ITextMessage CreateTextMessage()
		{
			return this.nmsSession.CreateTextMessage();
		}

		/// <summary>
		/// Creates a new text message with the given body.
		/// </summary>
		public Apache.NMS.ITextMessage CreateTextMessage(string text)
		{
			return this.nmsSession.CreateTextMessage(text);
		}

		/// <summary>
		/// Creates a new Map message which contains primitive key and value
		/// pairs.
		/// </summary>
		public Apache.NMS.IMapMessage CreateMapMessage()
		{
			return this.nmsSession.CreateMapMessage();
		}

		/// <summary>
		/// Creates a new object message containing the given .NET object as
		/// the body.
		/// </summary>
		public Apache.NMS.IObjectMessage CreateObjectMessage(object body)
		{
			return this.nmsSession.CreateObjectMessage(body);
		}

		/// <summary>
		/// Creates a new binary message.
		/// </summary>
		public Apache.NMS.IBytesMessage CreateBytesMessage()
		{
			return this.nmsSession.CreateBytesMessage();
		}

		/// <summary>
		/// Creates a new binary message with the given body.
		/// </summary>
		public Apache.NMS.IBytesMessage CreateBytesMessage(byte[] body)
		{
			return this.nmsSession.CreateBytesMessage(body);
		}

		/// <summary>
		/// Creates a new stream message.
		/// </summary>
		public Apache.NMS.IStreamMessage CreateStreamMessage()
		{
			return this.nmsSession.CreateStreamMessage();
		}

		#endregion

		#region IDisposable Members

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
					this.xmsMessageProducer.Close();
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
		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
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
	}
}
