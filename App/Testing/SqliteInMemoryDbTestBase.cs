﻿using BlazorHomeSite.Services.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Testing
{
    public abstract class SqliteInMemoryDbTestBase : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<DatabaseService> _contextOptions;
        private bool disposedValue;

        protected SqliteInMemoryDbTestBase()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<DatabaseService>()
            .UseSqlite(_connection)
            .Options;

            using var homeSiteDbContext = new DatabaseService(_contextOptions);

            if (!homeSiteDbContext.Database.EnsureCreated())
            {
                throw new SystemException("Cannot create db context");
            }
        }

        ~SqliteInMemoryDbTestBase()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _connection.Dispose();
                }

                disposedValue = true;
            }
        }

        protected DatabaseService GetContext() => new(_contextOptions);
    }
}