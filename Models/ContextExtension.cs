// Archivo: DbFlujosTestContext.Custom.cs
using GestionSolicitud.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GestionSolicitud.Models
{
    public partial class DbFlujosTestContext
    {
        public virtual DbSet<SpListadoSoliciturdes> ListadoSoliciturdesSp { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpListadoSoliciturdes>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });
        }

        public List<SpListadoSoliciturdes> spListadoSoliciturdes(int idUsuario, int idRol)
        {
            return ListadoSoliciturdesSp
                .FromSqlRaw("EXEC sp_ListadoSoliciturdes @idUsuario, @Rol",
                    new SqlParameter("@idUsuario", idUsuario),
                    new SqlParameter("@Rol", idRol))
                .AsNoTracking()
                .ToList();
        }
    }
}