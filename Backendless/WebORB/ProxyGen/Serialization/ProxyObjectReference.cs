// Copyright 2004-2007 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


namespace Weborb.ProxyGen.DynamicProxy.Serialization
{
	using System;
	using System.Reflection;
	using System.Runtime.Serialization;
	using Weborb.ProxyGen.Core.Interceptor;
	using Weborb.ProxyGen.DynamicProxy.Generators;

	public class ProxyObjectReference
	{
		private static ModuleScope _scope = new ModuleScope();

        private readonly Type _baseType;
		private readonly Type[] _interfaces;
		private readonly object _proxy;
		private readonly ProxyGenerationOptions _proxyGenerationOptions;

		private bool _isInterfaceProxy;
		private bool _delegateToBase;

		public static void ResetScope()
		{
			_scope = new ModuleScope();
		}

		protected ProxyObjectReference()
		{
			_baseType = DeserializeTypeFromString("__baseType");
			_proxy = RecreateProxy ();
		}

		private Type DeserializeTypeFromString (string key)
		{
			//return Type.GetType(_info.GetString(key), true, false);
            throw new Exception( "this class should not be used" );
		}

		protected virtual object RecreateProxy ()
		{
			if (_baseType == typeof(object)) // TODO: replace this hack by serializing a flag or something
			{
				_isInterfaceProxy = true;
				return RecreateInterfaceProxy();
			}
			else
			{
				_isInterfaceProxy = false;
				return RecreateClassProxy();
			}
		}

		public object RecreateInterfaceProxy()
		{
            throw new Exception( "should never get here" );
            /*
			InterfaceGeneratorType generatorType = (InterfaceGeneratorType) _info.GetInt32("__interface_generator_type");
			
			Type theInterface = DeserializeTypeFromString ("__theInterface");
			Type targetType = DeserializeTypeFromString ("__targetFieldType");
			
			InterfaceProxyWithTargetGenerator generator;
			switch(generatorType)
			{
				case InterfaceGeneratorType.WithTarget:
					generator = new InterfaceProxyWithTargetGenerator(_scope, theInterface);
					break;
				case InterfaceGeneratorType.WithoutTarget:
					generator = new InterfaceProxyWithoutTargetGenerator(_scope, theInterface);
					break;
				case InterfaceGeneratorType.WithTargetInterface:
					generator = new InterfaceProxyWithTargetInterfaceGenerator(_scope, theInterface);
					break;
				default:
					throw new InvalidOperationException(
						string.Format(
							"Got value {0} for the interface generator type, which is not known for the purpose of serialization.",
							generatorType));
			}

			Type proxy_type = generator.GenerateCode(targetType, _interfaces, _proxyGenerationOptions);
			return FormatterServices.GetSafeUninitializedObject (proxy_type);
             */ 
		}

		public object RecreateClassProxy()
		{
            throw new Exception( "should never get here" );
            /*
			_delegateToBase = _info.GetBoolean ("__delegateToBase");

			ClassProxyGenerator cpGen = new ClassProxyGenerator(_scope, _baseType);

			Type proxy_type = cpGen.GenerateCode(_interfaces, _proxyGenerationOptions);


			if (_delegateToBase)
			{
				return Activator.CreateInstance(proxy_type, new object[] {_info, _context});
			}
			else
			{
				return FormatterServices.GetSafeUninitializedObject(proxy_type);
			}
             */ 
		}

		protected void InvokeCallback(object target)
		{
            throw new Exception( "should never get here" );
            /*
			if (target is IDeserializationCallback)
			{
				(target as IDeserializationCallback).OnDeserialization(this);
			}
             */ 
		}

		public object GetRealObject()
		{
			return _proxy;
		}

		public void GetObjectData()
		{
			// There is no need to implement this method as 
			// this class would never be serialized.
		}

		public void OnDeserialization (object sender)
		{
            /*
			IInterceptor[] _interceptors = (IInterceptor[]) _info.GetValue ("__interceptors", typeof (IInterceptor[]));
			SetInterceptors (_interceptors);
			
			if (_isInterfaceProxy)
			{
				object target = _info.GetValue ("__target", typeof (object));
				SetTarget (target);
			}
			else if (!_delegateToBase)
			{
				object[] baseMemberData = (object[]) _info.GetValue ("__data", typeof (object[]));
				MemberInfo[] members = FormatterServices.GetSerializableMembers (_baseType);
				FormatterServices.PopulateObjectMembers (_proxy, members, baseMemberData);
			}

			InvokeCallback (_proxy);
             */ 
		}

		private void SetTarget (object target)
		{
			FieldInfo targetField = _proxy.GetType ().GetField ("__target");
			if (targetField == null)
			{
				throw new SerializationException (
					"The SerializationInfo specifies an invalid interface proxy type, which has no __target field.");
			}

			targetField.SetValue (_proxy, target);
		}

		private void SetInterceptors (IInterceptor[] interceptors)
		{
			FieldInfo interceptorField = _proxy.GetType ().GetField ("__interceptors");
			if (interceptorField == null)
			{
				throw new SerializationException (
					"The SerializationInfo specifies an invalid proxy type, which has no __interceptors field.");
			}

			interceptorField.SetValue (_proxy, interceptors);
		}
	}
}
