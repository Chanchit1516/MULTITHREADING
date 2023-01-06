var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//DB Postgrest
//การทำ multithread ต้อง inject db ให้เป็นแบบ ServiceLifetime.Transient

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(AESService.Decrypt(builder.Configuration.GetConnectionString("DefaultDatabase")), options => { options.CommandTimeout(300); }), ServiceLifetime.Transient);
//builder.Services.AddDbContext<CClaimDbContext>(options =>
//    options.UseSqlServer(AESService.Decrypt(builder.Configuration.GetConnectionString("CClaimDatabase"))));

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

app.Run();
