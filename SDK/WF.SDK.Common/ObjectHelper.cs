using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace WF.SDK.Common
{
  /// <summary>
  /// This class provides a set of static methods to help with object types.
  /// </summary>
  public static class ObjectHelper
  {
    public const BindingFlags ALL_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    public const BindingFlags CONSTRUCTOR_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    public static BindingFlags flags = ALL_FLAGS;

    #region GetMember

    /// <summary>
    /// Returns a reference to an object's hidden member of the given name, and returns the type as T.
    /// This works on instance and static private/internal/protected members and fields.  Returns default(T)
    /// if obj is null.  The match delegate will work on the name and the member name to try to find a match
    /// </summary>
    public static T GetMember<T>(object obj, string name, Func<string, string> matchDelegate = null)
    {
      if (obj == null) { return default(T); }
      return ObjectHelper.GetMember<T>(obj.GetType(), name, obj, matchDelegate);
    }


    /// <summary>
    /// Returns a reference to an object's hidden member of the given name, and returns the type as T.
    /// This works on instance and static private/internal/protected members and fields.  When the instance is null,
    /// only static members are returned.  If type parameter is null, returns default<T>.
    /// </summary>
    public static T GetMember<T>(Type type, string name, object obj = null, Func<string, string> matchDelegate = null)
    {
      if (type == null) { return default(T); }
      T ret = default(T);
      string fixedName = name;
      if (matchDelegate != null) { fixedName = matchDelegate(name); }
      try
      {
        FieldInfo[] list = type.GetFields(ObjectHelper.flags);
        foreach (FieldInfo inf in list)
        {
          if (matchDelegate == null && inf.Name == fixedName) { ret = (T)inf.GetValue(obj); return ret; }
          if (matchDelegate != null && matchDelegate(inf.Name) == fixedName) { ret = (T)inf.GetValue(obj); return ret; }
        }
      }
      catch (Exception e) { throw e; }
      try
      {
        PropertyInfo[] list = type.GetProperties(ObjectHelper.flags);
        foreach (PropertyInfo inf in list)
        {
          if (matchDelegate == null && inf.Name == fixedName) { ret = (T)inf.GetValue(obj, BindingFlags.GetProperty | ObjectHelper.flags, null, null, null); return ret; }
          if (matchDelegate != null && matchDelegate(inf.Name) == fixedName) { ret = (T)inf.GetValue(obj, BindingFlags.GetProperty | ObjectHelper.flags, null, null, null); return ret; }
        }
      }
      catch (Exception e) { throw e; }
      return ret;
    }

    #endregion

    #region GetMemberType

    /// <summary>
    /// Returns the System.Type object for a member of a class.  This works on instance
    /// and static private/internal/protected members and fields.  Returns null
    /// if obj is null.
    /// </summary>
    public static Type GetMemberType(object obj, string name, Func<string, string> matchDelegate = null)
    {
      if (obj == null) { return null; }
      return ObjectHelper.GetMemberType(obj.GetType(), name, obj, matchDelegate);
    }


    /// <summary>
    /// Returns a reference to an object's hidden member of the given name, and returns the type as T.
    /// This works on instance and static private/internal/protected members and fields.  When the instance is null,
    /// only static members are returned.
    /// </summary>
    public static Type GetMemberType(Type type, string name, object obj = null, Func<string, string> matchDelegate = null)
    {
      Type ret = null;
      if (type == null) { return ret; }
      string fixedName = name;
      if (matchDelegate != null) { fixedName = matchDelegate(name); }
      try
      {
        FieldInfo[] list = type.GetFields(ObjectHelper.flags);
        foreach (FieldInfo inf in list)
        {
          if (matchDelegate == null && inf.Name == fixedName) { ret = inf.FieldType; return ret; }
          if (matchDelegate != null && matchDelegate(inf.Name) == fixedName) { ret = inf.FieldType; return ret; }
        }
      }
      catch (Exception e) { throw e; }

      try
      {
        PropertyInfo[] list = type.GetProperties(ObjectHelper.flags);
        foreach (PropertyInfo inf in list)
        {
          if (matchDelegate == null && inf.Name == fixedName) { ret = inf.PropertyType; return ret; }
          if (matchDelegate != null && matchDelegate(inf.Name) == fixedName) { ret = inf.PropertyType; return ret; }
        }
      }
      catch (Exception e) { throw e; }
      return ret;
    }

    #endregion

    #region SetMember
    /// <summary>
    /// Sets the value of the indicated field on the 
    /// This works on static and private members and fields.
    /// </summary>
    public static void SetMember<T>(object obj, string name, T value)
    {
      ObjectHelper.SetMember<T>(obj.GetType(), name, value, obj);
    }

    /// <summary>
    /// Sets the value of the indicated field on the 
    /// This works on static and private members and fields.
    /// </summary>
    public static void SetMember<T>(Type type, string name, T value, object obj = null)
    {

      try
      {
        FieldInfo[] list = type.GetFields(flags);
        foreach (FieldInfo inf in list)
        {
          if (inf.Name == name)
          {
            inf.SetValue(obj, value, BindingFlags.SetField | ObjectHelper.flags, null, null);
            return;
          }
        }
      }
      catch (Exception e) { throw e; }

      try
      {
        PropertyInfo[] list = type.GetProperties(flags);
        foreach (PropertyInfo inf in list)
        {
          if (inf.Name == name)
          {
            inf.SetValue(obj, value, BindingFlags.SetProperty | ObjectHelper.flags, null, null, null);
            return;
          }
        }
      }
      catch (Exception e) { throw e; }
      //Should never get here.
      throw new Exception("Could not find that member name on the object.");
    }
    #endregion

    #region CallMember
    /// <summary>
    /// Calls the indicated method, returning the type at T.
    /// This works on static and private methods.
    /// </summary>
    public static T CallMember<T>(object obj, string methodName, params object[] methodParams)
    {
      //Object Type
      Type t = obj.GetType();
      //Type array of the parameters for overload matching
      Type[] types = new Type[0];
      if (methodParams != null && methodParams.Length > 0) { types = methodParams.ToList().Select(i => i.GetType()).ToArray(); }

      //Find the method.
      MethodInfo inf = null;
      //Try to find the method
      if (inf == null) { try { inf = t.GetMethod(methodName, ObjectHelper.flags, null, types, null); } catch { } }
      //Try again without methodParams
      if (inf == null) { try { inf = t.GetMethod(methodName, ObjectHelper.flags); } catch { } }

      //Try the base?
      if (inf == null) { try { inf = t.BaseType.GetMethod(methodName, ObjectHelper.flags, null, types, null); } catch { } }
      //Try again without methodParams
      if (inf == null) { try { inf = t.BaseType.GetMethod(methodName, ObjectHelper.flags); } catch { } }

      //Throw exception if it is still null
      if (inf == null) { throw new Exception("Cannot find method named: " + methodName); }
      try
      {
        var ret = inf.Invoke(obj, methodParams);
        return (T)ret;
      }
      catch (Exception e) { throw e; }
    }

    /// <summary>
    /// Calls the indicated method, and does not return a value.
    /// This works on static and private methods.
    /// </summary>
    public static void CallMember(object obj, string methodName, params object[] methodParams)
    {
      //Object Type
      Type t = obj.GetType();
      //Type array of the parameters for overload matching
      Type[] types = new Type[0];
      if (methodParams != null && methodParams.Length > 0) { types = methodParams.ToList().Select(i => i.GetType()).ToArray(); }

      //Find the method.
      MethodInfo inf = null;
      //Try to find the method
      if (inf == null) { try { inf = t.GetMethod(methodName, ObjectHelper.flags, null, types, null); } catch { } }
      //Try again without methodParams
      if (inf == null) { try { inf = t.GetMethod(methodName, ObjectHelper.flags); } catch { } }

      //Try the base?
      if (inf == null) { try { inf = t.BaseType.GetMethod(methodName, ObjectHelper.flags, null, types, null); } catch { } }
      //Try again without methodParams
      if (inf == null) { try { inf = t.BaseType.GetMethod(methodName, ObjectHelper.flags); } catch { } }

      //Throw exception if it is still null
      if (inf == null) { throw new Exception("Cannot find method named: " + methodName); }
      try
      {
        inf.Invoke(obj, methodParams);
      }
      catch (Exception e) { throw e; }
    }

    /// <summary>
    /// Calls the indicated static method on the given type, and does not return a value.
    /// This works on static and private methods.
    /// </summary>
    public static T CallMember<T>(Type t, string methodName, params object[] methodParams)
    {
      //Type array of the parameters for overload matching
      Type[] types = new Type[0];
      if (methodParams != null && methodParams.Length > 0) { types = methodParams.ToList().Select(i => i.GetType()).ToArray(); }
      //Find the method.
      MethodInfo inf = null;
      //Try to find the method.
      inf = t.GetMethod(methodName, ObjectHelper.flags, null, types, null);
      //Try again without methodParams
      if (inf == null) { inf = t.GetMethod(methodName, ObjectHelper.flags); }
      //Throw exception if it is still null
      if (inf == null) { throw new Exception("Cannot find method named: " + methodName); }
      try
      {
        return (T)inf.Invoke(null, methodParams);
      }
      catch (Exception e) { throw e; }
    }

    /// <summary>
    /// Calls the indicated static method on the given type, and returns a value of type T.
    /// This works on static and private methods.
    /// </summary>
    public static void CallMember(Type t, string methodName, params object[] methodParams)
    {
      //Type array of the parameters for overload matching
      Type[] types = new Type[0];
      if (methodParams != null && methodParams.Length > 0) { types = methodParams.ToList().Select(i => i.GetType()).ToArray(); }
      //Find the method.
      MethodInfo inf = null;
      //Try to find the method.
      inf = t.GetMethod(methodName, ObjectHelper.flags, null, types, null);
      //Try again without methodParams
      if (inf == null) { inf = t.GetMethod(methodName, ObjectHelper.flags); }
      //Throw exception if it is still null
      if (inf == null) { throw new Exception("Cannot find method named: " + methodName); }
      try
      {
        inf.Invoke(null, methodParams);
      }
      catch (Exception e) { throw e; }
    }

    #endregion

    #region Construct

    public static object Construct(Type objType, Type[] paramTypes, params object[] paramValues)
    {
      if (paramTypes == null) { paramTypes = new Type[0]; }

      ConstructorInfo ci = objType.GetConstructor(CONSTRUCTOR_FLAGS, null, paramTypes, null);
      return ci.Invoke(paramValues);
    }

    public static T Construct<T>(Type[] paramTypes, params object[] paramValues)
    {
      Type t = typeof(T);
      ConstructorInfo ci = t.GetConstructor(CONSTRUCTOR_FLAGS, null, paramTypes, null);
      return (T)ci.Invoke(paramValues);
    }

    public static T Construct<T>(List<Type> paramTypes, List<object> paramValues)
    {
      return ObjectHelper.Construct<T>(paramTypes.ToArray(), paramValues.ToArray());
    }

    public static T Construct<T>(params object[] paramValues)
    {
      var paramTypes = new Type[paramValues.Length];

      for (int i = 0; i < paramValues.Length; i++)
      {
        paramTypes[i] = paramValues[i].GetType();
      }

      return ObjectHelper.Construct<T>(paramTypes, paramValues);
    }

    #endregion

    #region Attributes

    //Constants for AtributKeys
    private const string KEY_CLASS_LEVEL = "(Type Attribute)";
    private const string KEY_FIELD_LEVEL = "(Field Attribute)";
    private const string KEY_PROPERTY_LEVEL = "(Property Attribute)";
    private const string KEY_METHOD_LEVEL = "(Method Attribute)";
    private const string KEY_EVENT_LEVEL = "(Event Attribute)";
    private const string KEY_MEMBER_LEVEL = "(Member Attribute)";

    //The attribute cache
    private static Dictionary<string, List<Attribute>> attributes = new Dictionary<string, List<Attribute>>();

    private static bool attributeCaching = false;
    /// <summary>
    /// Turns caching on or off
    /// </summary>
    public static bool AttributeCaching { get { return ObjectHelper.attributeCaching; } set { ObjectHelper.attributeCaching = value; if (!value) { ObjectHelper.FlushAttributeCache(); } } }

    /// <summary>
    /// Flushes the Attribute collection container
    /// </summary>
    public static void FlushAttributeCache() { ObjectHelper.attributes = new Dictionary<string, List<Attribute>>(); }

    /// <summary>
    /// Loads the attributes for the type into the cache.
    /// </summary>
    public static void LoadAttributes(Type t)
    {
      ObjectHelper.AllClassLevelAttributes(t);
      ObjectHelper.AllEventLevelAttributes(t);
      ObjectHelper.AllFieldLevelAttributes(t);
      ObjectHelper.AllMemberLevelAttributes(t);
      ObjectHelper.AllMethodLevelAttributes(t);
      ObjectHelper.AllPropertyLevelAttributes(t);
    }

    /// <summary>
    /// Loads the attributes for the type into the cache.
    /// </summary>
    public static void LoadAttributes(object t)
    {
      ObjectHelper.LoadAttributes(t.GetType());
    }


    private static List<Attribute> GetAllTypeAttributes(Type type)
    {
      var key = ObjectHelper.GetAttributeListKey(type);
      List<Attribute> ret = null;

      //Fetch
      ret = ObjectHelper.FetchFromCache(key);
      //If we have something, return it.
      if (ret != null) { return ret; }
      //Get the attributes
      ret = type.GetCustomAttributes(true).OfType<Attribute>().ToList();
      //Cache if cache is active
      ObjectHelper.AddToCache(key, ret);
      return ret;
    }

    private static List<Attribute> GetAllFieldAttributes(Type type)
    {
      List<Attribute> ret = null;

      //Fetch
      ret = ObjectHelper.FetchFromCache(type.FullName, ObjectHelper.KEY_FIELD_LEVEL);
      //If we have something, return it.
      if (ret != null) { return ret; }
      //Get the attributes
      ret = new List<Attribute>();
      BindingFlags bf = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
      var fields = type.GetFields(bf).ToList();
      foreach (var field in fields)
      {
        var fullKey = ObjectHelper.GetAttributeListKey(field);
        var temp = field.GetCustomAttributes(true).OfType<Attribute>().ToList();
        ret.AddRange(temp);
        ObjectHelper.AddToCache(fullKey, temp);
      }
      return ret;
    }

    private static List<Attribute> GetAllPropertyAttributes(Type type)
    {
      List<Attribute> ret = null;

      //Fetch
      ret = ObjectHelper.FetchFromCache(type.FullName, ObjectHelper.KEY_PROPERTY_LEVEL);
      //If we have something, return it.
      if (ret != null) { return ret; }
      //Get the attributes
      ret = new List<Attribute>();
      BindingFlags bf = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
      var props = type.GetProperties(bf).ToList();
      foreach (var prop in props)
      {
        var fullKey = ObjectHelper.GetAttributeListKey(prop);
        var temp = prop.GetCustomAttributes(true).OfType<Attribute>().ToList();
        ret.AddRange(temp);
        ObjectHelper.AddToCache(fullKey, temp);
      }
      return ret;
    }

    private static List<Attribute> GetAllMethodAttributes(Type type)
    {
      List<Attribute> ret = null;

      //Fetch
      ret = ObjectHelper.FetchFromCache(type.FullName, ObjectHelper.KEY_METHOD_LEVEL);
      //If we have something, return it.
      if (ret != null) { return ret; }
      //Get the attributes
      ret = new List<Attribute>();
      BindingFlags bf = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
      var methods = type.GetMethods(bf).ToList();
      foreach (var method in methods)
      {
        var fullKey = ObjectHelper.GetAttributeListKey(method);
        var temp = method.GetCustomAttributes(true).OfType<Attribute>().ToList();
        ret.AddRange(temp);
        ObjectHelper.AddToCache(fullKey, temp);
      }
      return ret;
    }

    private static List<Attribute> GetAllEventAttributes(Type type)
    {
      List<Attribute> ret = null;

      //Fetch
      ret = ObjectHelper.FetchFromCache(type.FullName, ObjectHelper.KEY_EVENT_LEVEL);
      //If we have something, return it.
      if (ret != null) { return ret; }
      //Get the attributes
      ret = new List<Attribute>();
      BindingFlags bf = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
      var events = type.GetEvents(bf).ToList();
      foreach (var evt in events)
      {
        var fullKey = ObjectHelper.GetAttributeListKey(evt);
        var temp = evt.GetCustomAttributes(true).OfType<Attribute>().ToList();
        ret.AddRange(temp);
        ObjectHelper.AddToCache(fullKey, temp);
      }
      return ret;
    }

    private static List<Attribute> GetAllMemberAttributes(Type type)
    {
      List<Attribute> ret = null;

      //Fetch
      ret = ObjectHelper.FetchFromCache(type.FullName, ObjectHelper.KEY_MEMBER_LEVEL);
      //If we have something, return it.
      if (ret != null) { return ret; }
      //Get the attributes
      ret = new List<Attribute>();
      BindingFlags bf = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
      var members = type.GetMembers(bf).ToList();
      foreach (var member in members)
      {
        var fullKey = ObjectHelper.GetAttributeListKey(member);
        var temp = member.GetCustomAttributes(true).OfType<Attribute>().ToList();
        ret.AddRange(temp);
        ObjectHelper.AddToCache(fullKey, temp);
      }
      return ret;
    }

    public static string GetAttributeListKey(Type type) { return type.FullName + ObjectHelper.KEY_CLASS_LEVEL; }
    public static string GetAttributeListKey(FieldInfo info) { return info.DeclaringType.FullName + info.Name + ObjectHelper.KEY_FIELD_LEVEL; }
    public static string GetAttributeListKey(PropertyInfo info) { return info.DeclaringType.FullName + info.Name + ObjectHelper.KEY_PROPERTY_LEVEL; }
    public static string GetAttributeListKey(MethodInfo info) { return info.DeclaringType.FullName + info.Name + ObjectHelper.KEY_METHOD_LEVEL; }
    public static string GetAttributeListKey(EventInfo info) { return info.DeclaringType.FullName + info.Name + ObjectHelper.KEY_EVENT_LEVEL; }
    public static string GetAttributeListKey(MemberInfo info) { return info.DeclaringType.FullName + info.Name + ObjectHelper.KEY_MEMBER_LEVEL; }

    public static string GetAttributeListKey(Enum value)
    {
      try
      {
        var type = value.GetType();
        BindingFlags bf = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        var fields = type.GetFields(bf).ToList();
        return ObjectHelper.GetAttributeListKey(fields.FirstOrDefault(i => i.Name == value.ToString()));
      }
      catch { return ""; }
    }

    public static List<Attribute> GetAttributes(string fullKey) { return ObjectHelper.FetchFromCache(fullKey); }

    public static List<Attribute> AllClassLevelAttributes(Type type) { return ObjectHelper.GetAllTypeAttributes(type); }
    public static List<Attribute> AllClassLevelAttributes(object instance) { return ObjectHelper.GetAllTypeAttributes(instance.GetType()); }

    public static List<Attribute> AllFieldLevelAttributes(Type type) { return ObjectHelper.GetAllFieldAttributes(type); }
    public static List<Attribute> AllFieldLevelAttributes(object instance) { return ObjectHelper.GetAllFieldAttributes(instance.GetType()); }

    public static List<Attribute> AllPropertyLevelAttributes(Type type) { return ObjectHelper.GetAllPropertyAttributes(type); }
    public static List<Attribute> AllPropertyLevelAttributes(object instance) { return ObjectHelper.GetAllPropertyAttributes(instance.GetType()); }

    public static List<Attribute> AllMethodLevelAttributes(Type type) { return ObjectHelper.GetAllMethodAttributes(type); }
    public static List<Attribute> AllMethodLevelAttributes(object instance) { return ObjectHelper.GetAllMethodAttributes(instance.GetType()); }

    public static List<Attribute> AllEventLevelAttributes(Type type) { return ObjectHelper.GetAllEventAttributes(type); }
    public static List<Attribute> AllEventLevelAttributes(object instance) { return ObjectHelper.GetAllEventAttributes(instance.GetType()); }

    public static List<Attribute> AllMemberLevelAttributes(Type type) { return ObjectHelper.GetAllMemberAttributes(type); }
    public static List<Attribute> AllMemberLevelAttributes(object instance) { return ObjectHelper.GetAllMemberAttributes(instance.GetType()); }

    private static void AddToCache(string fullKey, List<Attribute> atts)
    {
      if (!ObjectHelper.AttributeCaching) { return; }
      if (atts == null) { return; }  //Never add a null entry
      //Thread Safety
      lock (ObjectHelper.attributes)
      {
        if (ObjectHelper.attributes.ContainsKey(fullKey)) { ObjectHelper.attributes[fullKey] = atts; }
        else { ObjectHelper.attributes.Add(fullKey, atts); }
      }
    }

    /// <summary>
    /// Return null for caching off, or key not present.  Return list otherwise.  May be empty.
    /// </summary>
    private static List<Attribute> FetchFromCache(string keyFull)
    {
      if (!ObjectHelper.AttributeCaching) { return null; }
      //Thread Safety
      lock (ObjectHelper.attributes)
      {
        if (ObjectHelper.attributes.ContainsKey(keyFull)) { return ObjectHelper.attributes[keyFull]; }
      }
      return null;
    }

    /// <summary>
    /// Return null for caching off, or key not present.  Return list otherwise.  May be empty.
    /// </summary>
    private static List<Attribute> FetchFromCache(string keyPart, string levelKey)
    {
      if (!ObjectHelper.AttributeCaching) { return null; }

      List<Attribute> ret = new List<Attribute>();
      //Thread Safety
      lock (ObjectHelper.attributes)
      {
        if (ObjectHelper.attributes.Count(i => i.Key.StartsWith(keyPart) && i.Key.EndsWith(levelKey) && i.Value != null) == 0) { return null; }
        var temp = ObjectHelper.attributes.Where(i => i.Key.StartsWith(keyPart) && i.Key.EndsWith(levelKey) && i.Value != null).Select(i => i.Value).ToList();
        temp.ForEach(i => ret.AddRange(i));
        return ret;
      }
    }


    #endregion
  }
}
