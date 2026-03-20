using Microsoft.AspNetCore.Mvc;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace ObituariosWeb.Controllers
{
    public class HomeController : Controller
    {
        // Cadena de conexión real
        private string _connString = @"User=SYSDBA;Password=masterkey;Database=C:\BasesFirebird\serrafun_f3.fdb;DataSource=localhost;Port=3050;Charset=NONE;";

        // INDEX: para mostrar homenajes
        public IActionResult Index()
        {
            DataTable dt = new DataTable();

            try
            {
                using (FbConnection con = new FbConnection(_connString))
                {
                    con.Open();

                    string query = @"SELECT NOMBRE_EXTINTO, APELLIDO_EXTINTO, FECHA_FALLECIMIENTO, EDAD, COMENTARIO_FALLECIMIENTO
                                     FROM EXTINTOS
                                     ORDER BY FECHA_FALLECIMIENTO DESC";

                    using (FbCommand cmd = new FbCommand(query, con))
                    using (FbDataAdapter da = new FbDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "⚠ Error de conexión: " + ex.Message;
            }

            return View(dt); // Muestra Index.cshtml
        }

        // PRIVACY: tabla con datos y búsqueda
        public IActionResult Privacy(string apellido)
        {
            DataTable dt = new DataTable();

            try
            {
                using (FbConnection con = new FbConnection(_connString))
                {
                    con.Open();

                    string query = @"SELECT NOMBRE_EXTINTO, APELLIDO_EXTINTO, FECHA_FALLECIMIENTO, EDAD, COMENTARIO_FALLECIMIENTO
                                     FROM EXTINTOS";

                    if (!string.IsNullOrEmpty(apellido))
                    {
                        query += " WHERE APELLIDO_EXTINTO LIKE @apellido";
                    }

                    using (FbCommand cmd = new FbCommand(query, con))
                    {
                        if (!string.IsNullOrEmpty(apellido))
                            cmd.Parameters.AddWithValue("@apellido", "%" + apellido + "%");

                        using (FbDataAdapter da = new FbDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "⚠ Error de conexión: " + ex.Message;
            }

            return View(dt); // Muestra Privacy.cshtml
        }
    }
}