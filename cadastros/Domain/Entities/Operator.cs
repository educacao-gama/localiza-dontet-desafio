using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.Entities.Enums;
using Domain.Entities.Interfaces;

namespace Domain.Entities
{
    [Table("users")]
    public class Operator : Person, IOperator
    {   
        [Column]
		[JsonIgnore]
        [JsonPropertyName("Documento")]
        public override string Document { get; set; }

        [Column]
		[JsonIgnore]
        [JsonPropertyName("Tipo")]
        public override int Type { get; set; }

        [Column]
        [JsonIgnore]
        [JsonPropertyName("EnderecoId")]
        public override int? AddressId { get; set; }

        [Required]
        [JsonPropertyName("Matricula")]
        public string Registration
        {
            get
            {
                return this.Document;
            }
            set
            {
                this.Document = value;
            }
        }

        [JsonPropertyName("TipoDeAcesso")]
        public override PersonRole Role
        {
            get
            {
                return PersonRole.Operator;
            }
        }
    }
}
