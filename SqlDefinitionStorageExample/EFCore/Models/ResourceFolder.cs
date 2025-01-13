﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlDefinitionStorageExample.EFCore.Models
{
    public class ResourceFolder : ResourceFolderBase
    {
    }

    public class ResourceFolderBase
    {

        [Column("Id")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        [Column("Name")]
        [Required]
        public string Name { get; set; }

        [Column("ParentUri")]
        public string ParentUri { get; set; }

        [Column("HasSubFolders")]
        public bool HasSubFolders { get; set; }

        [Column("Uri")]
        public string Uri { get; set; }

        [Column("CreatedOn")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Column("ModifiedOn")]
        public DateTime ModifiedOn { get; set; }
    }
}
