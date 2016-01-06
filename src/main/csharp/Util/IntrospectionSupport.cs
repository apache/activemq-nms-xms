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
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Specialized;
using Apache.NMS;
using Apache.NMS.Util;

namespace Apache.NMS.XMS.Util
{
	/// <summary>
	/// Utility class used to provide convenience methods that apply named
	/// property settings to objects.
	/// </summary>
	public class IntrospectionSupport
	{
        #region Manage maps of member names and URI aliases

		private static Dictionary<Type, StringDictionary> nameMaps =
			new Dictionary<Type, StringDictionary>();
		private static readonly object nameMapsLock = new object();

		/// <summary>
		/// Gets the member names map for the specified type.
		/// </summary>
		/// <param name="type">Type whose names map is requested.</param>
		/// <returns>Names map for the specified type.</returns>
        /// <remarks>
		/// The map is created and registered if it is not found in the
		/// <c>nameMaps</c> registry.
        /// </remarks>
		public static StringDictionary GetNameMap(Type type)
		{
			StringDictionary nameMap;
			lock(IntrospectionSupport.nameMapsLock)
			{
				if(!IntrospectionSupport.nameMaps.TryGetValue(
					type, out nameMap))
				{
					nameMap = CreateNameMap(type);
					IntrospectionSupport.nameMaps.Add(type, nameMap);
				}
			}
			return nameMap;
		}

		/// <summary>
		/// Creates a dictionary of public property and attribute names,
        /// indexed by themselves plus all URI attribute keys associated
		/// to them.
		/// </summary>
		/// <param name="type">Type whose names map is requested.</param>
		/// <returns>Names map for the specified type.</returns>
		/// <remarks>
		/// Applied to this property:
		/// <code>
		///   [UriAttribute("My.Test", "MyTest")]
		///   public string Test
		///   { get { return(_test); }
		///     set { _test = value; }
		///   }
		/// </code>
		/// the method returns a dictionary containing
		/// ("test" -> "Test"), ("my.test" -> "Test"), ("mytest" -> "Test").
		/// Note that <c>StringDictionary</c> converts keys to lowercase but
		/// keeps values untouched.
		/// </remarks>
		public static StringDictionary CreateNameMap(Type type)
		{
			StringDictionary nameMap = new StringDictionary();
			BindingFlags flags = BindingFlags.FlattenHierarchy
							| BindingFlags.Public
							| BindingFlags.Instance;

			// Process public instance self or inherited property
			foreach(PropertyInfo propertyInfo in type.GetProperties(flags))
		    {
				AddToNameMap(nameMap, propertyInfo);
			}

			// Process public instance self or inherited fields
			foreach(FieldInfo fieldInfo in type.GetFields(flags))
		    {
				AddToNameMap(nameMap, fieldInfo);
			}

			return(nameMap);
		}

		/// <summary>
		/// Adds a property or field name and URI attribute keys to the
		/// specified name map.
		/// </summary>
		/// <param name="nameMap">Name map.</param>
		/// <param name="memberInfo">Member information for the property
		/// or field.</param>
		private static void AddToNameMap(StringDictionary nameMap,
			MemberInfo memberInfo)
		{
			// Add member name mapped to itself
			nameMap.Add(memberInfo.Name, memberInfo.Name);

			// For each UriAttribute custom attribute
			foreach(Attribute attr in memberInfo.GetCustomAttributes(
				typeof(UriAttributeAttribute), true))
			{
				// For each URI attribute key
				foreach(string key in
					((UriAttributeAttribute)attr).AttributeKeys)
				{
					// Index property name by URI attribute key
					if(!nameMap.ContainsKey(key))
					{
						nameMap.Add(key, memberInfo.Name);
					}
				}
			}

			return;
		}

        #endregion

		#region Set properties

