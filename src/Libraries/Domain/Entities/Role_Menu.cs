namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class Role_Menu : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// ��Ȩʱ��
        /// </summary>
        public DateTime? CreateTime { get; set; }

        #region Relationships

        public int RoleInfoId { get; set; }
        [ForeignKey("RoleInfoId")]
        public virtual RoleInfo RoleInfo { get; set; }

        public int Sys_MenuId { get; set; }
        [ForeignKey("Sys_MenuId")]
        public virtual Sys_Menu Sys_Menu { get; set; }

        #endregion
    }
}
