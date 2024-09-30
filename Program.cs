
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


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
