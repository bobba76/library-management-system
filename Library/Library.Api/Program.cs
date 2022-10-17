using Library.Api;
using Library.Api.Mapping;
using Library.Application;
using Library.Infrastructure;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var assemblies = new[] {typeof(IAmApi).Assembly, typeof(IAmApplication).Assembly};

builder.Services.AddMediatR(assemblies);
builder.Services.AddAutoMapper(assemblies);
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors();

// Add local services
builder.Services.AddSingleton(MappingProfile.Mapper.GetMap());

// Add infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// global cors policy
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.UseAuthorization();

app.MapControllers();

app.Run();