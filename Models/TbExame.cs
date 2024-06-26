﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App_aula_1.Models;

[Table("tbExame")]
public partial class TbExame
{
    [Key]
    public int IdExame { get; set; }

    public int Grupo { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string Descricao { get; set; }

    [InverseProperty("IdExameNavigation")]
    public virtual ICollection<TbExameXPacientes> TbExameXPacientes { get; set; } = new List<TbExameXPacientes>();
}