using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sales.Data;
using Sales.Data.Models;
using Sales.Models.Cart;
using Sales.Options;
using Sales.Services;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.Configure<MailOptions>(builder.Configuration.GetSection("Mail"));

builder.Services.AddScoped<IEmailSender, EmailSenderService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opt =>
{
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(sp => CartService.GetCart(sp));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

using var scope = scopeFactory.CreateAsyncScope();

await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<ApplicationUser>>();
var userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;
await context.Database.MigrateAsync();

async Task CreateNewUserAsync(string email, string password, string name, string? address, string? phoneNumber, string role)
{
    var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    using var scope = scopeFactory.CreateAsyncScope();

    await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<ApplicationUser>>();
    var userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;

    var user = new ApplicationUser
    {
        EmailConfirmed = true,
        Name = name,
        PhoneNumber = phoneNumber
    };
    await userStore.SetUserNameAsync(user, email, CancellationToken.None);
    await userEmailStore.SetEmailAsync(user, email, CancellationToken.None);
    var result = await userManager.CreateAsync(user, password);

    

    if (result.Succeeded)
    {
        user = await userManager.FindByNameAsync(email);
        if (user != null)
        {
            await userManager.AddToRoleAsync(user, role);

            if (address != null)
            {
                context.UserAddresses.Add(new UserAddress
                {
                    UserId = user.Id,
                    Address = address
                });

                await context.SaveChangesAsync();
            }
        }
    }
}

async Task CreateNewOrderAsync(int customerId, DateTime date, int employeeId)
{
    var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    using var scope = scopeFactory.CreateAsyncScope();

    await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<ApplicationUser>>();
    var userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;

    var customer = await context.Users.Include(u => u.UserAddresses).FirstOrDefaultAsync(o => o.Id == customerId);


    var order = new Order
    {
        CustomerId = customerId,
        CreatedDate = date,
        EmployeeId = employeeId,
        Address = customer!.UserAddresses.FirstOrDefault()!.Address
    };

    await context.Orders.AddAsync(order);
    await context.SaveChangesAsync();
}

async Task CreateNewOrderedProductsAsync(int orderId, int productId, int quntity)
{
    var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    using var scope = scopeFactory.CreateAsyncScope();

    await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<ApplicationUser>>();
    var userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;


    var orderedProduct = new OrderedProduct
    {
        OrderId = orderId,
        ProductId = productId,
        Quantity = quntity
    };

    await context.OrderedProducts.AddAsync(orderedProduct);
    await context.SaveChangesAsync();
}



// Добавляем заказы
if (!context.Orders.Any())
{
}

// Добавляем товары к заказу
if (!context.OrderedProducts.Any())
{

}

// Создаем админа
var user = await userManager.FindByNameAsync("admin@mail.com");

if (user == null)
{
    user = new ApplicationUser();
    user.EmailConfirmed = true;
    await userStore.SetUserNameAsync(user, "admin@mail.com", CancellationToken.None);
    await userEmailStore.SetEmailAsync(user, "admin@mail.com", CancellationToken.None);
    var result = await userManager.CreateAsync(user, "c8XBx%");

    if (result.Succeeded)
    {
        user = await userManager.FindByNameAsync("admin@mail.com");
        if (user != null)
        {
            await userManager.AddToRoleAsync(user, "Administrator");
        }
    }
}
else
{
    if (!await userManager.IsInRoleAsync(user, "Administrator"))
    {
        await userManager.AddToRoleAsync(user, "Administrator");
    }
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
