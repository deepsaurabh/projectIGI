using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
   public class TableColumnNameHelper
    {
       public static string GetTableName(Type type)
       {
           return "tbl" + type.Name;
       }

       public static string GetCoulmnNameFromProperty<T>(Type type, Expression<Func<T>> exp)
       {
           var property = GetMemeberName(exp);
           if (!property.Equals("Id"))
           {
               return property;
           }

           var propertyInfo = type.GetProperty(property);
           return GetColumnNamePrependedByEntityName(propertyInfo);
       }

       #region InternalHelerMethods

       public static string GetColumnNamePrependedByEntityName(PropertyInfo propertyIInfo)
       {
           return propertyIInfo.ReflectedType.Name + propertyIInfo.Name;
       }


       public static string GetMemeberName<T>(Expression<Func<T>> exp)
       {
           var expressionBody = (MemberExpression)exp.Body;
          return expressionBody.Member.Name;
       }

       #endregion
    }
}
