using System.Data.Common;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.EF
{
    public static class ProcedureExtention
    {
        private static IProcedure _defaultImplementation = new DefaultProcedure();
        public static IProcedure Implementation = _defaultImplementation;
        public static void RevertToDefaultImplementation()
        {
            Implementation = _defaultImplementation;
        }

        public static DbCommand LoadStoredProc(this DbContext context, string storedProcName)
        {
            return Implementation.LoadStoredProc(context, storedProcName);
        }

        public static DbCommand WithSqlParam(this DbCommand command, string paramName, object paramValue)
        {
            return Implementation.WithSqlParam(command, paramName, paramValue);
        }

        public static Task<List<T>> ExecuteStoredProcedureAssync<T>(this DbCommand command)
        {
            return Implementation.ExecuteStoredProcedureAssync<T>(command);
        }

        private static List<T> MapToList<T>(this DbDataReader dataReader)
        {
            var objList = new List<T>();
            var props = typeof(T).GetRuntimeProperties();

            var colMapping = dataReader.GetColumnSchema()
              .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
              .ToDictionary(key => key.ColumnName.ToLower());

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (var prop in props)
                    {
                        var value = dataReader.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
                        prop.SetValue(obj, value == DBNull.Value ? null : value);
                    }
                    objList.Add(obj);
                }
            }
            return objList;
        }

        private class DefaultProcedure : IProcedure
        {
            public DbCommand LoadStoredProc(DbContext context, string storedProcName)
            {
                var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = storedProcName;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                return command;
            }

            public DbCommand WithSqlParam(DbCommand command, string paramName, object paramValue)
            {
                if (string.IsNullOrEmpty(command.CommandText))
                {
                    throw new InvalidOperationException("Call LoadStoredProc before using this method");
                }

                var param = command.CreateParameter();
                param.ParameterName = paramName;
                param.Value = paramValue;
                command.Parameters.Add(param);
                return command;
            }

            public async Task<List<T>> ExecuteStoredProcedureAssync<T>(DbCommand command)
            {
                using (command)
                {
                    if (command.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        command.Connection.Open();
                    }
                    try
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            return reader.MapToList<T>();
                        }
                    }
                    catch (Exception e)
                    {
                        throw (e);
                    }
                    finally
                    {
                        command.Connection.Close();
                    }
                }
            }
        }
    }

    public interface IProcedure
    {
        DbCommand LoadStoredProc(DbContext context, string storedProcName);

        DbCommand WithSqlParam(DbCommand command, string paramName, object paramValue);

        Task<List<T>> ExecuteStoredProcedureAssync<T>(DbCommand command);
    }
}