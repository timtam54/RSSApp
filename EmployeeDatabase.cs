using System;
using RssMob.Models;
using SQLite;

namespace RssMob
{

    public static class Constants
    {
        public const string DatabaseFilename = "TodoSQLite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }

    public class EmployeeDatabase
    {
        public async Task<List<Employee>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<Employee>().ToListAsync();
        }
        public async Task<List<Employee>> GetItemsNotDoneAsync()
        {
            await Init();
            return await Database.Table<Employee>().ToListAsync();

            // SQL queries are also possible
            //return await Database.QueryAsync<Employee>("SELECT * FROM [Employee] WHERE [Done] = 0");
        }

        public async Task<Employee> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<Employee>().Where(i => i.id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(Employee item,bool newrec)
        {
            try
            {
                await Init();
                //if (item.id != 0)
                if (!newrec)
                    return await Database.UpdateAsync(item);
                else
                    return await Database.InsertAsync(item);
            }
            catch (Exception ex)
            {
                var ff = ex;
                return -1;
            }
        }

        public async Task<int> DeleteItemAsync(Employee item)
        {
            await Init();
            return await Database.DeleteAsync(item);
           
        }
        SQLiteAsyncConnection Database;

        public EmployeeDatabase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;
            // SQLitePCL.raw.SetProvider(new SQLite3Provider_e_sqlite3());
           
            try
            {
                Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                var result = await Database.CreateTableAsync<Employee>();
            }
            catch (Exception ex)
            {
                var ff = ex;
                var dd = ff;
            }
        }
    }
}

