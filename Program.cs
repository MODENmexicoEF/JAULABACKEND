using FluentValidation;
using JAULABACKEND.AutoMappers;
using JAULABACKEND.DTOs;
using JAULABACKEND.Models;
using JAULABACKEND.Repository;
using JAULABACKEND.Validators;
using Microsoft.EntityFrameworkCore;
using JAULABACKEND.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



//Mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

//Conexion
builder.Services.AddDbContext<JaulaContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("JaulaBeisConnection"));
});
builder.Services.AddMemoryCache();
//Servicios
builder.Services.AddKeyedScoped<ICommonService<JugadorDto, JugadorInsertDto, JugadorUpdateDto>, JugadoresService>("jugadoresService");
builder.Services.AddKeyedScoped<ICommonService<TrabajadorDto, TrabajadorInsertDto, TrabajadorUpdateDto>, TrabajadoresService>("trabajadoresService");

builder.Services.AddKeyedScoped<IJugarService, JugarService>("JugarService");
builder.Services.AddKeyedScoped<IJugarService, Jugar2Service>("Jugar2Service");

//Repositorios
builder.Services.AddScoped<IRepository<Jugador>,JugadoresRepository>();
builder.Services.AddScoped<IRepository<Trabajador>, TrabajadoresRepository >();

//Validators
builder.Services.AddScoped<IValidator<JugadorInsertDto>, JugadorInsertValidator>();
builder.Services.AddScoped<IValidator<JugadorUpdateDto>, JugadorUpdateValidator>();
builder.Services.AddScoped<IValidator<TrabajadorInsertDto>, TrabajadorInsertValidator>();
builder.Services.AddScoped<IValidator<TrabajadorUpdateDto>, TrabajadorUpdateValidator>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
