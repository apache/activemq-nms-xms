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

namespace Apache.NMS.XMS.Util
{
    /// <summary>
    /// Attribute for mapping a URI attribute key to an object's property.
    /// </summary>
    public class UriAttributeAttribute : System.Attribute
    {
        private readonly string[] attributeKeys;

        /// <summary>
        /// Constructs an <c>UriAttributeAttribute</c> specifying a list
        /// of attribute keys.
        /// </summary>
        /// <param name="keys">URI attribute keys.</param>
        public UriAttributeAttribute(params string[] keys)
        {
            this.attributeKeys = new string[keys.Length];
            for(int k = 0; k < keys.Length; k++)
            {
                this.attributeKeys[k] = keys[k];
            }
        }

        /// <summary>
        /// URI attribute keys.
        /// </summary>
        public string[] AttributeKeys
        {
            get { return this.attributeKeys; }
        }
    }
}
