using lost_on_island.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add session services to the DI container
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // or whatever you want the timeout to be
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add our custom services
builder.Services.AddSingleton<ILocationProvider, LocationProvider>();
builder.Services.AddScoped(typeof(ISessionStorage<>), typeof(SessionStorage<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); 
app.UseAuthorization();

app.MapRazorPages();

app.Run();
