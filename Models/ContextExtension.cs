// Archivo: DbFlujosTestContext.Custom.cs
using GestionSolicitud.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GestionSolicitud.Models
{
    public partial class DbFlujosTestContext
    {
        public virtual DbSet<SpListadoSoliciturdes> ListadoSoliciturdesSp { get; set; }
        public virtual DbSet<SpListadoSoliciturdesAll> ListadoSoliciturdesAllSp { get; set; }

        public virtual DbSet<SpListadoSoliciturdesSAP> ListadoSoliciturdesSpSAP { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpListadoSoliciturdes>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });
            
            modelBuilder.Entity<SpBuscarAutocompletado>(entity =>
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

        public List<SpListadoSoliciturdesAll> spListadoSoliciturdesAll(int idUsuario, int idRol)
        {
            return ListadoSoliciturdesAllSp
                .FromSqlRaw("EXEC sp_ListadoSoliciturdesAll @idUsuario, @Rol",
                    new SqlParameter("@idUsuario", idUsuario),
                    new SqlParameter("@Rol", idRol))
                .AsNoTracking()
                .ToList();
        }

        public List<SpListadoSoliciturdesSAP> spListadoSoliciturdesSAP(int idUsuario, int idRol)
        {
            return ListadoSoliciturdesSpSAP
                .FromSqlRaw("EXEC sp_ListadoSoliciturdesSAP @idUsuario, @Rol",
                    new SqlParameter("@idUsuario", idUsuario),
                    new SqlParameter("@Rol", idRol))
                .AsNoTracking()
                .ToList();
        }

    }
}