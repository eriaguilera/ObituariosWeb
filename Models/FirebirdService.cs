using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace ObituariosWeb
{
    public class FirebirdService
    {
        private readonly IConfiguration _configuration;

        public FirebirdService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DataTable ObtenerHomenajes()
        {
            var connectionString = _configuration.GetConnectionString("FirebirdConnection");

            using var connection = new FbConnection(connectionString);
            connection.Open();

            var query = @"
            SELECT FIRST 100
                ss.COD_SERVICIO_SEPELIO,
                suc.NOMBRE AS ESTABLECIMIENTO,
                sal.NOMBRE AS SALA,
                ext.APELLIDO_NOMBRE AS FALLECIDO,
                ext.NRO_DOCUMENTO AS DNI,
                ext.FECHA_FALLECIMIENTO,
                ss.FECHA_HORA_ARRIBO AS INICIO_SERVICIO
            FROM SERVICIO_SEPELIO ss
            LEFT JOIN SUCURSALES suc ON ss.COD_SUCURSAL = suc.COD_SUCURSAL
            LEFT JOIN SALAS_SEPELIO sal ON ss.COD_SALA = sal.COD_SALA
            LEFT JOIN SERVICIOS_SEPELIOS_EXTINTOS rel ON ss.COD_SERVICIO_SEPELIO = rel.COD_SERVICIO_SEPELIO
            LEFT JOIN EXTINTOS ext ON rel.COD_EXTINTO = ext.COD_EXTINTO
            ORDER BY ss.FECHA_HORA_ARRIBO DESC
            ";

            using var command = new FbCommand(query, connection);
            using var adapter = new FbDataAdapter(command);

            var table = new DataTable();
            adapter.Fill(table);

            return table;
        }
    }
}