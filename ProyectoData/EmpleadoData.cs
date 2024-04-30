using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using ProyectoModelo;

namespace ProyectoData
{
    public class EmpleadoData
    {
        private readonly ConnectionsStrings conexiones;

        public EmpleadoData(IOptions<ConnectionsStrings> options)
        {
            conexiones = options.Value;
        }

        public async Task<List<Empleado>> Lista()
        {
            List<Empleado> lista = new List<Empleado>();

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("SP_ListaEmpleados", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Empleado
                        {
                            IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            Sueldo = Convert.ToInt32(reader["Sueldo"]),
                            FechaContrato = reader["FechaContrato"].ToString(),
                            Departamento = new Departamento()
                            {
                                IdDepartamento = Convert.ToInt32(reader["IdEmpleado"]),
                                Nombre = reader["Nombre"].ToString() 
                            }
                        });
                    }
                }
            }

            return lista;
        }

        public async Task<bool> Crear(Empleado objeto)
        {
            bool respuesta = true;

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("SP_CrearEmpleados", conexion);
                cmd.Parameters.AddWithValue("@NombreCompleto",objeto.NombreCompleto);
                cmd.Parameters.AddWithValue("@IdDepartamento", objeto.Departamento!.IdDepartamento);
                cmd.Parameters.AddWithValue("@Sueldo", objeto.Sueldo);
                cmd.Parameters.AddWithValue("@FechaContrato", objeto.FechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conexion.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true: false;
                }
                catch (Exception)
                {

                    respuesta = false;
                }
            }

            return respuesta;
        }

        public async Task<bool> Editar(Empleado objeto)
        {
            bool respuesta = true;

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("SP_EditarEmpleados", conexion);
                cmd.Parameters.AddWithValue("@IdEmpleado", objeto.IdEmpleado);
                cmd.Parameters.AddWithValue("@NombreCompleto", objeto.NombreCompleto);
                cmd.Parameters.AddWithValue("@IdDepartamento", objeto.Departamento!.IdDepartamento);
                cmd.Parameters.AddWithValue("@Sueldo", objeto.Sueldo);
                cmd.Parameters.AddWithValue("@FechaContrato", objeto.FechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conexion.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch (Exception)
                {

                    respuesta = false;
                }
            }

            return respuesta;
        }

        public async Task<bool> Eliminar(int id)
        {
            bool respuesta = true;

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("SP_EliminarEmpleados", conexion);
                cmd.Parameters.AddWithValue("@IdEmpleado", id);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conexion.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch (Exception)
                {

                    respuesta = false;
                }
            }

            return respuesta;
        }
    }
}
