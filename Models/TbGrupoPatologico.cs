﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App_aula_1.Models;

[Table("tbGrupoPatologico")]
public partial class TbGrupoPatologico
{
    [Key]
    public int IdGrupoPatologico { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; }

    [InverseProperty("IdGrupoPatologicoNavigation")]
    public virtual ICollection<TbGrupoPatologicoXPatologia> TbGrupoPatologicoXPatologia { get; set; } = new List<TbGrupoPatologicoXPatologia>();
}