		/// <summary>
		/// Sets the public properties of a target object using a string map.
		/// This method uses .Net reflection to identify public properties of
		/// the target object matching the keys from the passed map.
		/// </summary>
		/// <param name="target">Object whose properties will be set.</param>
		/// <param name="valueMap">Map of key/value pairs.</param>
		public static void SetProperties(object target,
			StringDictionary valueMap)
		{
			SetProperties(target, valueMap, GetNameMap(target.GetType()));
		}

        /// <summary>
        /// Sets the public properties of a target object using a string map.
        /// This method uses .Net reflection to access public properties of
        /// the target object matching the keys from the passed map.
        /// </summary>
        /// <param name="target">The object whose properties will be set.</param>
        /// <param name="valueMap">Map of key/value pairs.</param>
        /// <param name="nameMap">Map of key/property name pairs.</param>
        public static void SetProperties(object target,
            StringDictionary valueMap,
			StringDictionary nameMap)
        {
			Tracer.DebugFormat("SetProperties called with target: {0}",
				target.GetType().Name);

			// Application of specified values is recursive. If a key does not
			// correspond to a member of the current target object, it is
			// supposed to refer to a sub-member of such a member. Since member
			// keys can contain dot characters, an attempt is made to find the
			// "longest" key corresponding to a member of the current object
			// (this identifies the "sub-target"), and extract the remaining
			// key characters as a sub-key to sub-members.
			// The following dictionary indexes keys to "sub-targets", and
			// "sub-key"/value pairs to assign to "sub-targets".
			Dictionary<string, StringDictionary> subTargetMap = null;

			foreach(string key in valueMap.Keys)
			{
				if(nameMap.ContainsKey(key))
				{
					// Key refers to a member of the current target
					string memberName = nameMap[key];
					MemberInfo member = FindMemberInfo(target, memberName);
					if(member == null)
					{
						// Should not happen if the nameMap was indeed created
						// for the current target object...
						throw new NMSException(string.Format(
							"No such property or field: {0} on class: {1}",
							memberName, target.GetType().Name));
					}

					// Set value
					try
					{
						if(member.MemberType == MemberTypes.Property)
						{
							PropertyInfo property = (PropertyInfo)member;
							object value = ConvertValue(valueMap[key],
								property.PropertyType);
							property.SetValue(target, value, null);
						}
						else
						{
							FieldInfo field = (FieldInfo)member;
							object value = ConvertValue(valueMap[key],
								field.FieldType);
							field.SetValue(target, value);
						}
					}
					catch(Exception ex)
					{
						throw NMSExceptionSupport.Create(
							"Error while attempting to apply option.", ex);
					}
                }
				else
				{
					// Key does NOT refers to a member of the current target
					// Extract maximal member key + subkeys
					string memberKey = key;
					int dotPos = memberKey.LastIndexOf('.');
					bool memberFound = false;
					while(!memberFound && dotPos > 0)
					{
						memberKey = memberKey.Substring(0, dotPos);
						if(nameMap.ContainsKey(memberKey))
						{
							memberKey = nameMap[memberKey];
							memberFound = true;
						}
						else
						{
							dotPos = memberKey.LastIndexOf('.');
						}
					}

					if(!memberFound)
					{
						throw new NMSException(string.Format(
							"Unknown property or field: {0} on class: {1}",
							key, target.GetType().Name));
					}

					// Register memberKey, subKey and value for further processing
					string subKey = key.Substring(dotPos + 1);
					StringDictionary subValueMap;

					if(subTargetMap == null)
					{
						subTargetMap = new Dictionary<string, StringDictionary>();
					}

					if(!subTargetMap.TryGetValue(memberKey, out subValueMap))
					{
						subValueMap = new StringDictionary();
						subTargetMap.Add(memberKey, subValueMap);
					}

					// In theory, we can't have the same subkey twice, since
					// they were unique subkeys from another dictionary.
					// Therefore, no need to check for subValueMap.ContainsKey.
					subValueMap.Add(subKey, valueMap[key]);
				}
            }

			// Now process any compound assignments.
			if(subTargetMap != null)
			{
				foreach(string subTargetKey in subTargetMap.Keys)
				{
					MemberInfo member = FindMemberInfo(target, subTargetKey);
					object subTarget = GetUnderlyingObject(member, target);
					SetProperties(subTarget, subTargetMap[subTargetKey]);
				}
			}
        }

