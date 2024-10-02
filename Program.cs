
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
	options.AddPolicy("UploadPermissions", builder =>
	{
		builder.WithOrigins(["http://localhost:4200", "http://localhost:5270/"]);
	});
});
var app = builder.Build();
app.UseCors("UploadPermissions");

app.MapPost("/upload", async (HttpContext context) =>
{
	var arquivos = context.Request.Form.Files;

	foreach (var arquivo in arquivos)
	{
		if (arquivo.Length > 0)
		{
			try
			{
				var caminhoArquivo = Path.Combine("uploads", arquivo.FileName);
				using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
				{
					await arquivo.CopyToAsync(stream);
				}

			}
			catch (Exception ex)
			{
				return Results.BadRequest(new { message = ex.Message });
			}

		}
	}
	return Results.Ok(new { totalArquivos = arquivos.Count, tamanhoBytes = arquivos.Sum(arquivo => arquivo.Length) });
});

app.Run();
