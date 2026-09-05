using System.ComponentModel.DataAnnotations;

namespace ClasesTallerMecanico.Dtos;

/// <summary>
/// Minimal API no ejecuta los DataAnnotations ([Required], [Range], [StringLength], etc.)
/// automáticamente como sí lo hace MVC. Esta extensión los fuerza a mano en cada
/// endpoint POST/PUT, devolviendo un 400 con el detalle de cada campo inválido
/// en vez de dejar pasar cualquier cosa hacia la base de datos.
/// </summary>
public static class ValidationExtensions
{
    public static IResult? Validar(this object dto)
    {
        var contexto = new ValidationContext(dto);
        var resultados = new List<ValidationResult>();
        var esValido = Validator.TryValidateObject(dto, contexto, resultados, validateAllProperties: true);

        if (esValido)
            return null;

        var errores = resultados
            .SelectMany(resultado =>
                (resultado.MemberNames.Any() ? resultado.MemberNames : new[] { string.Empty })
                    .Select(miembro => new { Miembro = miembro, resultado.ErrorMessage }))
            .GroupBy(x => x.Miembro)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Select(x => x.ErrorMessage ?? "Valor inválido.").ToArray());

        return Results.ValidationProblem(errores);
    }
}
