using FirebirdSql.Data.FirebirdClient;
using System.Data;

public class FirebirdService
{
    private readonly string _connectionString;

    public FirebirdService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("FirebirdConnection");
    }
  
public DataTable ObtenerExtintos(string apellido, int page, int pageSize)
{
    using var connection = new FbConnection(_connectionString);
    connection.Open();

    int skip = (page - 1) * pageSize;

    string query = $@"
    SELECT FIRST {pageSize} SKIP {skip}
    APELLIDO_EXTINTO,
    NOMBRE_EXTINTO,
    FECHA_NACIMIENTO,
    NRO_DOCUMENTO,
    SEXO,
    FECHA_FALLECIMIENTO,
    EDAD,
    COMENTARIO_FALLECIMIENTO,
    EXPEDIDO_POR
    FROM EXTINTOS";

    if (!string.IsNullOrEmpty(apellido))
    {
        query += " WHERE APELLIDO_EXTINTO CONTAINING @apellido";
    }

    query += " ORDER BY FECHA_FALLECIMIENTO DESC";

    using var command = new FbCommand(query, connection);

    if (!string.IsNullOrEmpty(apellido))
    {
        command.Parameters.AddWithValue("@apellido", apellido);
    }

    using var adapter = new FbDataAdapter(command);
    var table = new DataTable();
    adapter.Fill(table);

    return table;
}
    public DataTable ObtenerHomenajes()
{
    using var connection = new FbConnection(_connectionString);
    connection.Open();

    string query = @"
SELECT FIRST 10
    SS.COD_SERVICIO_SEPELIO,
    SU.NOMBRE AS ESTABLECIMIENTO,
    SA.NOMBRE AS SALA,
    EX.APELLIDO_NOMBRE AS FALLECIDO,
    EX.NRO_DOCUMENTO AS DNI,
    EX.FECHA_FALLECIMIENTO,
    SS.FECHA_HORA_ARRIBO AS INICIO_SERVICIO
FROM SERVICIO_SEPELIO SS
INNER JOIN SERVICIOS_SEPELIOS_EXTINTOS SSE 
    ON SSE.COD_SERVICIO_SEPELIO = SS.COD_SERVICIO_SEPELIO
INNER JOIN EXTINTOS EX 
    ON EX.COD_EXTINTO = SSE.COD_EXTINTO
LEFT JOIN SUCURSALES SU 
    ON SU.COD_SUCURSAL = SS.COD_SUCURSAL
LEFT JOIN SALAS_SEPELIO SA 
    ON SA.COD_SALA = SS.COD_SALA
ORDER BY SS.FECHA_HORA_ARRIBO DESC
";  
    using var command = new FbCommand(query, connection);
    using var adapter = new FbDataAdapter(command);

    var table = new DataTable();
    adapter.Fill(table);

    return table;
}

}