		/// <summary>
		/// Converts the specified string value to the type of the target
		/// member.
		/// </summary>
		private static object ConvertValue(string inputString, Type targetType)
		{
			// If the target member is an enumeration, get the enumeration
			// value or combined (or-ed) values
			object value;
			if(targetType.IsEnum)
			{
				if(inputString.Contains("+"))
				{
					string[] inputValues = inputString.Split('+');

					FieldInfo fieldInfo = targetType.GetField(inputValues[0],
						BindingFlags.Public
						| BindingFlags.Static
						| BindingFlags.IgnoreCase);
					if(fieldInfo == null)
					{
						throw new NMSException(string.Format(
							"Invalid {0} value \"{1}\"", targetType.Name,
							inputValues[0]));
					}
					dynamic val = fieldInfo.GetValue(null);

					for(int v = 1; v < inputValues.Length; v++)
					{
						fieldInfo = targetType.GetField(inputValues[v],
							BindingFlags.Public
							| BindingFlags.Static
							| BindingFlags.IgnoreCase);
						if(fieldInfo == null)
						{
							throw new NMSException(string.Format(
								"Invalid {0} value \"{1}\"", targetType.Name,
								inputValues[v]));
						}
						val = (dynamic)val | (dynamic)fieldInfo.GetValue(null);
					}

					value = Convert.ChangeType(val, targetType);
				}
				else
				{
					FieldInfo fieldInfo = targetType.GetField(inputString,
                                 BindingFlags.Public
                               | BindingFlags.Static
                               | BindingFlags.IgnoreCase);
					if(fieldInfo == null)
					{
						throw new NMSException(string.Format(
							"Invalid {0} value \"{1}\"", targetType.Name,
							inputString));
					}
					value = fieldInfo.GetValue(null);
				}
			}
			else
			{
				// Not an enumeration
				value = Convert.ChangeType(inputString,
					targetType, CultureInfo.InvariantCulture);
			}
			return value;
		}

		#endregion

		#region Get member information and objects

		/// <summary>
		/// Gets member information for a property or field of the target
		/// object.
		///	</summary>
		/// <param name="target">Target object.</param>
		/// <param name="name">Property or field name.</param>
		/// <returns>Retrieved member information.</returns>
        private static MemberInfo FindMemberInfo(object target, string name)
        {
            BindingFlags flags = BindingFlags.FlattenHierarchy
                               | BindingFlags.Public
                               | BindingFlags.Instance
                               | BindingFlags.IgnoreCase;

            Type type = target.GetType();

            MemberInfo member = type.GetProperty(name, flags);

            if(member == null)
            {
                member = type.GetField(name, flags);
            }

            return member;
        }

		/// <summary>
		/// Gets object assigned to the specified property or field member of
		/// the target object.
		///	</summary>
		/// <param name="member">Member information.</param>
		/// <param name="target">Target object.</param>
		/// <returns>Retrieved object.</returns>
		private static object GetUnderlyingObject(
			MemberInfo member, object target)
		{
			object result = null;

			if(member.MemberType == MemberTypes.Field)
			{
				FieldInfo field = member as FieldInfo;

				if(field.FieldType.IsPrimitive)
				{
					throw new NMSException(string.Format(
						"The field given is a primitive type: {0}",
						member.Name));
				}

				result = field.GetValue(target);
			}
			else
			{
				PropertyInfo property = member as PropertyInfo;
				MethodInfo getter = property.GetGetMethod();

				if(getter == null)
				{
					throw new NMSException(string.Format(
						"Cannot access member: {0}",
						member.Name));
				}

				result = getter.Invoke(target, null);
			}

			if(result == null)
			{
				throw new NMSException(string.Format(
					"Could not retrieve the value of member {0}.",
					member.Name));
			}

			return result;
		}

		#endregion
    }
}
