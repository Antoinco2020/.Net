using Domain.Interfaces;
using Repository;
using Service;
using soap_net8;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSoapCore();
builder.Services.AddScoped<IService, soap_net8.Service>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => {
    _ = endpoints.UseSoapEndpoint<IService>(
    path: "/Service.asmx",
    encoder: new SoapEncoderOptions(),
    serializer: SoapSerializer.XmlSerializer);
});
app.UseHttpsRedirection();
app.Run();
