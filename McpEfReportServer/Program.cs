using McpEfReportServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<SchemaToolService>();
builder.Services.AddScoped<DynamicQueryTool>();
builder.Services.AddScoped<AdaptiveAgentService>();
builder.Services.AddScoped<SqlGuardService>();
builder.Services.AddScoped<DynamicQueryTool>();
builder.Services.AddScoped<ExcelExportService>();
builder.Services.AddScoped<PdfExportService>();
builder.Services.AddScoped<AdaptiveAgentService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Agent}/{action=Index}/{id?}");

app.Run();