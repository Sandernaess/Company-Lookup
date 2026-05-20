using CompanyLookup.Api.Configurations;
using CompanyLookup.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInternalServices();
builder.Services.AddExternalServices();

var app = builder.Build();

app.UseExceptionHandler(err => err.Run(async context =>
{
    context.Response.StatusCode = 500;
    await context.Response.WriteAsJsonAsync(new
    {
        error = "An unexpected error occurred."
    });
}));

app.MapCompaniesEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();