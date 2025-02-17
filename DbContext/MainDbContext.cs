using System.Data;
using Microsoft.EntityFrameworkCore;

using Configuration;
using Models;
using Models.DTO;
using DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Npgsql.Replication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DbContext;

//DbContext namespace is a fundamental EFC layer of the database context and is
//used for all Database connection as well as for EFC CodeFirst migration and database updates 

public class MainDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    IConfiguration _configuration;
    DatabaseConnections _databaseConnections;

    private readonly SysAdminCred _sysCred;

    readonly Encryptions _encryptions;

   

    public string dbConnection  => _databaseConnections.GetDbConnection(this.Database.GetConnectionString());

    #region C# model of database tables
    public DbSet<AttractionDbM> Attractions  { get; set; }
    public DbSet<CommentDbM> Comments  { get; set; }

    public DbSet<FinancialDbM> Financials  { get; set; }
    public DbSet<CategoryDbM> Catgeories { get; set; }
    public DbSet<AddressDbM> Addresses { get; set; }
    public DbSet<UserDbM> Users { get; set; }    


    public DbSet<RoleDbM> Roles { get; set; }

    #endregion

    #region model the Views
    public DbSet<GstUsrInfoDbDto> InfoDbView { get; set; }
    public DbSet<GstUsrInfoAttractionsDto> InfoAttractionsView { get; set; }
    public DbSet<GstUsrInfoCategoriesDto> InfoCategoriesView { get; set; }

    

    #endregion

    #region constructors
    public MainDbContext() { }
    public MainDbContext(DbContextOptions options, IConfiguration configuration, DatabaseConnections databaseConnections, IOptions<SysAdminCred> sysCred, Encryptions encryptions) : base(options)
    { 
        _databaseConnections = databaseConnections;
        _configuration = configuration;
        _sysCred = sysCred.Value;
        _encryptions = encryptions;
    }
    #endregion

    //Here we can modify the migration building
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        #region model the Views
        modelBuilder.Entity<GstUsrInfoDbDto>().ToView("vwInfoDb", "gstusr").HasNoKey();
        modelBuilder.Entity<GstUsrInfoAttractionsDto>().ToView("vwInfoAttractions", "gstusr").HasNoKey();  
        modelBuilder.Entity<GstUsrInfoCategoriesDto>().ToView("vwInfoCategories", "gstusr").HasNoKey();    

         modelBuilder.Entity<FinancialDbM>()
            .Property(a => a.EnryptedToken).HasColumnType("nvarchar(max)");    

              modelBuilder.Entity<FinancialDbM>()
        .Property(f => f.Comments)
        .HasColumnType("nvarchar(MAX)"); 

   
        #endregion

        #region Seed the Roles

        //modelBuilder.Entity<UserDbM>

        #endregion

        #region override modelbuilder
        #endregion
        
        base.OnModelCreating(modelBuilder);
    }

    #region DbContext for some popular databases
    public class SqlServerDbContext : MainDbContext
    {
        public SqlServerDbContext() { }
        public SqlServerDbContext(DbContextOptions options, IConfiguration configuration, DatabaseConnections databaseConnections, IOptions<SysAdminCred> sysCred, Encryptions encryptions) 
            : base(options, configuration, databaseConnections, sysCred, encryptions) { }


        //Used only for CodeFirst Database Migration and database update commands
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder = SetupDatabaseConnection(optionsBuilder, 
                    (options, connectionString) => options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure()));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HaveColumnType("money");
            configurationBuilder.Properties<string>().HaveColumnType("nvarchar(200)");

            base.ConfigureConventions(configurationBuilder);
        }
    }

    public class MySqlDbContext : MainDbContext
    {
        public MySqlDbContext() { }
        public MySqlDbContext(DbContextOptions options) : base(options, null, null, null, null) { } // Reminder!!! Nullar encryptions och sysadminsecreten, lägg till de här ifall de behövs senare


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder = SetupDatabaseConnection(optionsBuilder, 
                    (options, connectionString) => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");

            base.ConfigureConventions(configurationBuilder);

        }
    }

    public class PostgresDbContext : MainDbContext
    {
        public PostgresDbContext() { }
        public PostgresDbContext(DbContextOptions options) : base(options, null, null, null, null){ }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder = SetupDatabaseConnection(optionsBuilder, 
                    (options, connectionString) => options.UseNpgsql(connectionString));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");
            base.ConfigureConventions(configurationBuilder);
        }
    }

    public class SqliteDbContext : MainDbContext
    {
        public SqliteDbContext() { }
        public SqliteDbContext(DbContextOptions options) : base(options, null, null, null, null){ }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder = SetupDatabaseConnection(optionsBuilder, 
                    (options, connectionString) => options.UseSqlite(connectionString));
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
    #endregion


    #region Methods used only during Migration
    private void DesignTimeMigrationCreateServices()
    {
        //ASP.NET Core program.cs has not run by efc design-time, configure and create services as in 
        //program.cs

        // Create a configuration
        System.Console.WriteLine($"   configuring");
        var conf = new ConfigurationBuilder();
        conf.AddApplicationSecrets("../Configuration/Configuration.csproj");

        System.Console.WriteLine($"   building configuring");
        var configuration = conf.Build();

        System.Console.WriteLine($"   creating services");
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDatabaseConnections(configuration);
        serviceCollection.AddSingleton<IConfiguration>(configuration);

        // Build the service provider and get the DbContext
        System.Console.WriteLine($"   retrieving services from serviceProvider");

        var serviceProvider = serviceCollection.BuildServiceProvider();
        _configuration = serviceProvider.GetRequiredService<IConfiguration>();
        System.Console.WriteLine($"   {nameof(IConfiguration)} retrieved");

        _databaseConnections = serviceProvider.GetRequiredService<DatabaseConnections>();
        System.Console.WriteLine($"   {nameof(DatabaseConnections)} retrieved");
        System.Console.WriteLine($"      secret source: {_databaseConnections.SetupInfo.SecretSource}");
        System.Console.WriteLine($"      database server: {_databaseConnections.SetupInfo.DataConnectionServer}");
        System.Console.WriteLine($"      database connection tag: {_databaseConnections.SetupInfo.DataConnectionTag}");

    }

    private DbConnectionDetail GetDatabaseConnection()
    {
        var connection = _databaseConnections.GetDataConnectionDetails(_configuration["DatabaseConnections:MigrationDataUser"]);
        if (connection.DbConnectionString == null)
        {
            throw new InvalidDataException($"Error: Connection string for {connection.DbConnection}, {connection.DbUserLogin} not set");
        }

        return connection;
    }

    DbContextOptionsBuilder SetupDatabaseConnection(DbContextOptionsBuilder optionsBuilder, Func<DbContextOptionsBuilder, string, DbContextOptionsBuilder> options)
    {
        System.Console.WriteLine($"{nameof(SqlServerDbContext)}: executing OnConfiguring...");
        if (_databaseConnections == null || _configuration == null)
        {
            DesignTimeMigrationCreateServices();
        }

        DbConnectionDetail connection = GetDatabaseConnection();

        optionsBuilder = options(optionsBuilder, connection.DbConnectionString);
        System.Console.WriteLine($"{nameof(SqlServerDbContext)}: OnConfiguring completed successfully");
        System.Console.WriteLine($"Proceeding with migration with user: {connection.DbUserLogin} " +
                                        $"on database connection: {connection.DbConnection}");
        return optionsBuilder;
    }
    #endregion
    
    public async Task SeedRolesAsync()
    {

        IQueryable sys = Users.Where(x => x.UserName == _sysCred.SysUserName);



        if (!Roles.Any())
        {
            Roles.AddRange(new List<RoleDbM>
            {
                new RoleDbM { RoleId = Guid.NewGuid(), Roles = enumRoles.gstusr},
                new RoleDbM { RoleId = Guid.NewGuid(), Roles = enumRoles.usr},
                new RoleDbM { RoleId = Guid.NewGuid(), Roles = enumRoles.supusr},
                new RoleDbM { RoleId = Guid.NewGuid(), Roles = enumRoles.sysadmin}
            });

            await SaveChangesAsync();
        }

        if (!Users.Any())
        {
            Users.Add(new UserDbM { UserId = Guid.NewGuid(), UserName = _sysCred.SysUserName, Password = _encryptions.EncryptPasswordToBase64(_sysCred.SysPassword), Role = enumRoles.sysadmin.ToString() });

            await SaveChangesAsync();
        }

        
    }
  

    public string GetDbStringsForRole(enumRoles userRole, string db = "jadedb.docker")
    {
    
    var connectionKey = $"SQLServer-{db.Replace(".", "-")}-{userRole.ToString().ToLower()}";

    
    var connectionString = _configuration.GetConnectionString(connectionKey);

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new KeyNotFoundException($"Connection string for key '{connectionKey}' was not found in appsettings.json.");
    }

    Console.WriteLine($"Fetched Role String: {connectionString}");

    return connectionString;
    }

}

 


