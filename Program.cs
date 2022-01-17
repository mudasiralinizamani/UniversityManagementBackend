var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database Context
builder.Services.AddDbContext<AuthContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDbContext<ApiContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


// Dependency Injection
builder.Services.AddScoped<IFaculty, IFacultyService>();
builder.Services.AddScoped<INotification, INotificationService>();
builder.Services.AddScoped<IDepartment, IDepartmentService>();
builder.Services.AddScoped<ISubject, ISubjectService>();

// Identity Configration
builder.Services.AddIdentityCore<UserModel>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AuthContext>();

builder.Services.Configure<IdentityOptions>(opt =>
{
  opt.Password.RequireDigit = false;
  opt.Password.RequireLowercase = false;
  opt.Password.RequireUppercase = false;
  opt.Password.RequireNonAlphanumeric = false;
  opt.User.RequireUniqueEmail = true;
}
);

builder.Services.AddCors();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.Run();
