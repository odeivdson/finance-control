using Infrastructure.Services;
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Web.ViewModels;
using Application.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configura��o do EF Core com SQLite
builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseSqlite("Data Source=finance.db"));

// Inje��o dos reposit�rios
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IParcelaRepository, ParcelaRepository>();
builder.Services.AddScoped<IMetaRepository, MetaRepository>();

// Inje��o dos servi�os
builder.Services.AddScoped<ITransacaoService, TransacaoService>();

// Adiciona logging
builder.Services.AddLogging();

// Adiciona AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// Middleware global para tratamento de exce��es n�o tratadas
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro n�o tratado na aplica��o");
        context.Response.Redirect("/Error?message=" + Uri.EscapeDataString("Ocorreu um erro inesperado. Tente novamente mais tarde."));
    }
});

app.Run();

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TransacaoDto, TransacaoViewModel>().ReverseMap();
        CreateMap<TransacaoFiltroViewModel, TransacaoFiltroDto>().ReverseMap();
        CreateMap<MetaDto, MetaViewModel>().ReverseMap();
        CreateMap<ParcelaDto, ParcelaViewModel>().ReverseMap();
        CreateMap<CategoriaDto, CategoriaViewModel>().ReverseMap();
    }
}
